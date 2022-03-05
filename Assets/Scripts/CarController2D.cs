using UnityEngine;

public class CarController2D : MonoBehaviour {

    public float acceleration;
    public float steering;
    private new Rigidbody2D rigidbody2D;

    private void Start() {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        var h = -InputManager.Instance.Actions.Horizontal.ReadValue<float>();
        var v = InputManager.Instance.Actions.Vertical.ReadValue<float>();

        Vector2 speed = transform.up * (v * acceleration);
        rigidbody2D.AddForce(speed);

        var direction = Vector2.Dot(rigidbody2D.velocity, rigidbody2D.GetRelativeVector(Vector2.up));
        if (direction >= 0.0f) {
            // rigidbody2D.rotation += h * steering * (rigidbody2D.velocity.magnitude / 5.0f);
            rigidbody2D.AddTorque((h * steering) * (rigidbody2D.velocity.magnitude / 10.0f));
        } else {
            // rigidbody2D.rotation -= h * steering * (rigidbody2D.velocity.magnitude / 5.0f);
            rigidbody2D.AddTorque((-h * steering) * (rigidbody2D.velocity.magnitude / 10.0f));
        }

        var forward = new Vector2(0.0f, 0.5f);
        var steeringRightAngle = rigidbody2D.angularVelocity > 0 ? -90 : 90;

        Vector2 rightAngleFromForward = Quaternion.AngleAxis(steeringRightAngle, Vector3.forward) * forward;
        Debug.DrawLine(rigidbody2D.position, rigidbody2D.GetRelativePoint(rightAngleFromForward), Color.green);

        var driftForce = Vector2.Dot(rigidbody2D.velocity, rigidbody2D.GetRelativeVector(rightAngleFromForward.normalized));
        var relativeForce = rightAngleFromForward.normalized * -1.0f * (driftForce * 10.0f);

        Debug.DrawLine(rigidbody2D.position, rigidbody2D.GetRelativePoint(relativeForce), Color.red);

        rigidbody2D.AddForce(rigidbody2D.GetRelativeVector(relativeForce));
    }

}
