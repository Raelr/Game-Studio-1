using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class IntroRubberBand : MonoBehaviour
{

    public PostProcessVolume introFar, introClose;
    public AnimationCurve introFarCurve, introCloseCurve;
    public float introTime;

    void Start()
    {
        TurnOff();
    }

    public void TurnOff()
    {
        introFar.weight = 0;
        introClose.weight = 0;
    }

    public void TurnOn()
    {
        EvaluateProgress(0);
    }

    public void Rubber()
    {
        StartCoroutine(RubberEffect());
    }

    IEnumerator RubberEffect ()
    {
        float et = 0;
        while (et < introTime)
        {
            EvaluateProgress(et / introTime);

            et += Time.deltaTime;
            yield return null;
        }

        EvaluateProgress(1);
    }

    //value 0 to 1
    private void EvaluateProgress (float progress)
    {
        introFar.weight = introFarCurve.Evaluate(progress);
        introClose.weight = introCloseCurve.Evaluate(progress);
    }
}
