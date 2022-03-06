using AI;
using UnityEngine;

namespace Towers {
    public class Projectile : MonoBehaviour {

        public int Damage { get; set; }
        
        public Vector2 Direction { get; set; }

        public float Speed { get; set; }

        private void Start() {
            GetComponent<Rigidbody2D>().velocity = Direction * Speed;
        }

        private void OnCollisionEnter2D(Collision2D col) {
            if (col.gameObject.CompareTag("Enemy")) {
                col.gameObject.GetComponent<Enemy>().TakeDamage(Damage);
            }
            Destroy(gameObject);
        }

        private void OnBecameInvisible() {
            Destroy(gameObject);
        }

    }
}
