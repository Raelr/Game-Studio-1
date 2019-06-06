using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionIndicator : MonoBehaviour
{

    public Transform camTransform;
    public GameObject leftDir, rightDir, upDir, downDir;
    public float minThreshold = 55, maxThreshold = 180;

    void Update()
    {
        Vector3 angle = camTransform.eulerAngles;

        leftDir.SetActive(angle.y > minThreshold && angle.y < maxThreshold);
        rightDir.SetActive(angle.y < 360- minThreshold && angle.y > 360- maxThreshold);
        upDir.SetActive(angle.x > minThreshold && angle.x < maxThreshold);
        downDir.SetActive(angle.x < 360 - minThreshold && angle.x > 360 - maxThreshold);
    }
}
