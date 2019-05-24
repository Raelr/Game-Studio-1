using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stunner : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player") {
            collision.transform.GetComponentInParent<MovementController>().Stun(3);
        }
    }
}
