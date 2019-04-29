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

    const float insanityDecaySpeed = 0.5f;

    const float maxSanity = 9f;

    const float impactSanityDamage = 8f;

    float currentSanity;

    public delegate void OnPlayerLostGame();

    public event OnPlayerLostGame onPlayerLose;

    public override void Initialise() {

        base.Initialise();

        currentSanity = 9f;

        currentLives = lives;

        UIMaster.instance.onUIChange.Invoke("Lives: ", currentLives);
    }

    public void OnPlayerHit()
    {
        DecrementLives();
        DecaySanityConstant();

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

    public void DecaySanityConstant() {

        if (currentSanity > 0) {

            float projectedSanity = currentSanity - insanityDecaySpeed;

            currentSanity = Mathf.Lerp(currentSanity, projectedSanity, insanityDecaySpeed * Time.deltaTime);

            UIMaster.instance.onMeterChange.Invoke(insanityDecaySpeed);

        } else {

            onPlayerLose?.Invoke();
        }
    }

    public void DecaySanityByAmount()
    {
        if (currentSanity > 0)
        {

            Debug.LogWarning(currentSanity);

            float projectedSanity = currentSanity - impactSanityDamage;

            currentSanity = Mathf.Lerp(currentSanity, projectedSanity, impactSanityDamage * Time.deltaTime);

            Debug.LogWarning(currentSanity);

            UIMaster.instance.onMeterChange.Invoke(impactSanityDamage);

        }
        else
        {

            onPlayerLose?.Invoke();
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
