using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeonRing : MonoBehaviour
{
    public List<Color> ringCols;
    public Renderer neonRingRenderer;

    private void Start()
    {
        Color randomCol = ringCols[Random.Range(0, ringCols.Count)];
        neonRingRenderer.material.SetColor("_AtmoColor", randomCol);
    }

}
