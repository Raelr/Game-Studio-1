using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
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

    // put your variables for player data here

    public void Initialise()
    {

        // initialise variables
    }

}
