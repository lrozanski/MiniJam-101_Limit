using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace UI {
    public class PauseMenu : MonoBehaviour {

        [SerializeField, ChildGameObjectsOnly]
        private UIDocument pauseMenu;

        private void Start() => InputManager.Instance.Actions.Pause.performed += Pause;

        private void Pause(InputAction.CallbackContext _) {
            pauseMenu.gameObject.SetActive(!pauseMenu.gameObject.activeSelf);
            Time.timeScale = Time.timeScale > 0f ? 0f : 1f;
        }

    }
}
