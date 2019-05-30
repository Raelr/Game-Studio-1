using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpCol : MonoBehaviour
{
    public Color colA, colB;
    public Renderer render;
    public float flashSpeed = 1;

    public bool randomDelay;

    float delay;

    private void Start()
    {
        if (randomDelay) delay = Random.Range(0f, 2f);
    }

    void Update()
    {
        render.material.SetColor("_TintColor",Color.Lerp(colA, colB, Mathf.PingPong(delay+(Time.time * flashSpeed), 1)));
    }
}
