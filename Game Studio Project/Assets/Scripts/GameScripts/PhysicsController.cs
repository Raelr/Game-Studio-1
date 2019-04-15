using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsController : InitialisedEntity
{

    [Header("Physics Collider")]
    [SerializeField]
    Collider entityCollider;

    [Header("Physics Rigidbody")]
    [SerializeField]
    Rigidbody entityRigidbody;

    [Header("Player Sensor")]
    [SerializeField]
    PlayerSensor collisionSensor;

    public delegate void OnCollisionhandler();

    public OnCollisionhandler onCollision;

    public override void Initialise() {

        base.Initialise();

        entityCollider = GetComponentInChildren<Collider>();

        entityRigidbody = GetComponentInChildren<Rigidbody>();

        collisionSensor = GetComponentInChildren<PlayerSensor>();

        collisionSensor.onCollision += OnPlayerHit;
    }

    // Adds force to the object in question.
    public void AddForce(Vector3 velocity) {

		entityRigidbody.velocity = velocity;
    }

    public void OnPlayerHit() {

        onCollision?.Invoke();

    }
}
