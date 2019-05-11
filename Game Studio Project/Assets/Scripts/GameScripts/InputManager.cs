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
    }

    // Listens for input.
    void GetMouseInput() {

        if (Input.GetKeyDown(KeyCode.Mouse0)) {

            // Call the master's input delegate.
            master?.ClickEvent();
        }

        Vector3 targetPoint = -GetMousePosition() + GetDistortVector();

        master?.MoveToward(targetPoint);
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
