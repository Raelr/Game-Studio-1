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


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayerPrefs.SetFloat("SensitivityMultiplier", 0.4f);
            SensitivityDisplay.instance.SetDisplay(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayerPrefs.SetFloat("SensitivityMultiplier", 0.5f);
            SensitivityDisplay.instance.SetDisplay(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlayerPrefs.SetFloat("SensitivityMultiplier", 0.6f);
            SensitivityDisplay.instance.SetDisplay(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PlayerPrefs.SetFloat("SensitivityMultiplier", 0.7f);
            SensitivityDisplay.instance.SetDisplay(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            PlayerPrefs.SetFloat("SensitivityMultiplier", 0.8f);
            SensitivityDisplay.instance.SetDisplay(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            PlayerPrefs.SetFloat("SensitivityMultiplier", 0.9f);
            SensitivityDisplay.instance.SetDisplay(6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            PlayerPrefs.SetFloat("SensitivityMultiplier", 1);
            SensitivityDisplay.instance.SetDisplay(7);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            PlayerPrefs.SetFloat("SensitivityMultiplier", 1.2f);
            SensitivityDisplay.instance.SetDisplay(8);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            PlayerPrefs.SetFloat("SensitivityMultiplier", 1.5f);
            SensitivityDisplay.instance.SetDisplay(9);
        }




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

        float sensitivityMultiplier = GetSensitivityMultiplier();


        float horizontalJoyTarget = Input.GetAxis("HorizontalJoy");
        float verticalJoyTarget = Input.GetAxis("VerticalJoy");

        
        horizontalJoy += horizontalJoyChangeSpeed;
        horizontalJoyChangeSpeed += horizontalJoyStrength * (horizontalJoyTarget - horizontalJoy);
        horizontalJoyChangeSpeed *= horizontalJoyBouncy;


        verticalJoy += verticalJoyChangeSpeed;
        verticalJoyChangeSpeed += verticalJoyStrength * (verticalJoyTarget - verticalJoy);
        verticalJoyChangeSpeed *= verticalJoyBouncy;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector2 flightVector = new Vector2(horizontalInput + horizontalJoy, verticalInput + verticalJoy) * sensitivityMultiplier;
        
        master?.RotateEntity(new Vector2(flightVector.x, flightVector.y));
	}

    float GetSensitivityMultiplier ()
    {
        float sense = 1;
        if (PlayerPrefs.HasKey("SensitivityMultiplier"))
        {
            sense = PlayerPrefs.GetFloat("SensitivityMultiplier");
        }
        else
        {
            PlayerPrefs.SetFloat("SensitivityMultiplier", 1);
        }
        return sense;
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
