﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomScalar : MonoBehaviour
{
    public Vector3 minScale, maxScale;

    private void Start()
    {
        transform.localScale = new Vector3(Random.Range(minScale.x, maxScale.x), Random.Range(minScale.y, maxScale.y), Random.Range(minScale.z, maxScale.z));
    }


}
