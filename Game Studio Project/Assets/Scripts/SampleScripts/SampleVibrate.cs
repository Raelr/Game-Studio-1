using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SampleVibrate : MonoBehaviour
{

    public float strength;

    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            for (int i = 0; i < 4; i++)
            {
             //   HapticEngine.instance.V(i, strength);
            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
           //     HapticEngine.instance.V(i, 0);
            }
        }
        
    }

}
