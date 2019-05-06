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

    private float inputX;
    private Vector2 inputDir;

	// Initialises all variables and gets the physics component. 
	public override void Initialise() {

        base.Initialise();

        physics = GetComponent<PhysicsController>();
		player = transform.Find("Visuals");
        physics.Initialise();

        physics.onCollision += onPlayerCollision;
    }

    public void FixedUpdate() {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        inputDir = new Vector2(inputX, inputY);
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

        //Vector2 dir = GlobalMethods.GetDirection(player.position, targetPos*-1);

        rotationX += inputDir.x * stepRotation * Time.deltaTime * force;
        rotationY += inputDir.y * stepRotation * Time.deltaTime * force;

        //Debug.Log(rotationX);

        float shipRotationX = Mathf.Clamp(rotationX, minRotation, maxRotation);
        float shipRotationY = Mathf.Clamp(rotationY, minRotation, maxRotation);

        Debug.Log(-stepRotation * inputDir.y);
        transform.localRotation = Quaternion.Euler(new Vector3(-rotationY, rotationX, 0));
        player.transform.localRotation = Quaternion.Euler(new Vector3(-stepRotation*inputDir.y*2, stepRotation*inputDir.x*2, -rotationX));
        //Debug.Log(rotationX);
    }

    public void JoystickMovement(Vector2 targetPos) {
        float dist = Vector3.Distance(targetPos, player.position);
        Vector2 velocity = inputDir * force * Time.deltaTime *100;

        Vector3 nextPosition = (Vector2)player.position + GlobalMethods.Normalise(inputDir);

        //Clamps velocity to make sure player stays within the set bounds
        velocity.x = GlobalMethods.WithinBounds(nextPosition.x, -xBounds, xBounds) ? velocity.x : 0;
        velocity.y = GlobalMethods.WithinBounds(nextPosition.y, -yBounds, yBounds) ? velocity.y : 0;

        physics.AddForce(velocity);
    }

    public void onPlayerCollision() {

        onCollision?.Invoke();
    }
}
