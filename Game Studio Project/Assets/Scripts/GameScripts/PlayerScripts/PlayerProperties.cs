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

    const float impactSanityDamage = 5f;

    float currentSanity;

    [SerializeField]
    float sanityDodgeIncrease = 0f;

    bool isDecaying = true;

    public delegate void OnPlayerLostGame();

    public event OnPlayerLostGame onPlayerLose;

    public delegate void SoundChangedHandler(float volume);

    public event SoundChangedHandler OnSoundChanged;

    public override void Initialise() {

        base.Initialise();

        currentSanity = 9f;
    }

    public void OnPlayerHit()
    {
        DecaySanityConstant();
    }

    public void DecaySanityConstant() {

        if (currentSanity > 0)
        {

            currentSanity = Mathf.Lerp(currentSanity, currentSanity - insanityDecaySpeed, insanityDecaySpeed * Time.deltaTime);

            UIMaster.instance.onMeterChange.Invoke(insanityDecaySpeed);

            float normalisedSanity = 1f - (currentSanity / maxSanity);
            CameraEffects.instance.ApplyInsanity(normalisedSanity);

            OnSoundChanged?.Invoke(normalisedSanity);
        }
        else
        {
            onPlayerLose?.Invoke();
        }
    }

    public void DecaySanityByAmount()
    {

        if (isDecaying) {
            float projectedSanity = Mathf.Lerp(currentSanity, currentSanity - impactSanityDamage, impactSanityDamage * Time.deltaTime);

            if (projectedSanity > 0)
            {
                currentSanity = projectedSanity;

                UIMaster.instance.onMeterChange.Invoke(impactSanityDamage);

                float normalisedSanity = 1f - (currentSanity / maxSanity);

                OnSoundChanged?.Invoke(normalisedSanity);
            }
            else
            {
                onPlayerLose?.Invoke();
            }
        }
    }

    public void ImproveSanity()
    {
        float sanity = Mathf.Lerp(currentSanity, currentSanity + sanityDodgeIncrease, sanityDodgeIncrease * Time.deltaTime);

        if (sanity >= maxSanity)
        {
            sanity = maxSanity;
        } 

        if (isDecaying) {
            isDecaying = false;
            if (sanity < maxSanity)
            {
                currentSanity = sanity;

                UIMaster.instance.onMeterChange.Invoke(sanityDodgeIncrease, true);

                float normalisedSanity = 1f - (currentSanity / maxSanity);

                OnSoundChanged?.Invoke(normalisedSanity);
            }
        }
        isDecaying = true;
    }
}
