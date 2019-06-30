using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SensitivityDisplay : MonoBehaviour
{
    public static SensitivityDisplay instance;

    public Renderer render;
    public TextMeshPro text;
    

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public void SetDisplay (int number)
    {
        text.text = "" + number;
        StopAllCoroutines();
        StartCoroutine(Appear());
    }

    IEnumerator Appear ()
    {
        render.enabled = true;
        yield return new WaitForSeconds(0.4f);
        render.enabled = false;
    }
}
