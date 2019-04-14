using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
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



    // put your variables for player physics here

    public void Initialise()
    {

        // initialise variables

    }

    private void OnCollisionEnter(Collision collision)
    {
        playerMaster.Controller.Hit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) { playerMaster.Controller.Hit(); }
    }
}
