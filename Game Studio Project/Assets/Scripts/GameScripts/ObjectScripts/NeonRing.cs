using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeonRing : MonoBehaviour
{
    public List<Color> ringCols;
    public Renderer neonRingRenderer;

    public Transform neonRing;

    private void Start()
    {
        transform.localScale *= Random.Range(0.3f, 1);


        SetRingSize(0);
        Color randomCol = ringCols[Random.Range(0, ringCols.Count)];
        neonRingRenderer.material.SetColor("_AtmoColor", randomCol);
        StartCoroutine(GrowRing(1));
    }

    private IEnumerator GrowRing (float length)
    {
        float timeElapsed = 0;

        while (timeElapsed < length)
        {
            SetRingSize(timeElapsed / length);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        SetRingSize(1);
    }




    private void SetRingSize(float size)
    {
        neonRing.transform.localScale = new Vector3(size, size, size);
    }

}
