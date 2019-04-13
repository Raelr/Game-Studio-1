using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSpawner : MonoBehaviour
{
    private GameMaster gameMaster;

    [Header("Player Prefab")]
    [SerializeField]
    private PlayerMaster playerMaster = null;

    public GameMaster GameMaster
    {
        get { return gameMaster; }
        set { gameMaster = value; }
    }

    public void SetupGameMasterReference(GameMaster master)
    {
        GameMaster = master;
    }

    
    public void Initialise()
    {

        // initialise variables

    }
    

    public PlayerMaster SpawnPlayer ()
    {
        // Spawn Player in their original spot.
        PlayerMaster spawnedPlayer = Instantiate(playerMaster);

        spawnedPlayer.Setup(GameMaster);

        return spawnedPlayer;
        
        //when we spawn the player prefab, run the following line of code:
        //spawnedPlayer.GetComponent<PlayerMaster>().Setup(GameMaster);
        //that will ensure that the player has access to the game master
    }

}
