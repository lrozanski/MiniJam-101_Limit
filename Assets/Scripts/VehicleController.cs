using System;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class VehicleController : MonoBehaviour {

    [SerializeField, PropertyRange(0f, 2f)]
    private float acceleration;

    [SerializeField, PropertyRange(0f, 50f)]
    private float turnSpeed;

    [SerializeField, PropertyRange(0f, 1f)]
    private float brakingSpeed;
    
    private bool accelerating;
    private bool decelerating;

    private new Rigidbody2D rigidbody2D;

    [ShowInInspector, ReadOnly]
    private Vector2 Velocity => rigidbody2D == null ? Vector2.zero : rigidbody2D.velocity;

    protected void Start() {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    protected void Accelerate() {
        rigidbody2D.velocity += (Vector2) transform.up * acceleration;
        accelerating = true;
        decelerating = false;
    }

    protected void Decelerate() {
        rigidbody2D.velocity -= (Vector2) transform.up * brakingSpeed;
        accelerating = false;
        decelerating = true;
    }

    protected void TurnLeft() {
        if (rigidbody2D.velocity.magnitude <= 0.3f) {
            return;
        }
        rigidbody2D.AddTorque(turnSpeed * Time.deltaTime, ForceMode2D.Impulse);
        // rigidbody2D.MoveRotation(rigidbody2D.rotation + turnSpeed);
    }

    protected void TurnRight() {
        if (rigidbody2D.velocity.magnitude <= 0.3f) {
            return;
        }
        rigidbody2D.AddTorque(-turnSpeed * Time.deltaTime, ForceMode2D.Impulse);
        // rigidbody2D.MoveRotation(rigidbody2D.rotation - turnSpeed);
    }

    protected virtual void Update() {
        if (!accelerating && !decelerating && rigidbody2D.velocity.magnitude > 0.3f) {
            rigidbody2D.velocity -= (Vector2) transform.up * brakingSpeed;
        }
    }

}
