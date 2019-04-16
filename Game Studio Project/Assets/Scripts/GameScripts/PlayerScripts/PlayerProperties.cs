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
        StartCoroutine(FlashPlaceholder()); // PLACEHOLDER DELETE ME RIGHT FUCKING NOW

        if (currentLives <= 0) {

            onPlayerLose?.Invoke();

        } else {
            UIMaster.instance.onUIChange.Invoke("Lives: ", currentLives);
        }
    }

    public void IncreaseSpeed() {


    }


    GameObject flashPlane; // PLACEHOLDER DELETE ME 
    void Start ()
    {
        flashPlane = GameObject.Find("[FLASH]");  // PLACEHOLDER DELETE ME 
        flashPlane.GetComponent<Renderer>().enabled = false;  // PLACEHOLDER DELETE ME 
    }  // PLACEHOLDER DELETE ME 

    IEnumerator FlashPlaceholder ()  // PLACEHOLDER DELETE ME 
    {
        flashPlane.GetComponent<Renderer>().enabled = true;  // PLACEHOLDER DELETE ME 
        yield return new WaitForSeconds(0.1f);  // PLACEHOLDER DELETE ME 
        flashPlane.GetComponent<Renderer>().enabled = false;  // PLACEHOLDER DELETE ME 
    }  // PLACEHOLDER DELETE ME 
}
