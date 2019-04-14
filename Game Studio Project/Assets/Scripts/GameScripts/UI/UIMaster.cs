using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMaster : MonoBehaviour
{
    //variables
    private PlayerMaster playerMaster;
    private UIController controller;
    private UITime time;
    private UILives lives;

    //properties (access these)
    public PlayerMaster PlayerMaster {
        get { return playerMaster; }
        set { playerMaster = value; }
    }

    public UIController Controller {
        get { return controller; }
        set {
            controller = value;
            controller.SetUIMasterReference(this);
        }
    }

    public UITime Time {
        get { return time; }
        set {
            time = value;
            time.SetUIMasterReference(this);
        }
    }

    public UILives Lives {
        get { return lives; }
        set {
            lives = value;
            lives.SetUIMasterReference(this);
        }
    }

    public void Setup(PlayerMaster master) {
        SetupPlayerMasterReference(master);
        SetupReferences();
        Controller.InitialiseAll();
    }

    private void SetupPlayerMasterReference(PlayerMaster master) {
        playerMaster = master;
    }

    private void SetupReferences() {
        controller = GetComponent<UIController>();
        time = GetComponent<UITime>();
        lives = GetComponent<UILives>();
    }
}
