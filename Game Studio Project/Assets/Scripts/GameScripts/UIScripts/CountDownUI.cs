using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDownUI : MonoBehaviour
{
    [SerializeField]
    private TextMesh tm, tm2, tm3;
    [SerializeField]
    private Renderer tmrend, tmrend2, tmrend3;
    private bool counting = false;
    private float duration = 2f;
    private float t = 0;


    [SerializeField]
    private Renderer galaxyBase;

    [SerializeField]
    private ParticleSystem galaxyPS;
    [SerializeField]
    private int galaxyPSCount;

    [SerializeField]
    private IntroRubberBand introRubber;

    public static CountDownUI instance;

    private void Awake()
    {
        instance = this;
    }


    IEnumerator StartCountDown()
    {
        introRubber.TurnOn();
        while (counting) {
            if (t >= duration)
            {
                StopCounting();
            }
            else {
                int num = (int)((duration - t) * 1.5f) + 1;
                tm3.text = tm2.text = tm.text = "" + num;
                t += Time.fixedDeltaTime;
            }
            yield return null;
        }
    }

    public void StopCounting() {
        counting = false;
        t = 0;
        tmrend3.enabled = tmrend2.enabled = tmrend.enabled = false;
        galaxyBase.enabled = false;
        galaxyPS.Emit(galaxyPSCount);
        StopCoroutine(StartCountDown());
        introRubber.Rubber();
    }

    public void StartCounting() {
        t = 0;
        counting = true;
        tmrend3.enabled = tmrend2.enabled = tmrend.enabled = true;
        galaxyBase.enabled = true;
        StartCoroutine(StartCountDown());
    }
}
