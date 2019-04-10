﻿using System.Collections;
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

}
