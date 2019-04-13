using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Super class for storing all methods which are relevant to all classes.
public abstract class InitialisedEntity : MonoBehaviour
{
    // Handles variable initialisation (akin to a Start() method)
    public virtual void Initialise() {
        Debug.Log(this + "Initialised");
    }
}
