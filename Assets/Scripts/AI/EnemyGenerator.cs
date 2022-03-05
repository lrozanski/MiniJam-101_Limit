using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AI {
    public class EnemyGenerator : MonoBehaviour {

        [SerializeField]
        private List<Vector2> startPositions;

        [SerializeField, AssetsOnly, AssetSelector(Paths = "Assets/Prefabs")]
        private EnemyController enemyPrefab;

        [SerializeField, PropertyRange(1, 1000)]
        private int maxGenerations;

        [SerializeField, PropertyRange(1f, 100f)]
        private float timeScale;

        private readonly List<EnemyController> enemies = new();
        private int index;
        private int generation;

        private EnemyController[] previousBest = new EnemyController[0];

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

            // accelerateAmount: 0.9919808, decelerateAmount: 0.7541581, turnAmount: 0.5326825
            // accelerateAmount = 1f, decelerateAmount = 0.02117753f, turnAmount = 0.6687697f

            enemy.Ai = new EnemyController.AI {
                // accelerateAmount = previousGeneration != null
                    // ? Mathf.Clamp01(previousGeneration.Ai.accelerateAmount + Random.Range(-0.2f, 0.2f))
                    // : Random.value,
                // decelerateAmount = previousGeneration != null
                    // ? Mathf.Clamp01(previousGeneration.Ai.decelerateAmount + Random.Range(-0.2f, 0.2f))
                    // : Random.value,
                // turnAmount = previousGeneration != null
                    // ? Mathf.Clamp01(previousGeneration.Ai.turnAmount + Random.Range(-0.2f, 0.2f))
                    // : Random.value
                    accelerateAmount = 0.9913467f, decelerateAmount = 0.7942677f, turnAmount = 0.8672262f
            };

            if (generation % 10 == 0) {
                Mutate(enemy);
            }
            return enemy;
        }

        private static void Mutate(EnemyController enemy) {
            Debug.Log("Mutating enemy");

            switch (Random.Range(0, 3)) {
                case 0:
                    enemy.Ai = new EnemyController.AI {
                        accelerateAmount = Random.value,
                        decelerateAmount = enemy.Ai.decelerateAmount,
                        turnAmount = enemy.Ai.turnAmount
                    };
                    return;
                case 1:
                    enemy.Ai = new EnemyController.AI {
                        accelerateAmount = enemy.Ai.accelerateAmount,
                        decelerateAmount = Random.value,
                        turnAmount = enemy.Ai.turnAmount
                    };
                    return;
                case 2:
                    enemy.Ai = new EnemyController.AI {
                        accelerateAmount = enemy.Ai.accelerateAmount,
                        decelerateAmount = enemy.Ai.decelerateAmount,
                        turnAmount = Random.value
                    };
                    return;
            }
        }

        private async UniTaskVoid NextGeneration() {
            while (generation++ < maxGenerations) {
                GenerateEnemies();
                Debug.Log($"Starting generation {generation}");

                await UniTask.Delay(TimeSpan.FromSeconds(15));

                enemies.Sort((e1, e2) => GetScore(e2).CompareTo(GetScore(e1)));

                if (previousBest.Length == 0) {
                    previousBest = new[] { enemies[0], enemies[1] };
                    LogResult(enemies[0]);
                    LogResult(enemies[1]);
                }
                if (previousBest.Length >= 1 && GetScore(previousBest[0]) < GetScore(enemies[0])) {
                    previousBest[0] = enemies[0];
                    LogResult(enemies[0]);
                }
                if (previousBest.Length >= 2 && GetScore(previousBest[1]) < GetScore(enemies[1])) {
                    previousBest[1] = enemies[1];
                    LogResult(enemies[1]);
                }
                await UniTask.Yield();
            }

            Debug.Log("Best results:");
            LogResult(previousBest[0]);
            LogResult(previousBest[1]);

#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#endif
        }

        private void LogResult(EnemyController enemy) {
            Debug.Log($"Score: {GetScore(enemy)}, Finished in: {enemy.TimeElapsed}, collisions: {enemy.Collisions}, parameters: {enemy.Ai}");
        }

        private static int GetScore(EnemyController enemy) => Mathf.RoundToInt(
            (enemy.ReachedFinishLine ? 1000 : 0)
            + enemy.DistanceInTiles * 100
            - enemy.Collisions * 500
            - enemy.TimeElapsed * 10
        );

    }
}
