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
        private Transform origin1;
        [SerializeField]
        private Transform player;
        [SerializeField]
        private Vector3 startMin, startMax;
        [SerializeField]
        private Vector3[] spawns;
        [SerializeField]
        private float spacing;
        [SerializeField]
        private float force;
        [SerializeField]
        private float dist = 400;
        [SerializeField]
        private Vector3 dir;
        [SerializeField]
        private bool randomRotate = true;

        public float forceMultiplier = 1;

        [SerializeField]
        private Rigidbody rigid;

        private GamePooler gamePooler;
        private ObjectType objectType;

        [SerializeField]
        private float zDespawn;

        private Vector3 Dir { get { return (origin1.position - player.position).normalized; } }
        public float Force { get { return force; } set { force = value; } }

        Renderer ren;

        public void Setup(GamePooler pooler, ObjectType type)
        {
            if (ren == null)
            {
                ren = GetComponentInChildren<Renderer>();
            }
            rigid.velocity = Vector3.zero;
            origin1 = GameObject.FindGameObjectWithTag("Player").transform;
            player = origin1.GetChild(0).transform;

            //dir = (origin1 - transform.position).normalized;
            spawns = new Vector3[5];
            spawns[0] = (Dir * -1) * dist + player.position;
            spawns[1] = (GetDir(origin1.position + origin1.up * spacing) * -1) * dist + player.position;
            spawns[2] = (GetDir(origin1.position + (-origin1.up) * spacing) * -1) * dist + player.position;
            spawns[3] = (GetDir(origin1.position + origin1.right * spacing) * -1) * dist + player.position;
            spawns[4] = (GetDir(origin1.position + (-origin1.right) * spacing) * -1) * dist + player.position;
            Vector3 mySpawn = spawns[Random.Range(0,5)];

            startMin += origin1.position;
            startMax += mySpawn;

            transform.position = new Vector3(Random.Range(startMin.x, startMax.x), Random.Range(startMin.y, startMax.y), Random.Range(startMin.z, startMax.z));
            if (randomRotate) transform.Rotate(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
            //transform.localScale = new Vector3(2, 2, 2);
            //transform.localScale *= Random.Range(0.5f, 1f);

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

        private Vector3 GetDir(Vector3 pos) {
            return (pos - player.position).normalized;
        }

        private void FixedUpdate()
        {
           
            if (!isActive) {
                return;
            }
            else {
                if (Vector3.Distance(Vector3.zero, transform.position) > 50f && GetDir(transform.position).z<0) {
                    BackToPool();
                }
            }

            // rigid.AddForce(Dir * force * forceMultiplier);

            Vector3 velocity = (Dir * force * forceMultiplier);

            rigid.velocity = velocity;
        }


        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player") {

            }

        }

        public void BackToPool() {
            isActive = false;
            gamePooler.PoolObject(objectType, gameObject);

        }

        IEnumerator GrowObstacle(float length) {

            float timeElapsed = 0;

            while (timeElapsed < length) {
                SetObstacleSize(timeElapsed / length * 11);

                timeElapsed += Time.deltaTime;
                yield return null;
            }
            SetObstacleSize(11);
        }

        void SetObstacleSize(float size) {
            transform.localScale = new Vector3(size, size, size);
        }

        public void StartGrowRoutine() {

            StartCoroutine(GrowObstacle(4f));
        }
    }
}
