using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlternativeArchitecture
{
    public class Obstacle : MonoBehaviour
    {
        //placeholder script - will be replaced with obstacle master

        private bool isActive;

        [SerializeField]
        private Transform target;
        [SerializeField]
        private Vector3 startMin, startMax;
        [SerializeField]
        private Vector3 origin;
        [SerializeField]
        private float force;
        [SerializeField]
        private float dist = 400;
        [SerializeField]
        private Vector3 dir;

        [SerializeField]
        private Rigidbody rigid;

        private GamePooler gamePooler;
        private ObjectType objectType;

        [SerializeField]
        private float zDespawn;

        public void Setup(GamePooler pooler, ObjectType type)
        {
            rigid.velocity = Vector3.zero;
            target = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0);
            dir = (target.position - transform.position).normalized;
            origin = dir * dist + target.position;
            startMin += origin;
            startMax += origin;
            transform.position = new Vector3(Random.Range(startMin.x, startMax.x), Random.Range(startMin.y, startMax.y), Random.Range(startMin.z, startMax.z));
            transform.Rotate(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
            transform.localScale = new Vector3(2, 2, 2);
            transform.localScale *= Random.Range(0.1f, 1f);

            gamePooler = pooler;
            objectType = type;

            isActive = true;
        }

        private void FixedUpdate()
        {
            if (!isActive) return;
            rigid.AddForce(-dir*force);

            /*if (transform.position.z < zDespawn)
            {
                //isActive = false;
                //gamePooler.PoolObject(objectType, gameObject);
                BackToPool();
            }*/
        }


        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player") {

            }
                //Debug.Log("YOWSERS! ");
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Despawner") {
                BackToPool();
            }
        }

        public void BackToPool() {
            isActive = false;
            gamePooler.PoolObject(objectType, gameObject);
        }
    }
}