using AI;
using LR.Core.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Map {
    public class FinishLine : MonoBehaviour {

        [SerializeField, SceneObjectsOnly]
        private Transform carContainer;

        [SerializeField, ChildGameObjectsOnly]
        private BoxCollider2D boxCollider2D;

        [SerializeField, AssetsOnly, AssetSelector(Paths = "Assets/Prefabs")]
        private PlayerController playerPrefab;

        [SerializeField, AssetsOnly, AssetSelector(Paths = "Assets/Prefabs"), BoxGroup("Enemy Properties")]
        private EnemyController enemyPrefab;

        [SerializeField, BoxGroup("Enemy Properties")]
        private EnemyController.AI seed;

        [SerializeField, PropertyRange(0f, 0.2f), BoxGroup("Enemy Properties")]
        private float skillRandom;

        [SerializeField, PropertyRange(1, 10), BoxGroup("Enemy Properties")]
        private int enemyCars;

        private void Start() {
            CreateEnemies();
        }

        [Button]
        public void CreateEnemies() {
            const int rows = 3;
            var bounds = boxCollider2D.bounds;
            var rect = new Rect(bounds.min.x, bounds.min.y, bounds.size.x, bounds.size.y);

            for (var i = 0; i < enemyCars; i++) {
                var gridPosition = GridUtils.IndexTo2DCoords(i, rows);
                var localPosition = new Vector2(gridPosition.y, gridPosition.x);
                var position = rect.min + Vector2.one * 0.5f + localPosition;

                var enemy = Instantiate(enemyPrefab, position, Quaternion.AngleAxis(-90f, Vector3.forward), carContainer);
                var seedRandom = Random.Range(-skillRandom, skillRandom);

                enemy.Ai = seed;
                seed.accelerateAmount += seedRandom;
                seed.decelerateAmount += seedRandom;
                seed.turnAmount += seedRandom;
            }
        }

    }
}
