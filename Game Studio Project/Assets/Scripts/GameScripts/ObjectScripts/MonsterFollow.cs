using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFollow : MonoBehaviour
{


    public Transform target;
    public float smoothSpeed = 60f;

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, Time.deltaTime * smoothSpeed/100);
    }
}
