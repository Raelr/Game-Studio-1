using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookat : MonoBehaviour
{
    public Transform target;
    public float damping;
    string test = "";


    public void FixedUpdate()
    {
        if (target)
        {
            Transform camera = Camera.main.transform;
            Vector3 toTarget = target.position - camera.position;

            Quaternion targetRotation = Quaternion.LookRotation(toTarget);
            mainCamera.rotation = Quaternion.Lerp(mainCamera.rotation, targetRotation, damping * Time.deltaTime);

            test = ""+Mathf.Round(mainCamera.rotation.x*360) + ", " + Mathf.Round(mainCamera.rotation.y * 360) + ", " + Mathf.Round(mainCamera.rotation.z * 360);
        }
    }

    void OnGUI ()
    {
      //      GUI.Label(new Rect(10, 10, Screen.width, 20), test);
    }

    public void SetTarget (Transform newTarget)
    {
        target = newTarget;
    }

}
