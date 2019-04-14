using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMaster : MonoBehaviour
{
    //variables
    public static UIMaster instance;
    private UIController controller;
    private UITime time;
    private UILives lives;

    //properties (access these)
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

    public void Setup() {
        SetupReferences();
        Controller.InitialiseAll();
    }

    private void SetupReferences() {
        time = GetComponent<UITime>();
        lives = GetComponent<UILives>();
        controller = GetComponent<UIController>();
    }

    private void Awake()
    {
        instance = this;
        Setup();
    }
}
