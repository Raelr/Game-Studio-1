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
            camera.rotation = Quaternion.Lerp(camera.rotation, targetRotation, damping * Time.deltaTime);

            test = ""+Mathf.Round(camera.rotation.x*360) + ", " + Mathf.Round(camera.rotation.y * 360) + ", " + Mathf.Round(camera.rotation.z * 360);
        }
    }

    string test = "";

    void OnGUI ()
    {
            GUI.Label(new Rect(10, 10, Screen.width, 20), test);
    }

    public void SetTarget (Transform newTarget)
    {
        target = newTarget;
    }
}
