using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//contains references to the top level manager (e.g. spawner, time counter, progression)

public class GameMaster : MonoBehaviour
{

    // variables
    private GameController controller;
    private GameSpawner spawner;

    // properties (access these)
    public GameController Controller
    {
        get { return controller; }
        set
        {
            controller = value;
            controller.SetupGameMasterReference(this);
        }
    }

    public GameSpawner Spawner
    {
        get { return spawner; }
        set
        {
            spawner = value;
            spawner.SetupGameMasterReference(this);
        }
    }

    void Start()
    {
        SetupReferences();
        Controller.InitialiseAll();
    }
    
    private void SetupReferences ()
    {
        Controller = GetComponent<GameController>();
        Spawner = GetComponent<GameSpawner>();
    }

}
