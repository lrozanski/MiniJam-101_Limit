using System;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace Map {
    public class TileDirection : MonoBehaviour {

        [SerializeField, PropertyRange(0f, 360f)]
        private float angle;

        public Vector2 Direction { get; private set; }

        private void Start() {
            var radians = angle * Mathf.Deg2Rad;
            Direction = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians));
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.red;
            var position = transform.position;
            var radians = angle * Mathf.Deg2Rad;
            var direction = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians));

            var start = position - direction;
            var end = position + direction;

            var leftArrow = Quaternion.AngleAxis(135f, Vector3.forward) * direction;
            var rightArrow = Quaternion.AngleAxis(-135f, Vector3.forward) * direction;

            Gizmos.DrawLine(start, end);
            Gizmos.DrawLine(end, end + leftArrow);
            Gizmos.DrawLine(end, end + rightArrow);
        }

    }
}
