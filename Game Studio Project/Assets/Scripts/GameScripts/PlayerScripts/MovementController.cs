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
	private float xBounds = 100f;
	private float yBounds = 12f;

	private Transform player;

	// Initialises all variables and gets the physics component. 
	public override void Initialise() {

        base.Initialise();

        physics = GetComponent<PhysicsController>();
		player = transform.Find("Visuals");
        physics.Initialise();

        physics.onCollision += onPlayerCollision;
    }

	// DEPRECIATED -- 

	// Keeping old movement componenet as a reference

	//  public void MoveEntity(Vector2 targetPos) {
	//      if (!invertMovement)
	//      {
	//          targetPos *= -1;
	//          targetPos.y += 2;
	//      }
	//      //targetPos *= -1;
	//      //targetPos.y += 2;

	//float dist = Vector3.Distance(targetPos, player.position);
	//Vector2 dir = GlobalMethods.GetDirection(player.position, targetPos);
	//Vector2 velocity = dir * (force * (dist / maxDistance));

	//Vector3 nextPosition = (Vector2)player.position + GlobalMethods.Normalise(dir);

	//      //Clamps velocity to make sure player stays within the set bounds
	//      velocity.x = GlobalMethods.WithinBounds(nextPosition.x, -xBounds, xBounds) ? velocity.x : 0;
	//velocity.y = GlobalMethods.WithinBounds(nextPosition.y, -yBounds, yBounds) ? velocity.y : 0;

	//physics.AddForce(velocity);
	//}

	// -- DEPRECIATED

	public void RotateEntity(Vector2 input) {

        rotationX += input.x * stepRotation * Time.deltaTime * force;
        rotationY += input.y * stepRotation * Time.deltaTime * force;

        float shipRotationX = Mathf.Clamp(rotationX, minRotation, maxRotation);
        float shipRotationY = Mathf.Clamp(rotationY, minRotation, maxRotation);

        Debug.Log(-stepRotation * input.y);
        transform.localRotation = Quaternion.Euler(new Vector3(-rotationY, rotationX, 0));
        player.transform.localRotation = Quaternion.Euler(new Vector3(-stepRotation* input.y*2, stepRotation*input.x*2, -rotationX));

    }
    
    public void onPlayerCollision() {

        onCollision?.Invoke();
    }
}
