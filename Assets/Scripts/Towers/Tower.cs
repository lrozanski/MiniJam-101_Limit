using Sirenix.OdinInspector;
using UI;
using UnityEngine;

namespace Towers {
    public class Tower : MonoBehaviour {

        [SerializeField, AssetsOnly, AssetSelector(Paths = "Assets/Prefabs")]
        private Projectile projectilePrefab;

        [SerializeField]
        private LayerMask enemyLayerMask;

        [SerializeField, PropertyRange(0f, 50f)]
        private float projectileSpeed;

        [SerializeField, PropertyRange(0f, 100)]
        private int projectileDamage;

        [SerializeField, PropertyRange(0f, 2f)]
        private float cooldown;

        [SerializeField, PropertyRange(0, 100)]
        private int initialUpgradeCost;

        [ShowInInspector, ReadOnly]
        public int UpgradeCost { get; set; }

        public int Level { get; private set; } = 1;

        private CircleCollider2D circleCollider2D;

        private float currentCooldown;

        public float ProjectileSpeed => projectileSpeed;

        public float ProjectileDamage => projectileDamage;

        public float Cooldown => cooldown;

        public bool HasChanged { get; private set; }

        private void Start() {
            circleCollider2D = GetComponentInChildren<CircleCollider2D>();
            UpgradeCost = initialUpgradeCost;

            GameOverlay.Instance.SetUpgradeCost(UpgradeCost);
        }

        private void Update() {
            currentCooldown = Mathf.Max(currentCooldown - Time.deltaTime, 0f);
        }

        private void FixedUpdate() {
            if (currentCooldown > 0f) {
                return;
            }
            var enemies = Physics2D.OverlapCircleAll(transform.position, circleCollider2D.radius, enemyLayerMask);

            if (enemies.Length == 0) {
                return;
            }
            currentCooldown = cooldown;
            var enemy = enemies[Random.Range(0, enemies.Length)];

            SpawnProjectile(enemy.transform.position);
        }

        private void SpawnProjectile(Vector3 target) {
            var projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            projectile.Direction = (target - transform.position).normalized;
            projectile.Speed = projectileSpeed;
            projectile.Damage = projectileDamage;
        }

        public void Upgrade() {
            GameManager.Instance.Money -= UpgradeCost;

            Level++;
            projectileSpeed *= 1.5f;
            projectileDamage += Mathf.RoundToInt(projectileDamage / 2f);
            cooldown *= 0.8f;
            HasChanged = true;
            UpgradeCost *= 2;

            GameOverlay.Instance.SetUpgradeCost(UpgradeCost);
        }

    }
}
