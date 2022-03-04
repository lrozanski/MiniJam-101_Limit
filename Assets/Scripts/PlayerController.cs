public class PlayerController : VehicleController {

    protected override void Update() {
        base.Update();
        
        var actions = InputManager.Instance.Actions;

        if (actions.Accelerate.IsPressed()) {
            Accelerate();
        } else if (actions.Decelerate.IsPressed()) {
            Decelerate();
        }
        if (actions.TurnLeft.IsPressed()) {
            TurnLeft();
        }
        if (actions.TurnRight.IsPressed()) {
            TurnRight();
        }
    }

}
