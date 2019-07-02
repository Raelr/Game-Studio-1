using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullLight : MonoBehaviour
{
    private Renderer render;

    public Transform meter;
    public float minLight;
    public AudioSource skullSound;
    public float pitch;
    public AudioClip skullBadClip;
    public AudioClip skullGoodClip;

    private bool glowing = false, startedGlowing = false, stoppingGlowing = false;

    private void Start()
    {
        render = GetComponent<Renderer>();
        SetOff();
    }

    private void Update()
    {
        if (shouldGlow() && !glowing && !startedGlowing)
        {
            startedGlowing = true;
            StartCoroutine(EnableGlow());
        }
        if (!shouldGlow() && glowing && !stoppingGlowing)
        {
            stoppingGlowing = true;
            StartCoroutine(DisableGlow());
        }
    }

    IEnumerator EnableGlow ()
    {
        skullSound.clip = skullBadClip;
        skullSound.pitch = pitch;
        skullSound.Play();
        float et = 0;
        while (et < 0.3f)
        {
            SetGlow(et / 0.3f);
            et += Time.deltaTime;
            yield return null;
        }
        SetOn();
        startedGlowing = false;
        glowing = true;
    }

    IEnumerator DisableGlow()
    {
      ///  skullSound.clip = skullGoodClip;
      ///  skullSound.pitch = 1;
      ///  skullSound.Play();
        float et = 0;
        while (et < 0.5f)
        {
            SetGlow(et / 0.5f);
            et += Time.deltaTime;
            yield return null;
        }
        SetOff();
        stoppingGlowing = false;
        glowing = false;
    }

    public void SetOn ()
    {
        SetGlow(1);
    }

    public void SetOff()
    {
        SetGlow(0);
    }

    private void SetGlow (float progress)
    {
        render.material.color = Color.Lerp(Color.black, Color.red, progress);
    }

    private bool shouldGlow ()
    {
        return meter.localScale.x < minLight;
    }
}
