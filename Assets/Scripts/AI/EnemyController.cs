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
            public float decelerateAmount;

            [PropertyRange(0f, 1f)]
            public float turnAmount;

            public override string ToString() {
                return $"{nameof(accelerateAmount)}: {accelerateAmount}, {nameof(decelerateAmount)}: {decelerateAmount}, {nameof(turnAmount)}: {turnAmount}";
            }

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

        [SerializeField, ChildGameObjectsOnly]
        private Transform frontDetector;

        [SerializeField]
        private LayerMask trackLayerMask;

        private HashSet<Vector2Int> VisitedTiles { get; } = new();

        [ShowInInspector]
        public bool ReachedFinishLine { get; private set; }

        [ShowInInspector]
        public float TimeElapsed { get; private set; }

        [ShowInInspector]
        public float DistanceInTiles => VisitedTiles.Count;

        [ShowInInspector]
        public int Collisions { get; private set; }

        private void Update() {
            if (ReachedFinishLine) {
                return;
            }
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
            var front = Physics2D.Linecast(position, frontDetector.position, trackLayerMask);

            if (leftWallFar && !rightWallFar) {
                UpdateSteering(Ai.turnAmount);
            } else if (rightWallFar && !leftWallFar) {
                UpdateSteering(-Ai.turnAmount);
            } else if (leftWide && !rightWide) {
                UpdateSteering(Ai.turnAmount * 1.5f);
            } else if (rightWide && !leftWide) {
                UpdateSteering(-Ai.turnAmount * 1.5f);
            } else if (leftWide && rightWide) {
                var direction = leftWide.distance < rightWide.distance ? 1 : -1;
                var amount = Mathf.Lerp(0.5f, 2f, 1f - (leftWide.distance < rightWide.distance ? leftWide.fraction : rightWide.fraction));
                
                UpdateSteering(direction * Ai.turnAmount * amount);
            } else {
                UpdateSteering(0f);
            }
            if (front) {
                UpdateThrottle(Ai.accelerateAmount - Ai.decelerateAmount * Mathf.Lerp(0f, 1f, 1f - front.fraction));
            } else {
                UpdateThrottle(Ai.accelerateAmount);
            }

            base.FixedUpdate();
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (TimeElapsed <= 5f || DistanceInTiles < 5) {
                return;
            }
            if (col.CompareTag("Finish Line")) {
                ReachedFinishLine = true;
            }
        }

        private void OnCollisionEnter2D(Collision2D col) {
            Collisions++;
        }

    }
}
