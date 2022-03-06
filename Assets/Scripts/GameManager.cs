using LR.Core;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager> {

    [field: SerializeField, PropertyRange(0f, 100f)]
    public int Lives { get; set; }

    private void Update() {
        if (Lives <= 0) {
            LoseGame();
        }
    }

    private void LoseGame() {
        
    }

}
