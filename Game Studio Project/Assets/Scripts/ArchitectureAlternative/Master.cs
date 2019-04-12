using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A super class for all master entities. 
public abstract class Master : InitialisedEntity
{


    // Gets all appropriate components from its subordinate scripts. 
    public virtual void SetUpReferences() {

    }

    // Initialises all subordinate scripts. 
    public virtual void InitialiseAll() {

    }

    // Abstract class for handling input from another script (such as the Input Manager)
    public virtual void ClickEvent() {

    }

    public virtual void MoveToward(Vector3 moveDirection) {

    }
}
