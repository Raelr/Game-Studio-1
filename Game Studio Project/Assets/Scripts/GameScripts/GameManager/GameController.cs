using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    private GameMaster gameMaster;

    PlayerMaster playerMaster;

    PlayerMaster PlayerMaster {

        get { return playerMaster; }
        set {

            playerMaster = value;
            SubscribePlayerDelegates();

        }
    }

    public GameSpawner Spawner { get { return GameMaster.Spawner; } }

    public GameMaster GameMaster
    {
        get { return gameMaster; }
        set { gameMaster = value; }
    }
    
    public void SetupGameMasterReference(GameMaster master)
    {
        GameMaster = master;
    }

    public void InitialiseAll()
    {
        // initialise all things through the GameMaster (that contains references to the top level managers)
        //e.g.
        // GameMaster.GUI.Initialise()
        // GameMaster.GameData.Initialise()
        // GameMaster.Spawner.Initialise()

        GameMaster.Spawner.Initialise();
        PlayerMaster = Spawner.SpawnPlayer();

        // Add new script: GameData script to store all important data points.
    }

    void SubscribePlayerDelegates() {

    }
}
