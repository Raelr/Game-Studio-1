using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDownUI : MonoBehaviour
{
    [SerializeField]
    private TextMesh tm;
    [SerializeField]
    private Renderer tmrend;
    private bool counting = false;
    private float duration = 3.0f;
    private float t = 0;

    IEnumerator StartCountDown() {
        while (counting) {
            if (t >= duration)
            {
                StopCounting();
            }
            else {
                int num = (int)(duration - t) + 1;
                tm.text = "" + num;
                t += Time.fixedDeltaTime;
            }
            yield return null;
        }
    }

    public void StopCounting() {
        counting = false;
        t = 0;
        tmrend.enabled = false;
        StopCoroutine(StartCountDown());
    }

    public void StartCounting() {
        t = 0;
        counting = true;
        tmrend.enabled = true;
        StartCoroutine(StartCountDown());
    }
}
