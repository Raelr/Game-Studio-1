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
	private float force = 15f;
	private float maxDistance = 8;

	private Transform player;
	private Vector2 screenBounds;

	// Initialises all variables and gets the physics component. 
	public override void Initialise() {

        base.Initialise();

        physics = GetComponent<PhysicsController>();
		player = transform.Find("Visuals");
		screenBounds.x = Screen.width / 2;
		screenBounds.y = Screen.height / 2;
        physics.Initialise();
    }

    // Makes all calculations for the physics and applies force via the physics component.
    public void MoveEntity(Vector2 targetPos) {
		//targetPos *= -1;
		//targetPos.y += 2;

		float dist = Vector3.Distance(targetPos, player.position);
		Vector2 dir = GlobalMethods.GetDirection(player.position, targetPos);
		Vector2 velocity = dir * (force * (dist / maxDistance));

		velocity.x = GlobalMethods.WithinBounds(velocity.x, -screenBounds.x, screenBounds.x) ? velocity.x : 0;
		velocity.y = GlobalMethods.WithinBounds(velocity.y, -screenBounds.y, screenBounds.y) ? velocity.y : 0;

		//player.position += new Vector3(velocity.x, velocity.y, 0);

		physics.AddForce(velocity);
    }
    
}
