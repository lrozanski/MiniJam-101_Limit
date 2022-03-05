using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AI {
    public class EnemyController : VehicleController {

        [Serializable]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public struct AI {

            [PropertyRange(0f, 1f)]
            public float accelerateAmount;

            [PropertyRange(0f, 1f)]
            public float turnAmount;

            public override string ToString() => $"{nameof(accelerateAmount)}: {accelerateAmount}, {nameof(turnAmount)}: {turnAmount}";

        }

        [ShowInInspector, ReadOnly]
        public AI Ai { get; set; }

        [SerializeField, ChildGameObjectsOnly]
        private Transform leftWallFarDetector;

        [SerializeField, ChildGameObjectsOnly]
        private Transform rightWallFarDetector;

        [SerializeField, ChildGameObjectsOnly]
        private Transform leftWideDetector;

        [SerializeField, ChildGameObjectsOnly]
        private Transform rightWideDetector;

        [SerializeField]
        private LayerMask trackLayerMask;

        public HashSet<Vector2Int> VisitedTiles { get; } = new();

        [ShowInInspector]
        public bool ReachedFinishLine { get; private set; }

        [ShowInInspector]
        public float TimeElapsed { get; private set; }

        [ShowInInspector]
        public float DistanceInTiles => VisitedTiles.Count;

        private void Update() {
            TimeElapsed += Time.deltaTime;
        }

        protected override void FixedUpdate() {
            var position = rigidbody2D.position;
            var currentTile = TilemapManager.Instance.WorldToCell(position);

            if (!VisitedTiles.Contains(currentTile)) {
                VisitedTiles.Add(currentTile);
            }

            var leftWallFar = Physics2D.Linecast(position, leftWallFarDetector.position, trackLayerMask);
            var rightWallFar = Physics2D.Linecast(position, rightWallFarDetector.position, trackLayerMask);
            var leftWide = Physics2D.Linecast(position, leftWideDetector.position, trackLayerMask);
            var rightWide = Physics2D.Linecast(position, rightWideDetector.position, trackLayerMask);

            if (leftWallFar && !rightWallFar) {
                UpdateSteering(Ai.turnAmount);
            } else if (rightWallFar && !leftWallFar) {
                UpdateSteering(-Ai.turnAmount);
            } else if (leftWide) {
                UpdateSteering(Ai.turnAmount);
            } else if (rightWide) {
                UpdateSteering(-Ai.turnAmount);
            } else {
                UpdateSteering(0f);
            }
            UpdateThrottle(Ai.accelerateAmount);

            base.FixedUpdate();
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.CompareTag("Finish Line")) {
                ReachedFinishLine = true;
            }
        }

    }
}
