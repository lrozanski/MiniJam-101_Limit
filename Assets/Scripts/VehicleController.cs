using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Borrowed heavily from https://github.com/ijidau/TopDown2DCar
/// </summary>
public abstract class VehicleController : MonoBehaviour {

    [SerializeField, PropertyRange(0f, 20f)]
    private float power;

    [SerializeField, PropertyRange(0f, 5f)]
    private float steeringPower;

    [SerializeField, PropertyRange(0f, 10f)]
    private float surfaceFriction;

    [SerializeField, PropertyRange(0f, 10f)]
    private float maxTyreFriction;

    [SerializeField, PropertyRange(0f, 10f)]
    private float skidmarkThreshold;

    [ShowInInspector, ReadOnly]
    private float steering;

    [ShowInInspector, ReadOnly]
    private float throttle;

    [ShowInInspector, ReadOnly]
    private Vector2 Velocity => rigidbody2D == null ? Vector2.zero : rigidbody2D.velocity;

    private new Rigidbody2D rigidbody2D;
    private TrailRenderer[] skidmarks;

    protected void Start() {
        rigidbody2D = GetComponent<Rigidbody2D>();
        skidmarks = GetComponentsInChildren<TrailRenderer>();
    }

    protected void UpdateThrottle(float amount) {
        throttle = amount;
    }

    protected void UpdateSteering(float amount) {
        steering = amount;
    }

    protected virtual void FixedUpdate() {
        var actualSteeringPower = Mathf.Lerp(0f, steeringPower, Mathf.Clamp01(rigidbody2D.velocity.magnitude / 2f));
        rigidbody2D.AddTorque(-steering * actualSteeringPower * Mathf.Sign(rigidbody2D.velocity.magnitude));

        var acceleration = (Vector2) transform.up * (throttle * power);
        rigidbody2D.AddForce(acceleration);

        var driftForce = new Vector2(transform.InverseTransformVector(rigidbody2D.velocity).x, 0f);
        var frictionForce = Vector2.ClampMagnitude(driftForce * -1 * surfaceFriction, maxTyreFriction);

        rigidbody2D.AddForce(rigidbody2D.GetRelativeVector(frictionForce), ForceMode2D.Force);

        // Debug.DrawLine(rigidbody2D.position, rigidbody2D.GetRelativePoint(driftForce), Color.green);
        // Debug.DrawLine(rigidbody2D.position, rigidbody2D.GetRelativePoint(frictionForce), Color.red);

        foreach (var trailRenderer in skidmarks) {
            trailRenderer.emitting = driftForce.sqrMagnitude > frictionForce.sqrMagnitude * skidmarkThreshold;
        }
    }

}
