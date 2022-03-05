public class PlayerController : VehicleController {

    protected void Update() {
        if (!InputManager.Instance.PlayerInput.inputIsActive) {
            return;
        }
        var actions = InputManager.Instance.Actions;

        UpdateSteering(actions.Horizontal.ReadValue<float>());
        UpdateThrottle(actions.Vertical.ReadValue<float>());
    }

}
