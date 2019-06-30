using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullLight : MonoBehaviour
{
    private Renderer render;

    private void Start()
    {
        render = GetComponent<Renderer>();
        SetOff();
    }

    public void SetOn ()
    {
        render.material.color = Color.red;
    }

    public void SetOff()
    {
        render.material.color = Color.black;
    }
}
