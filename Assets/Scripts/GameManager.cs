using LR.Core;
using Sirenix.OdinInspector;
using Towers;
using UI;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager> {

    [SerializeField, PropertyRange(0f, 100)]
    private int lives;

    [SerializeField, PropertyRange(0f, 100)]
    private int enemiesLeft;

    [SerializeField]
    private int money;
    
    private int maxLives;

    public bool IsGameOver => lives <= 0 || enemiesLeft <= 0;
    
    public int Lives {
        get => lives;
        set {
            lives = value;
            GameOverlay.Instance.SetLives(lives, maxLives);
        }
    }
    
    public int EnemiesLeft {
        get => enemiesLeft;
        set {
            enemiesLeft = value;
            GameOverlay.Instance.SetEnemiesLeft(enemiesLeft);
        }
    }
    
    public int Money {
        get => money;
        set {
            money = value;
            GameOverlay.Instance.SetMoney(money);
        }
    }

    private void Start() {
        maxLives = lives;
        Lives = lives;
        Money = money;
    }

    public void RemoveLife() {
        Lives = Mathf.Max(Lives - 1, 0);

        if (Lives == 0) {
            GameOver();
        }
    }

    private static void Victory() {
        GameOverlay.Instance.Victory();
        Time.timeScale = 0f;
    }

    private static void GameOver() {
        GameOverlay.Instance.GameOver();
        Time.timeScale = 0f;
    }

    public void SubtractEnemyNumber() {
        EnemiesLeft = Mathf.Max(EnemiesLeft - 1, 0);

        if (EnemiesLeft == 0) {
            Victory();
        }
    }

    public bool CanAfford(Tower tower) => money >= tower.UpgradeCost;

}
