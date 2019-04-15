using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UILives : InitialisedEntity
{
    [SerializeField]
    int lives = 0;

    int currentLives;

    int CurrentLives { get { return currentLives; } set { currentLives = value; UpdateLives(); } }

    [SerializeField]
    TextMeshPro livesText;

    public override void Initialise() {

        base.Initialise();

        CurrentLives = lives;
    }

    public void UpdateLives() {

        if (livesText.text != null) {

            livesText.text = "Lives: " + CurrentLives;
        }    
    }

    public void DecrementLives() {

        int projectedLives = CurrentLives--;

        if (projectedLives > 0) {

            CurrentLives = currentLives--;
        } else {
            Debug.Log("Death");
        }
    }
}
