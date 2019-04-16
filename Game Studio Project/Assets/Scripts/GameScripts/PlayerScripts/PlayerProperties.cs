using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperties : InitialisedEntity
{
    [Header("Player Speed")]
    [SerializeField]
    int lives;

    int currentLives;

    float speed;

    [Header("Player Speed")]
    float speedMultiplier;

    public delegate void OnPlayerLostGame();

    public event OnPlayerLostGame onPlayerLose;

    public override void Initialise() {

        base.Initialise();

        currentLives = lives;

        UIMaster.instance.onUIChange.Invoke("Lives: ", currentLives);
    }

    public void DecrementLives() {

        currentLives--;

        if (currentLives <= 0) {

            onPlayerLose?.Invoke();

        } else {

            UIMaster.instance.onUIChange.Invoke("Lives: ", currentLives);
        }
    }

    public void IncreaseSpeed() {


    }
}
