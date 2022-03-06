using System;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Towers {
    public class Tower : MonoBehaviour {

        [SerializeField, AssetsOnly, AssetSelector(Paths = "Assets/Prefabs")]
        private Projectile projectilePrefab;

        [SerializeField]
        private LayerMask enemyLayerMask;

        [SerializeField, PropertyRange(0f, 50f)]
        private float projectileSpeed;

        [SerializeField, PropertyRange(0f, 2f)]
        private float cooldown;

        private CircleCollider2D circleCollider2D;
        private float currentCooldown;

        private void Start() {
            circleCollider2D = GetComponentInChildren<CircleCollider2D>();
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
            var projectile = Instantiate(projectilePrefab, transform.position, quaternion.identity);
            projectile.Direction = (target - transform.position).normalized;
            projectile.Speed = projectileSpeed;
        }

    }
}
