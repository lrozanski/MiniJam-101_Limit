using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AI {
    public class EnemyGenerator : MonoBehaviour {

        [SerializeField]
        private List<Vector2> startPositions;

        [SerializeField, AssetsOnly, AssetSelector(Paths = "Assets/Prefabs")]
        private EnemyController enemyPrefab;

        [SerializeField, PropertyRange(1, 1000)]
        private int maxGenerations;

        [SerializeField, PropertyRange(1f, 20f)]
        private float timeScale;

        private readonly List<EnemyController> enemies = new();
        private int index;

        private EnemyController[] previousBest = Array.Empty<EnemyController>();

        private void Start() {
            Time.timeScale = timeScale;
            NextGeneration().Forget();
        }

        private void OnDisable() {
            Time.timeScale = 1f;
        }

        private void GenerateEnemies() {
            enemies.ForEach(enemy => Destroy(enemy.gameObject));
            enemies.Clear();
            var bestIndex = 0;

            for (var i = 0; i < startPositions.Count; i++) {
                if (previousBest.Length <= 0) {
                    enemies.Add(GenerateEnemy());
                    continue;
                }
                var enemyBase = previousBest[bestIndex++];

                if (bestIndex >= previousBest.Length) {
                    bestIndex = 0;
                }
                enemies.Add(GenerateEnemy(enemyBase));
            }
        }

        private EnemyController GenerateEnemy([CanBeNull] EnemyController previousGeneration = null) {
            if (index >= startPositions.Count) {
                index = 0;
            }
            var enemy = Instantiate(enemyPrefab, transform, false);
            enemy.transform.position = startPositions[index++];
            enemy.Ai = new EnemyController.AI {
                accelerateAmount = previousGeneration != null ? previousGeneration.Ai.accelerateAmount + Random.Range(-0.2f, 0.2f) : Random.value,
                turnAmount = previousGeneration != null ? previousGeneration.Ai.turnAmount + Random.Range(-0.2f, 0.2f) : Random.value
            };
            return enemy;
        }

        private async UniTaskVoid NextGeneration() {
            var generation = 0;

            while (generation++ < maxGenerations) {
                GenerateEnemies();
                await UniTask.Delay(TimeSpan.FromSeconds(15));

                enemies.Sort((e1, e2) => GetScore(e2).CompareTo(GetScore(e1)));

                if (previousBest.Length >= 1 && GetScore(previousBest[0]) < GetScore(enemies[0])) {
                    previousBest[0] = enemies[0];
                    Debug.Log(enemies[0].Ai);
                }
                if (previousBest.Length >= 2 && GetScore(previousBest[1]) < GetScore(enemies[1])) {
                    previousBest[1] = enemies[1];
                    Debug.Log(enemies[1].Ai);
                }
                await UniTask.Yield();
            }
        }

        private static int GetScore(EnemyController enemy) => Mathf.RoundToInt(
            (enemy.ReachedFinishLine ? 1000 : 0)
            + enemy.DistanceInTiles * 100
            - enemy.TimeElapsed * 10
        );

    }
}
