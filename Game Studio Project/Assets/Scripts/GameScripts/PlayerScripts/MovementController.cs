using System.Collections;
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
    [SerializeField] private bool invertMovement;
    private Vector3 lastPosition;

	[Header("Player Bounds")]
	private float xBounds = 10f;
	private float yBounds = 6f;

	private Transform player;


    private bool stunned;
    //how long stun lasts
    private float stunDur;
    //how long since we were stunned
    private float currStun;

	// Initialises all variables and gets the physics component. 
	public override void Initialise() {

        base.Initialise();

        physics = GetComponent<PhysicsController>();
		player = transform.Find("Visuals");
        stunned = false;
        stunDur = 0;
        currStun = 0;
        physics.Initialise();

        physics.onCollision += onPlayerCollision;
    }

    // Makes all calculations for the physics and applies force via the physics component.
    public void MoveEntity(Vector2 targetPos) {
        if (!stunned)
        {
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
        else {
            if (currStun >= stunDur)
            {
                stunned = false;
                stunDur = 0;
                currStun = 0;
            }
            else {
                currStun += Time.deltaTime;
            }
        }
    }

    public void Stun(float time) {
        stunned = true;
        stunDur = time;
        currStun = 0;
    }

    public void onPlayerCollision() {

        onCollision?.Invoke();
    }
}
