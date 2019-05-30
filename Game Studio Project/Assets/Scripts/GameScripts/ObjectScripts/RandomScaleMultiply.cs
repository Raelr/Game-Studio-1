using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomScaleMultiply : MonoBehaviour
{

    public float min, max;

    void Start()
    {
        transform.localScale *= Random.Range(min, max);
    }
    
}
