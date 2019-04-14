using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    private UIMaster master;
    public UIMaster Master
    {
        get { return master; }
        set { master = value; }
    }

    public void SetUIMasterReference(UIMaster ui)
    {
        master = ui;
    }

    public void InitialiseAll ()
    {
        UIMaster.instance.Lives.Initialise();
        UIMaster.instance.Time.Initialise();      
    }
}
