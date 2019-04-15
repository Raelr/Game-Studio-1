using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperties : InitialisedEntity
{
    [SerializeField]
    int lives;

    int currentLives;

    public override void Initialise() {

        base.Initialise();

        currentLives = lives;

        UIMaster.instance.onUIChange.Invoke("Lives: ", currentLives);
    }

    public void DecrementLives() {

        currentLives--;

        if (currentLives <= 0) {
            Debug.Log("Death");
        } else {
            UIMaster.instance.onUIChange.Invoke("Lives: ", currentLives);
        }
    }
}
