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

        GetMouseInput();
    }

    void GetMouseInput() {

        if (Input.GetKeyDown(KeyCode.Mouse0)) {

            // Call the master's input delegate.
            master.ProcessInput();
        }
    }
}
