using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AI {
    public class EnemyGenerator : MonoBehaviour {

        [SerializeField]
        private List<Vector2> startPositions;

        [SerializeField, AssetsOnly, AssetSelector(Paths = "Assets/Prefabs")]
        private EnemyController enemyPrefab;

        private readonly List<EnemyController> enemies = new();
        private int index;

        private void Start() {
            enemies.Clear();
            
            for (var i = 0; i < startPositions.Count; i++) {
                enemies.Add(GenerateEnemy());
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

    }
}
