using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private GameMaster gameMaster;

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
        
    }


    

}
