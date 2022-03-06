using UI;
using UnityEngine;

namespace Towers {
    public class TowerInteraction : MonoBehaviour {

        [SerializeField]
        private Tower tower;

        private bool playerInRange;

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.CompareTag("Player")) {
                GameOverlay.Instance.ToggleBottomBar(true);
                GameOverlay.Instance.SetUpgradeCost(tower.UpgradeCost);
                GameOverlay.Instance.SetTowerDetails(GetTowerDetails());

                playerInRange = true;
            }
        }

        private string GetTowerDetails() {
            return $@"{tower.name} - Level {tower.Level}
Projectile speed: {tower.ProjectileSpeed}
Projectile damage: {tower.ProjectileDamage}
Cooldown: {tower.Cooldown}";
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (other.CompareTag("Player")) {
                GameOverlay.Instance.ToggleBottomBar(false);
                playerInRange = false;
            }
        }

        private void Update() {
            if (!playerInRange) {
                return;
            }
            if (tower.HasChanged) {
                GameOverlay.Instance.SetTowerDetails(GetTowerDetails());
            }
            if (InputManager.Instance.Actions.UpgradeTower.WasPressedThisFrame() && GameManager.Instance.CanAfford(tower)) {
                tower.Upgrade();
            }
        }

    }
}
