using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


public class CameraEffects : MonoBehaviour
{

    public static CameraEffects instance;
    public float currentInsanity = 0;

    //DASH EFFECT
    public PostProcessVolume dashPost, insanityPost;
    private Coroutine dashRoutine;
    public float dashOnLength, dashOffLength;


    //DATA MOSH
    public bool moshEnabled = false;
    public Kino.Datamosh mosh;

    public Monster monster;



    private void Awake()
    {
        instance = this;
    }

    public void Initialise ()
    {
        DashOff();
    }




    public void ApplyInsanity (float insanity)
    {
        currentInsanity = insanity;
        if (!moshEnabled && insanity > 0.6f)
        {
            moshEnabled = true;
            EnableMosh();
        }
        else if (moshEnabled && insanity < 0.6f)
        {
            DisableMosh();
        }
        if (insanity > 0.6f)
        {
            SetMosh(insanity);
        }
        SetInsanityPost(insanity);
        //monster.MonsterReveal(insanity);
    }

    private void StopDashCoroutine ()
    {
        if (dashRoutine != null)
        {
            StopCoroutine(dashRoutine);
            dashRoutine = null;
        }
    }


    public void DashOn()
    {
        StopDashCoroutine();
        dashRoutine = StartCoroutine(ToggleDash(true));
    }

    public void DashOff()
    {
        StopDashCoroutine();
        dashRoutine = StartCoroutine(ToggleDash(false));
    }

    private IEnumerator ToggleDash (bool state)
    {
        float timeElapsed = 0;
        float length = state ? dashOnLength : dashOffLength;
        float start = dashPost.weight;
        float end = state ? 1 : 0;
        
        while (timeElapsed < length)
        {
            dashPost.weight = Mathf.Lerp(start, end, timeElapsed / length);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        dashPost.weight = end;
    }


    private void EnableMosh()
    {
        mosh.Glitch();
        mosh.enabled = true;
    }

    private void DisableMosh ()
    {
        mosh.enabled = false;
    }

    public void SetMosh (float insanity) //0 to 1
    {
        mosh.blockSize = (int)GlobalMethods.Remap(insanity, 0.5f, 1, 2, 50);
        mosh.entropy = GlobalMethods.Remap(insanity, 0.6f, 1, 0f, 0.5f);
    }
    
    public void SetInsanityPost (float insanity) //0 to 1
    {
        insanityPost.weight = GlobalMethods.Remap(insanity, 0.5f, 1f, 0, 1);
        if (insanity < 0.5f)
            insanityPost.weight = 0;
    }
    
}
