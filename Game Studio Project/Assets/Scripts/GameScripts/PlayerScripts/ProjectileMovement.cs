using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    [SerializeField]
    float timeAlive = 3;
    float t = 0;
    [SerializeField]
    float speed = 20;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (t == 1)
        {
            Destroy(gameObject);
        }
        else {
            t += Time.deltaTime / timeAlive;
        }
        rb.AddForce(Vector3.forward * speed, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Obstacle") {
            //"delete" asteroid; ask sean how i'm suppose to do this
            Destroy(gameObject);
        }
    }
}
