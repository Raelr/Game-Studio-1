using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles all actions which the entity can take. This is an example of a player's specific controller.
public class MovementController : InitialisedEntity {

    // The controller should keep track of all physics components (since it is the only component which needs to interface with physics)
    [Header("Physics")]
    [SerializeField]
    PhysicsController physics;

    // Initialises all variables and gets the physics component. 
    public override void Initialise() {

        base.Initialise();

        physics = GetComponent<PhysicsController>();

        physics.Initialise();
    }

    // Makes all calculations for the physics and applies force via the physics component.
    public void MoveEntity(Vector3 velocity) {

        physics.AddForce(velocity);
    }
}
