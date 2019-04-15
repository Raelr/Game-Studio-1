using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookat : MonoBehaviour
{
    public Transform target;
    public float damping;

    public void FixedUpdate()
    {
        if (target)
        {
            Transform camera = Camera.main.transform;
            Vector3 toTarget = target.position - camera.position;
            Quaternion targetRotation = Quaternion.LookRotation(toTarget);
            camera.rotation = Quaternion.Lerp(camera.rotation, targetRotation, damping);

        }
    }

    public void SetTarget (Transform newTarget)
    {
        target = newTarget;
    }
}
