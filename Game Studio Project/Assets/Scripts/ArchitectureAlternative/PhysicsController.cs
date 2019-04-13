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

    public override void Initialise() {

        base.Initialise();

        entityCollider = GetComponentInChildren<Collider>();

        entityRigidbody = GetComponentInChildren<Rigidbody>();
    }

    // Adds force to the object in question.
    public void AddForce(Vector3 velocity) {

        Debug.Log(velocity);
    }
}
