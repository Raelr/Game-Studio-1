using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningUI : MonoBehaviour
{
    [SerializeField]
    private Renderer ind1, text, ind2;
    private bool flashing;
    private bool startState = true;
    private bool endState = false;
    private float blinkTime = 0.5f;

    IEnumerator Flash() {
        while (flashing) {
            ind1.enabled = !ind1.enabled;
            text.enabled = !text.enabled;
            ind2.enabled = !ind2.enabled;
            yield return new WaitForSeconds(blinkTime);
        }
    }

    public void StartFlashing() {
        flashing = true;
        ind1.enabled = startState;
        text.enabled = startState;
        ind2.enabled = startState;
        StartCoroutine(Flash());
    }

    public void StopFlashing() {
        flashing = false;
        StopCoroutine(Flash());
        ind1.enabled = endState;
        text.enabled = endState;
        ind2.enabled = endState;
    }

}
