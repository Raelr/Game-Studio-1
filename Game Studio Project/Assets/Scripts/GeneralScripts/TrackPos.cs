using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPos : MonoBehaviour
{
    public Transform target;

    public bool isGlobal = true;

    void FixedUpdate()
    {
        if (isGlobal)
            transform.position = target.position;
        else
            transform.localPosition = target.localPosition;
    }
}
