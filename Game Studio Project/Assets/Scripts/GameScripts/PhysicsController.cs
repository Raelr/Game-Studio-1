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

	public delegate void OnNearMissHandler();

    public delegate void OnPlayerCollectHanlder();
    
	public delegate void OnRingHitHandler();

    public OnCollisionhandler onCollision;
    public OnPlayerCollectHanlder onPlayerCollect;
	public OnNearMissHandler onNearMiss;
	public OnRingHitHandler onRingHit;

    public override void Initialise() {

        base.Initialise();

        entityCollider = GetComponentInChildren<Collider>();

        entityRigidbody = GetComponentInChildren<Rigidbody>();

        collisionSensor = GetComponentInChildren<PlayerSensor>();

        collisionSensor.onCollision += OnPlayerHit;

		collisionSensor.onNearMiss += OnPlayerNearMiss;
        
		collisionSensor.onRingHit += OnPlayerRingHit;

        collisionSensor.onPlayerCollect += OnPlayerCollect;
    }

    // Adds force to the object in question.
    public void AddForce(Vector3 velocity) {

		entityRigidbody.velocity = velocity;
    }

    public void OnPlayerHit() {

        onCollision?.Invoke();

    }

    public void OnPlayerCollect() {
        onPlayerCollect?.Invoke();
    }

	public void OnPlayerNearMiss() {
		onNearMiss?.Invoke();
	}

	public void OnPlayerRingHit() {
		onRingHit?.Invoke();
	}
}
