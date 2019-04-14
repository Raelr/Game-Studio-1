using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLives : MonoBehaviour
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
    private int lives;

    public void Initialise()
    {

        // initialise variables
        lives = 3;
    }

    public int GetLives() {
        return lives;
    }

    public void DecreaseLives() {
        lives--;
    }
}
