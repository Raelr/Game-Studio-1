using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AlternativeArchitecture;

public class HomingAsteroid : MonoBehaviour
{
    private Transform target;
    public Rigidbody rigid;

    public float homingForceMin, homingForceMax;
    private float homingForce;
    

    void Start()
    {
        target = PlayerMaster.instance.shipRender.transform;

        homingForce = Random.Range(homingForceMin, homingForceMax);
    }

    private void Update()
    {
        if (target)
        {
            Vector3 force = (target.position - transform.position).normalized * homingForce * 100;
            force.z = 0;


            rigid.AddForce(force);


        }
    }
}
