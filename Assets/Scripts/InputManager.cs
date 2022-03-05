using JetBrains.Annotations;
using LR.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : SingletonMonoBehaviour<InputManager> {

    public class PlayerInputActions {

        public InputAction Horizontal { get; } = Instance.PlayerInput.actions["Horizontal"];
        public InputAction Vertical { get; } = Instance.PlayerInput.actions["Vertical"];
        public InputAction Pause { get; } = Instance.PlayerInput.actions["Pause"];

    }

    [UsedImplicitly]
    [field: SerializeField, ChildGameObjectsOnly]
    public PlayerInput PlayerInput { get; set; }

    public PlayerInputActions Actions { get; private set; }

    private void Start() => Actions = new PlayerInputActions();

}
