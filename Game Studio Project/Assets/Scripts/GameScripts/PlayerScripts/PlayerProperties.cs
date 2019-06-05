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

    public float InsanityDecaySpeed { get { return insanityDecaySpeed * timeMultiplier; } set { timeMultiplier = value; } }

    const float insanityDecaySpeed = 0.5f;

    const float maxSanity = 9f;

    const float impactSanityDamage = 5f;

    float currentSanity;

    public float timeMultiplier = 1f;

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

        if (isDecaying) {
            if (currentSanity > 0) {
                currentSanity = Mathf.Lerp(currentSanity, currentSanity - InsanityDecaySpeed, InsanityDecaySpeed * Time.deltaTime);

                UIMaster.instance.onMeterChange.Invoke(9 - currentSanity);

                float normalisedSanity = 1f - (currentSanity / maxSanity);
                CameraEffects.instance.ApplyInsanity(normalisedSanity);

                OnSoundChanged?.Invoke(normalisedSanity);
            } else {
                onPlayerLose?.Invoke();
            }
        }
    }

    public void DecaySanityByAmount()
    {
        if (isDecaying) {
            float projectedSanity = Mathf.Lerp(currentSanity, currentSanity - impactSanityDamage, impactSanityDamage * Time.deltaTime);

            if (projectedSanity > 0)
            {
                currentSanity = projectedSanity;

                UIMaster.instance.onMeterChange.Invoke(9 - currentSanity);

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
            if (sanity <= maxSanity)
            {
                currentSanity = Mathf.Clamp(sanity, 0, maxSanity);

                Debug.Log(currentSanity);

                UIMaster.instance.onMeterChange.Invoke(9 - currentSanity, true);

                float normalisedSanity = 1f - (currentSanity / maxSanity);

                OnSoundChanged?.Invoke(normalisedSanity);
            }
        }
        isDecaying = true;
    }
}
