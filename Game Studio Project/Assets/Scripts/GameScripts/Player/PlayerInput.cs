using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerMaster playerMaster;

    public PlayerMaster PlayerMaster
    {
        get { return playerMaster; }
        set { playerMaster = value; }
    }

    public void SetupPlayerMasterReference(PlayerMaster master)
    {
        PlayerMaster = master;
    }
    
    // put your variables for player input here
    public void Initialise ()
    {

        // initialise variables

    }

    private void FixedUpdate()
    {
        //example:
        if (Input.GetMouseButtonDown(0))
        {
            PlayerMaster.Controller.Fire();
        }
    }

    Vector2 GetMouseCoordinates() {

        Vector3 mouseCoordinates = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 mouseCoordinates2D = new Vector2(mouseCoordinates.x, mouseCoordinates.y);

        return mouseCoordinates2D;
    }
}
