using AI;
using LR.Core.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Map {
    public class FinishLine : MonoBehaviour {

        private const int Rows = 3;

        [SerializeField, SceneObjectsOnly]
        private Transform carContainer;

        [SerializeField, ChildGameObjectsOnly]
        private BoxCollider2D boxCollider2D;

        [SerializeField, AssetsOnly, AssetSelector(Paths = "Assets/Prefabs"), BoxGroup("Enemy Properties")]
        private EnemyController enemyPrefab;

        [SerializeField, BoxGroup("Enemy Properties")]
        private EnemyController.AI seed;

        [SerializeField, PropertyRange(0f, 0.2f), BoxGroup("Enemy Properties")]
        private float skillRandom;

        [SerializeField, PropertyRange(1, 10), BoxGroup("Enemy Properties")]
        private int enemyCars;

        private Rect spawnArea;
        
        private void Start() {
            var bounds = boxCollider2D.bounds;
            spawnArea = new Rect(bounds.min.x, bounds.min.y, bounds.size.x, bounds.size.y);
            
            SpawnEnemies();
        }

        private void SpawnEnemies() {
            for (var i = 0; i < enemyCars; i++) {
                SpawnEnemy(i);
            }
        }

        public void SpawnEnemy(int index) {
            var gridPosition = GridUtils.IndexTo2DCoords(index, Rows);
            var localPosition = new Vector2(gridPosition.y, gridPosition.x);
            var position = spawnArea.min + Vector2.one * 0.5f + localPosition;

            var enemy = Instantiate(enemyPrefab, position, Quaternion.AngleAxis(-90f, Vector3.forward), carContainer);
            var seedRandom = Random.Range(-skillRandom, skillRandom);

            enemy.Ai = seed;
            seed.accelerateAmount += seedRandom;
            seed.decelerateAmount += seedRandom;
            seed.turnAmount += seedRandom;

            enemy.StartDelay = Random.value * 0.5f;
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (Time.timeSinceLevelLoad < 5f) {
                return;
            }
            if (col.CompareTag("Enemy")) {
                GameManager.Instance.RemoveLife();
            }
        }

    }
}
