using LR.Core;
using Sirenix.OdinInspector;
using UI;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager> {

    [SerializeField, PropertyRange(0f, 100)]
    private int lives;

    private int maxLives;

    public bool IsGameOver => lives <= 0;
    
    public int Lives {
        get => lives;
        set {
            lives = value;
            GameOverlay.Instance.SetLives(lives, maxLives);
        }
    }

    private void Start() {
        maxLives = lives;
        Lives = lives;
    }

    public void RemoveLife() {
        Lives = Mathf.Max(Lives - 1, 0);

        if (Lives == 0) {
            GameOver();
        }
    }

    private static void GameOver() {
        GameOverlay.Instance.GameOver();
        Time.timeScale = 0f;
    }

}
