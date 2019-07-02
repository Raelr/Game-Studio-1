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

    bool isFrozen = false;

    public delegate void OnPlayerLostGame();

    public event OnPlayerLostGame onPlayerLose;

    public delegate void SoundChangedHandler(float volume);

    public event SoundChangedHandler OnSoundChanged;

    private WarningUI warn;

    private bool godMode;

    public override void Initialise() {

        base.Initialise();
        warn = GameObject.Find("WarningUI").GetComponent<WarningUI>();
        currentSanity = 9f;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.F1) && Input.GetKey(KeyCode.F2) && Input.GetKey(KeyCode.F3))
        {
            godMode = true;
        }
    }

    public void OnPlayerHit()
    {
        DecaySanityConstant();
    }

    public void DecaySanityConstant() {

        if (isDecaying && !isFrozen) {
            if (currentSanity > 0) {
                currentSanity = Mathf.Lerp(currentSanity, currentSanity - InsanityDecaySpeed, InsanityDecaySpeed * Time.deltaTime);

                UIMaster.instance.onMeterChange.Invoke(9 - currentSanity);

                float normalisedSanity = 1f - (currentSanity / maxSanity);
                CameraEffects.instance.ApplyInsanity(normalisedSanity);

                OnSoundChanged?.Invoke(normalisedSanity);

                if (normalisedSanity >= 0.7f) {
                    if (!warn.IsFlashing()) {
                        warn.StartFlashing();
                    }
                }
                
            } else if (!godMode) {
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

                if (normalisedSanity >= 0.7f)
                {
                    if (!warn.IsFlashing())
                    {
                        warn.StartFlashing();
                    }
                }
            }
            else if (!godMode)
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

                UIMaster.instance.onMeterChange.Invoke(9 - currentSanity, true);

                float normalisedSanity = 1f - (currentSanity / maxSanity);

                OnSoundChanged?.Invoke(normalisedSanity);

                if (normalisedSanity < 0.7f)
                {
                    if (warn.IsFlashing())
                    {
                        warn.StopFlashing();
                    }
                }
            }
        }
        isDecaying = true;
    }

    public void FreezeInsantiyMeter(float t) {
        StartCoroutine(Freeze(t));
    }

    private IEnumerator Freeze(float t) {
        float elapsedTime = 0;
        isFrozen = true;

        while (elapsedTime < t) {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isFrozen = false; 
    }
}
