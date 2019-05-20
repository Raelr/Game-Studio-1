﻿using System.Collections;
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
        private Transform origin1;
        [SerializeField]
        private Transform player;
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

        private Vector3 Dir { get { return (origin1.position - player.position).normalized; } }

        public void Setup(GamePooler pooler, ObjectType type)
        {
            rigid.velocity = Vector3.zero;
            origin1 = GameObject.FindGameObjectWithTag("Player").transform;
            player = origin1.GetChild(0).transform;
            //dir = (origin1 - transform.position).normalized;

            origin = (Dir*-1) * dist + player.position;
            startMin += origin;
            startMax += origin;
            transform.position = new Vector3(Random.Range(startMin.x, startMax.x), Random.Range(startMin.y, startMax.y), Random.Range(startMin.z, startMax.z));
            transform.Rotate(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
            //transform.localScale = new Vector3(2, 2, 2);
            transform.localScale *= Random.Range(0.5f, 1f);

            gamePooler = pooler;
            objectType = type;

            isActive = true;
        }

        private Vector3 GetDir() {
            Vector3 dir = (origin1.position - player.position).normalized;
            dir.x *= -1;
            dir.y *= -1;
            return dir;
        }

        private void FixedUpdate()
        {
            if (!isActive) return;

            Vector3 velocity = (Dir * force);

            rigid.velocity = velocity;



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