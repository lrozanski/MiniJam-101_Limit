using JetBrains.Annotations;
using LR.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Map {
    public class TilemapManager : SingletonMonoBehaviour<TilemapManager> {

        [UsedImplicitly]
        [field: SerializeField, SceneObjectsOnly]
        public Tilemap Tilemap { get; set; }

        [SerializeField]
        private Vector3Int startTile;

        public Vector2Int WorldToCell(Vector3 worldPosition) => (Vector2Int) Tilemap.WorldToCell(worldPosition);

    }
}
