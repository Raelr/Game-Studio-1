﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles all actions which the entity can take. This is an example of a player's specific controller.
public class MovementController : InitialisedEntity {

    public delegate void OnCollisionhandler();

    public OnCollisionhandler onCollision;

    // The controller should keep track of all physics components (since it is the only compone≤nt which needs to interface with physics)
    [Header("Physics")]
    [SerializeField]
    PhysicsController physics;

	[Header("Physics Properties")]
	[SerializeField] private float force = 25f;
    [SerializeField] private float maxDistance = 8;
    [SerializeField] private bool invertMovement = false;
    [SerializeField] private float minRotation = -30;
    [SerializeField] private float maxRotation = 30;
    [SerializeField] private float stepRotation = 0.1f;
    private float rotationX;
    private float rotationY;
    private Vector3 lastPosition;

	[Header("Player Bounds")]
	private float xBounds = 10f;
	private float yBounds = 6f;

	private Transform player;

	// Initialises all variables and gets the physics component. 
	public override void Initialise() {

        base.Initialise();

        physics = GetComponent<PhysicsController>();
		player = transform.Find("Visuals");
        physics.Initialise();

        physics.onCollision += onPlayerCollision;
    }

    // Makes all calculations for the physics and applies force via the physics component.
    public void MoveEntity(Vector2 targetPos) {
        if (!invertMovement)
        {
            targetPos *= -1;
            targetPos.y += 2;
        }
        //targetPos *= -1;
        //targetPos.y += 2;

        float dist = Vector3.Distance(targetPos, player.position);
		Vector2 dir = GlobalMethods.GetDirection(player.position, targetPos);
		Vector2 velocity = dir * (force * (dist / maxDistance));

		Vector3 nextPosition = (Vector2)player.position + GlobalMethods.Normalise(dir);

		//Clamps velocity to make sure player stays within the set bounds
		velocity.x = GlobalMethods.WithinBounds(nextPosition.x, -xBounds, xBounds) ? velocity.x : 0;
		velocity.y = GlobalMethods.WithinBounds(nextPosition.y, -yBounds, yBounds) ? velocity.y : 0;

		physics.AddForce(velocity);
    }

    public void RotateEntity(Vector2 targetPos) {
        //float inputx = targetpos.x * -1;
        //float inputy = targetpos.y * -1;
        //float rotation = inputX < 0 ? -30 : 30;

        Vector2 dir = (targetPos*-1) - (Vector2)player.transform.position;
        dir.Normalize();

        rotationX += dir.x * stepRotation;
        rotationY += dir.y * stepRotation;

        Debug.Log(rotationX);

        rotationX = Mathf.Clamp(rotationX, minRotation, maxRotation);
        rotationY = Mathf.Clamp(rotationY, minRotation, maxRotation);



        player.transform.rotation = Quaternion.Euler(new Vector3(0,rotationX,0));
        Debug.Log(rotationX);
    }

    public void onPlayerCollision() {

        onCollision?.Invoke();
    }
}
