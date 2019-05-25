using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    [SerializeField]
    private float pullForce;
    [SerializeField]
    private Transform player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            player = other.transform;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (player != null) {
            Vector3 dir = (transform.position - player.position).normalized;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") {
            player = null;
        }
    }
}
