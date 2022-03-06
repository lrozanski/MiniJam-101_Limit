using UnityEngine;

namespace Towers {
    public class Projectile : MonoBehaviour {

        public Vector2 Direction { get; set; }

        public float Speed { get; set; }

        private void Start() {
            GetComponent<Rigidbody2D>().velocity = Direction * Speed;
        }

        private void OnCollisionEnter2D(Collision2D col) {
            Destroy(gameObject);
        }

    }
}
