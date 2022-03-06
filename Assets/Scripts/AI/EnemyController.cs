using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Map;
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

        private RaycastHit2D leftWallFar;
        private RaycastHit2D rightWallFar;
        private RaycastHit2D leftWide;
        private RaycastHit2D rightWide;
        private RaycastHit2D front;

        private Vector2 tileDirection;
        private float angleToTileDirection;

        private void Update() {
            if (ReachedFinishLine) {
                return;
            }
            TimeElapsed += Time.deltaTime;
        }

        protected override void FixedUpdate() {
            angleToTileDirection = Vector2.SignedAngle(transform.up, tileDirection);

            var position = rigidbody2D.position;
            var currentTile = TilemapManager.Instance.WorldToCell(position);

            if (!VisitedTiles.Contains(currentTile)) {
                VisitedTiles.Add(currentTile);
            }

            leftWallFar = Physics2D.Linecast(position, leftWallFarDetector.position, trackLayerMask);
            rightWallFar = Physics2D.Linecast(position, rightWallFarDetector.position, trackLayerMask);
            leftWide = Physics2D.Linecast(position, leftWideDetector.position, trackLayerMask);
            rightWide = Physics2D.Linecast(position, rightWideDetector.position, trackLayerMask);
            front = Physics2D.Linecast(position, frontDetector.position, trackLayerMask);

            CalculateSteering();
            CalculateThrottle();
            CorrectCourse();

            base.FixedUpdate();
        }

        private void CalculateSteering() {
            if (leftWallFar && !rightWallFar) {
                UpdateSteering(Ai.turnAmount);
            } else if (rightWallFar && !leftWallFar) {
                UpdateSteering(-Ai.turnAmount);
            } else if (leftWide && !rightWide) {
                UpdateSteering(Ai.turnAmount * 1.5f);
            } else if (rightWide && !leftWide) {
                UpdateSteering(-Ai.turnAmount * 1.5f);
            } else {
                UpdateSteering(0f);
            }
        }

        private void CalculateThrottle() {
            if (front && front.distance < 1f) {
                switch (angleToTileDirection) {
                    case > 0f:
                        UpdateSteering(-Ai.turnAmount);
                        break;
                    case < 0f:
                        UpdateSteering(Ai.turnAmount);
                        break;
                }

                UpdateThrottle(-Ai.decelerateAmount);
            } else if (front) {
                UpdateThrottle(Ai.accelerateAmount - Ai.decelerateAmount * Mathf.Lerp(0f, 1f, 1f - front.fraction));
            } else {
                UpdateThrottle(Ai.accelerateAmount);
            }
        }

        private void CorrectCourse() {
            if (front.distance < 1f) {
                return;
            }
            switch (angleToTileDirection) {
                case > 90f:
                    UpdateSteering(-Ai.turnAmount);
                    break;
                case < -90f:
                    UpdateSteering(Ai.turnAmount);
                    break;
            }
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.CompareTag("Tile Direction")) {
                tileDirection = col.GetComponent<TileDirection>().Direction;
            }
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
