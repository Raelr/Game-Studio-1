using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperties : InitialisedEntity
{
    [Header("Player Speed")]
    [SerializeField]
    float speed;

    [Header("Player Speed")]
    float speedMultiplier;

    const float insanityDecaySpeed = 0.5f;

    const float maxSanity = 9f;

    const float impactSanityDamage = 4f;

    float currentSanity;

    public delegate void OnPlayerLostGame();

    public event OnPlayerLostGame onPlayerLose;

    public override void Initialise() {

        base.Initialise();

        currentSanity = 9f;
    }

    public void OnPlayerHit()
    {
        DecaySanityConstant();

    }


    public void DecaySanityConstant() {

        float projectedSanity = currentSanity - insanityDecaySpeed;

        if (projectedSanity > 0) { 

            currentSanity = Mathf.Lerp(currentSanity, projectedSanity, insanityDecaySpeed * Time.deltaTime);

            UIMaster.instance.onMeterChange.Invoke(insanityDecaySpeed);

        } else {

            onPlayerLose?.Invoke();
        }
    }

    public void DecaySanityByAmount()
    {
        float projectedSanity = Mathf.Lerp(currentSanity, currentSanity - impactSanityDamage, impactSanityDamage * Time.deltaTime);

        if (projectedSanity > 0)
        {
            currentSanity = projectedSanity;

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
}
