using Sirenix.OdinInspector;
using UnityEngine;

namespace AI {
    public class Enemy : MonoBehaviour {

        [field: SerializeField, PropertyRange(0f, 100f)]
        public int HP { get; set; }

        public void TakeDamage(int damage) {
            HP = (int) Mathf.Max(HP - damage, 0f);

            if (HP <= 0f) {
                Destroy(gameObject);
            }
        }

    }
}
