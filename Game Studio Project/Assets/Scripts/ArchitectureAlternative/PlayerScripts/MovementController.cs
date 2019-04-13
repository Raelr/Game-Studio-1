﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles all actions which the entity can take. This is an example of a player's specific controller.
public class MovementController : InitialisedEntity {

    // The controller should keep track of all physics components (since it is the only component which needs to interface with physics)
    [Header("Physics")]
    [SerializeField]
    PhysicsController physics;

	[Header("Physics Properties")]
	private float force = 1f;
	private float maxDistance = 8;

	// Initialises all variables and gets the physics component. 
	public override void Initialise() {

        base.Initialise();

        physics = GetComponent<PhysicsController>();

        physics.Initialise();
    }

    // Makes all calculations for the physics and applies force via the physics component.
    public void MoveEntity(Vector2 targetPos) {
		targetPos *= -1;
		targetPos.y += 2;

		float dist = Vector3.Distance(targetPos, transform.position);
		Vector2 dir = GetDirection(transform.position, targetPos);
		Vector2 velocity = dir * (force * (dist / maxDistance));
		transform.position += new Vector3(velocity.x, velocity.y, 0);

		physics.AddForce(velocity);
    }

	/// <summary>
	/// Gets the direction of point A to B.
	/// </summary>
	/// <returns>The direction of two points</returns>
	private Vector2 GetDirection(Vector2 a, Vector2 b) {
		return (b - a).normalized;
	}

}
