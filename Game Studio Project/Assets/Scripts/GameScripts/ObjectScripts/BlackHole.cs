using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    [SerializeField]
    private float pullForce;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private Rigidbody rb;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.tag == "Player") {
            player = other.transform;
            rb = player.GetComponent<Rigidbody>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (player != null && rb != null) {
            Vector3 dir = (transform.position - player.position).normalized;
            //not working
            rb.velocity += dir*pullForce;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") {
            player = null;
            rb = null;
        }
    }
}
