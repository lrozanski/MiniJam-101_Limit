public class PlayerController : VehicleController {

    private void Update() {
        var actions = InputManager.Instance.Actions;

        UpdateSteering(actions.Horizontal.ReadValue<float>());
        UpdateThrottle(actions.Vertical.ReadValue<float>());
    }

}
