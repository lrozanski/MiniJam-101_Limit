using UI;
using UnityEngine;

namespace Towers {
    public class TowerInteraction : MonoBehaviour {

        [SerializeField]
        private Tower tower;

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.CompareTag("Player")) {
                GameOverlay.Instance.ToggleBottomBar(true);

                var details = GetTowerDetails();
                GameOverlay.Instance.SetTowerDetails(details);
            }
        }

        private string GetTowerDetails() {
            return $@"
{tower.name}
Projectile speed: {tower.ProjectileSpeed}
Projectile damage: {tower.ProjectileDamage}
Cooldown: {tower.Cooldown}
";
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (other.CompareTag("Player")) {
                GameOverlay.Instance.ToggleBottomBar(false);
            }
        }

    }
}
