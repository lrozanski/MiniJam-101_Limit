using LR.Core;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI {
    [DefaultExecutionOrder(-1)]
    public class GameOverlay : SingletonMonoBehaviour<GameOverlay> {

        private UIDocument document;
        private VisualElement root;

        private VisualElement gameOverPanel;
        private Label money;
        private Label lives;
        private VisualElement bottomBar;
        private Label towerDetails;

        private void Start() {
            document = GetComponent<UIDocument>();
            root = document.rootVisualElement;

            gameOverPanel = root.Q<VisualElement>("GameOverPanel");
            money = root.Q<Label>("Money");
            lives = root.Q<Label>("Lives");
            bottomBar = root.Q<VisualElement>("BottomBar");
            towerDetails = root.Q<Label>("TowerDetails");

            gameOverPanel.style.display = DisplayStyle.None;
            bottomBar.style.display = DisplayStyle.None;
        }

        public void GameOver() => gameOverPanel.style.display = DisplayStyle.Flex;
        public void SetMoney(int amount) => money.text = $"Money: {amount}";
        public void SetLives(int amount, int max) => lives.text = $"Lives: {amount} / {max}";
        public void SetTowerDetails(string details) => towerDetails.text = details;

        public void ToggleBottomBar(bool show) => bottomBar.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;

    }
}
