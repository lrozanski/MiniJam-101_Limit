using FMODUnity;
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

    [SerializeField, ChildGameObjectsOnly]
    private StudioEventEmitter accelerateEvent;

    [SerializeField, ChildGameObjectsOnly]
    private StudioEventEmitter stopEvent;

    [SerializeField, ChildGameObjectsOnly]
    private StudioEventEmitter skidmarkEvent;

    [SerializeField, ChildGameObjectsOnly]
    private StudioEventEmitter crashEvent;

    protected new Rigidbody2D rigidbody2D;
    private TrailRenderer[] skidmarks;

    private bool playStopEvent;

    protected virtual void Start() {
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

        if (throttle > 0 && !accelerateEvent.IsPlaying()) {
            if (stopEvent.IsPlaying()) {
                stopEvent.Stop();
            }
            accelerateEvent.Play();
            playStopEvent = true;
        } else if (throttle <= 0 && accelerateEvent.IsPlaying()) {
            accelerateEvent.Stop();

            if (playStopEvent && !stopEvent.IsPlaying()) {
                stopEvent.Play();
                playStopEvent = false;
            }
        }

        var driftForce = new Vector2(transform.InverseTransformVector(rigidbody2D.velocity).x, 0f);
        var frictionForce = Vector2.ClampMagnitude(driftForce * -1 * surfaceFriction, maxTyreFriction);

        rigidbody2D.AddForce(rigidbody2D.GetRelativeVector(frictionForce), ForceMode2D.Force);

        var showSkidmarks = driftForce.sqrMagnitude > frictionForce.sqrMagnitude * skidmarkThreshold;

        foreach (var trailRenderer in skidmarks) {
            trailRenderer.emitting = showSkidmarks;
        }
        if (showSkidmarks && !skidmarkEvent.IsPlaying()) {
            skidmarkEvent.Play();
        } else if (!showSkidmarks && skidmarkEvent.IsPlaying()) {
            skidmarkEvent.Stop();
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D col) {
        crashEvent.Play();
    }

}
