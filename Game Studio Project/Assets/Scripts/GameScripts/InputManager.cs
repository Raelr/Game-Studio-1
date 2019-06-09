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
        if (Input.GetButtonDown("Fire1")) {
            // Call the master's input delegate.
            master?.ClickEvent();
        }
    }

    /*
    float horizontalJoy = 0;
    float verticalJoy = 0;

    float horizontalJoyDrift = 15f, verticalJoyDrift = 15f;*/

    float verticalJoy = 0;
    float verticalJoyChangeSpeed = 0;

    float verticalJoyBouncy = 0.8f;
    float verticalJoyStrength = 0.1f;

    float horizontalJoy = 0;
    float horizontalJoyChangeSpeed = 0;

    float horizontalJoyBouncy = 0.8f;
    float horizontalJoyStrength = 0.1f;

    // Sets movement from the horizontal and vertical Axis
    private void SetAxisMovment() {
    
        float horizontalJoyTarget = Input.GetAxis("HorizontalJoy");
        float verticalJoyTarget = Input.GetAxis("VerticalJoy");

        
        horizontalJoy += horizontalJoyChangeSpeed;
        horizontalJoyChangeSpeed += horizontalJoyStrength * (horizontalJoyTarget - horizontalJoy);
        horizontalJoyChangeSpeed *= horizontalJoyBouncy;


        verticalJoy += verticalJoyChangeSpeed;
        verticalJoyChangeSpeed += verticalJoyStrength * (verticalJoyTarget - verticalJoy);
        verticalJoyChangeSpeed *= verticalJoyBouncy;

        /*
        if (Mathf.Abs(horizontalJoyInput) > 0.1f)
            horizontalJoy = horizontalJoyInput;
        else
            horizontalJoy -= (horizontalJoy / horizontalJoyDrift);

        if (Mathf.Abs(verticalJoyInput) > 0.1f)
            verticalJoy = verticalJoyInput;
        else
            verticalJoy -= (verticalJoy / verticalJoyDrift);*/





        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector2 flightVector = new Vector2(horizontalInput + horizontalJoy, verticalInput + verticalJoy);
        
        master?.RotateEntity(new Vector2(flightVector.x, flightVector.y));
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
