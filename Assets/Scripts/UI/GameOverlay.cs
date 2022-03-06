using LR.Core;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI {
    [DefaultExecutionOrder(-1)]
    public class GameOverlay : SingletonMonoBehaviour<GameOverlay> {

        private UIDocument document;
        private VisualElement root;

        private VisualElement gameOverPanel;
        private VisualElement victoryPanel;
        private Label money;
        private Label lives;
        private Label enemiesLeft;
        private VisualElement bottomBar;
        private Label upgradeCost;
        private Label towerDetails;

        private void Start() {
            document = GetComponent<UIDocument>();
            root = document.rootVisualElement;

            gameOverPanel = root.Q<VisualElement>("GameOverPanel");
            victoryPanel = root.Q<VisualElement>("VictoryPanel");
            money = root.Q<Label>("Money");
            lives = root.Q<Label>("Lives");
            enemiesLeft = root.Q<Label>("EnemiesLeft");
            bottomBar = root.Q<VisualElement>("BottomBar");
            upgradeCost = root.Q<Label>("UpgradeCost");
            towerDetails = root.Q<Label>("TowerDetails");

            gameOverPanel.style.display = DisplayStyle.None;
            victoryPanel.style.display = DisplayStyle.None;
            bottomBar.style.display = DisplayStyle.None;
        }

        public void GameOver() => gameOverPanel.style.display = DisplayStyle.Flex;

        public void Victory() => victoryPanel.style.display = DisplayStyle.Flex;

        public void SetMoney(int amount) => money.text = $"$Money: {amount}";

        public void SetLives(int amount, int max) => lives.text = $"Lives: {amount} / {max}";

        public void SetEnemiesLeft(int number) => enemiesLeft.text = $"Enemies Left: {number}";
        public void SetUpgradeCost(int amount) => upgradeCost.text = $"${amount}";

        public void SetTowerDetails(string details) => towerDetails.text = details;

        public void ToggleBottomBar(bool show) => bottomBar.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;

    }
}
