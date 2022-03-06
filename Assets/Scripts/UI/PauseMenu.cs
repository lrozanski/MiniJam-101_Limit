using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI {
    public class PauseMenu : MonoBehaviour {

        private UIDocument pauseMenu;
        private bool initialized;

        private void Start() {
            pauseMenu = GetComponentInChildren<UIDocument>(true);

            InputManager.Instance.Actions.Pause.performed += Pause;
        }

        private void OnDisable() {
            InputManager.Instance.Actions.Pause.performed -= Pause;
        }

        private void Update() {
            if (!initialized && pauseMenu.gameObject.activeSelf) {
                pauseMenu.rootVisualElement.Q<Button>("Restart").clicked += () => {
                    Time.timeScale = 1f;
                    SceneManager.LoadScene("Main", LoadSceneMode.Single);
                };
                pauseMenu.rootVisualElement.Q<Button>("Exit").clicked += Application.Quit;

                initialized = true;
            }
        }

        private void Pause(InputAction.CallbackContext _) {
            pauseMenu.gameObject.SetActive(!pauseMenu.gameObject.activeSelf);

            if (!GameManager.Instance.IsGameOver) {
                Time.timeScale = Time.timeScale > 0f ? 0f : 1f;
            } else {
                pauseMenu.gameObject.SetActive(true);
            }
        }

    }
}
