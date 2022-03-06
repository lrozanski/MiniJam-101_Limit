using Map;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AI {
    public class Enemy : MonoBehaviour {

        public int Index { get; set; }

        [field: SerializeField, PropertyRange(0f, 100f)]
        public int HP { get; set; }

        public float TimeSinceSpawn { get; private set; }

        [field: SerializeField, PropertyRange(0, 100)]
        public int MoneyForKill { get; set; }
        
        private void Update() {
            TimeSinceSpawn += Time.deltaTime;
        }

        public void TakeDamage(int damage) {
            HP = (int) Mathf.Max(HP - damage, 0f);

            if (HP <= 0f) {
                GameManager.Instance.Money += MoneyForKill;
                GameManager.Instance.SubtractEnemyNumber();
                FinishLine.Instance.SpawnEnemy(Index);
                Destroy(gameObject);
            }
        }

    }
}
