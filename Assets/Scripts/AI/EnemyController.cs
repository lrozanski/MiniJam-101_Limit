using System;
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

        }

        public AI Ai { get; set; }

        [SerializeField, ChildGameObjectsOnly]
        private Transform leftWallFarDetector;

        [SerializeField, ChildGameObjectsOnly]
        private Transform rightWallFarDetector;

        [SerializeField]
        private LayerMask trackLayerMask;

        protected override void FixedUpdate() {
            var position = rigidbody2D.position;

            var leftWallFar = Physics2D.Linecast(position, leftWallFarDetector.position, trackLayerMask);
            var rightWallFar = Physics2D.Linecast(position, rightWallFarDetector.position, trackLayerMask);

            if (leftWallFar && !rightWallFar) {
                UpdateSteering(Ai.turnAmount);
            } else if (rightWallFar && !leftWallFar) {
                UpdateSteering(-Ai.turnAmount);
            }
            UpdateThrottle(Ai.accelerateAmount);

            base.FixedUpdate();
        }

    }
}
