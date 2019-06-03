using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;


public enum HapticEffect { NEON_BOOST, DASH_FIRE, HIT, APPROACH_DEATH, ESCAPE_DEATH, APPROACH_DEATH_CLOSE }

public class HapticEngine : MonoBehaviour
{
    
    public static HapticEngine instance;
    
    private Coroutine currentRoutine;
    
    [System.Serializable]
    public struct HapticEffectSettings
    {
        public HapticEffect effect;
        public bool isEnabled;
        public float delay;
        public float length;
        public float strength;
    }

    [SerializeField]
    public List<HapticEffectSettings> effectSettings;


    public bool finalMode = false;

    private void Start()
    {
        instance = this;
        V(0);
    }

    public void Vibrate (HapticEffect effect)
    {
        if (finalMode == true)
        {
            if (effect == HapticEffect.ESCAPE_DEATH)
            {
                finalMode = false;
            }
            else if (effect != HapticEffect.APPROACH_DEATH && effect != HapticEffect.APPROACH_DEATH_CLOSE)
            {
                return;
            }
        }

        if (effect == HapticEffect.APPROACH_DEATH || effect == HapticEffect.APPROACH_DEATH_CLOSE)
            finalMode = true;

        HapticEffectSettings retreivedSettings = GetEffectSettings(effect);

        if (!retreivedSettings.isEnabled) return;

        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
            V(0);
        }

        currentRoutine = StartCoroutine(VibrateTime(retreivedSettings.delay, retreivedSettings.length, retreivedSettings.strength));
    }

    private HapticEffectSettings GetEffectSettings (HapticEffect effect)
    {
        foreach (HapticEffectSettings effectSetting in effectSettings)
        {
            if (effectSetting.effect == effect) return effectSetting;
        }
        return new HapticEffectSettings();
    }

    private IEnumerator VibrateTime (float delay, float length, float strength)
    {
        yield return new WaitForSeconds(delay);
        V(strength);
        yield return new WaitForSeconds(length);
        V(0);
    }

    public void TurnOffVibration ()
    {
        V(0);
    }
    
    private void V (float strength)
    {
        for (int playerID = 0; playerID < 4; playerID++)
        {
            GamePad.SetVibration((PlayerIndex)((int)playerID), strength, strength);
        }
    }
}
