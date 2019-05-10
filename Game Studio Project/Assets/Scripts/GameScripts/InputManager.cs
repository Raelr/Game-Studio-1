using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Input manager script handles all input related scripts.
public class InputManager : MonoBehaviour
{
    // A master class for the input to interface with.
    [Header("Master")]
    [SerializeField]
    Master master;

	private float horizontalInput;
	private float verticalInput;

	public float HorizontalInput { get { return horizontalInput; } }
	public float VerticalInput { get { return verticalInput; } }

	// Find master.
	private void Awake() {

        master = GetComponent<Master>();
    }

    private void Update() {

        master?.OnUIChange();
    }

    private void FixedUpdate() {
        GetMouseInput();
		SetAxisMovment();
    }

    // Listens for input.
    void GetMouseInput() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            // Call the master's input delegate.
            master?.ClickEvent();
        }
        master?.MoveToward(-GetMousePosition());
    }

	// Sets movement from the horizontal and vertical Axis
	private void SetAxisMovment() {
		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis("Vertical");

		master?.RotateEntity(new Vector2(horizontalInput, verticalInput));
	}

	// Gets the mouse position and returns its screen point.
	Vector3 GetMousePosition() {

		return Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10.0f));
    }

    Vector3 GetDistortVector ()
    {
        Vector3 distortVector = new Vector3();
        distortVector = Random.insideUnitSphere * CameraEffects.instance.currentInsanity * 2;
        distortVector.z = -10.0f;
        


        return distortVector;
    }
}
