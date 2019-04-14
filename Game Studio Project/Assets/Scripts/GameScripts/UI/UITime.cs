using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITime : MonoBehaviour
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

    //put variables here

    public void Initialise()
    {
        //initialise variables
    }
}
