using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMaster : MonoBehaviour
{
    // variables
    private GameMaster gameMaster;
    private PlayerController controller;
    private PlayerInput input;
    private PlayerEffects effects;


    // properties (access these)
    public GameMaster GameMaster
    {
        get { return gameMaster; }
        set
        {
            gameMaster = value;
        }
    }

    public PlayerController Controller
    {
        get { return controller; }
        set
        {
            controller = value;
            controller.SetupPlayerMasterReference(this);
        }
    }
    
    public PlayerInput Input
    {
        get { return input; }
        set
        {
            input = value;
            input.SetupPlayerMasterReference(this);
        }
    }

    public PlayerEffects Effects
    {
        get { return effects; }
        set
        {
            effects = value;
            effects.SetupPlayerMasterReference(this);
        }
    }


    public void Setup(GameMaster master)
    {
        //sets up a reference to the top level
        SetupGameMasterReference(master);

        //sets up the references on the player level
        SetupReferences();

        //tells the controller to initialise everything (e.g. the input, effects, data, ...)
        Controller.InitialiseAll();
    }
    
    private void SetupGameMasterReference (GameMaster master)
    {
        gameMaster = master;
    }

    private void SetupReferences()
    {
        Controller = GetComponent<PlayerController>();
        Input = GetComponent<PlayerInput>();
        Effects = GetComponent<PlayerEffects>();
    }


}
