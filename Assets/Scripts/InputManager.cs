using System;
using LR.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : SingletonMonoBehaviour<InputManager> {

    public class PlayerInputActions {

        public InputAction Accelerate = Instance.PlayerInput.actions["Accelerate"];
        public InputAction Decelerate = Instance.PlayerInput.actions["Decelerate"];
        public InputAction TurnLeft = Instance.PlayerInput.actions["Turn Left"];
        public InputAction TurnRight = Instance.PlayerInput.actions["Turn Right"];
        public InputAction Pause = Instance.PlayerInput.actions["Pause"];

    }

    [field: SerializeField, ChildGameObjectsOnly]
    public PlayerInput PlayerInput { get; set; }

    public PlayerInputActions Actions { get; private set; }

    private void Start() {
        Actions = new PlayerInputActions();
    }

}
