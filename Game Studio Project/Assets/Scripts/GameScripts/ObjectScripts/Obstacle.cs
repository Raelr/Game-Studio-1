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

        float minWidth;
        float maxWidth;
        float minHeight;
        float maxHeight;

        float maxSize;

        private void Start() {
            minWidth = -Screen.width;
            maxWidth = Screen.width;
            minHeight = -Screen.height;
            maxHeight = Screen.height;
        }

        public void Setup(GamePooler pooler, ObjectType type)
        {
            if (ren == null)
            {
                ren = GetComponentInChildren<Renderer>();
            }
            rigid.velocity = Vector3.zero;
            origin1 = GameObject.FindGameObjectWithTag("Player").transform;
            player = origin1.GetChild(0).transform;

            maxSize = Random.Range(10,20);
            if (randomRotate) transform.Rotate(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));

            RandomSpawn();

            gamePooler = pooler;
            objectType = type;

            isActive = true;
        }

        private void RandomSpawn() {
            float randomWidth = Random.Range(-500, 500);
            float randomHeight = Random.Range(-500, 500);
            float randomDepth = Random.Range(2000 * 0.75f, 2000);

            Vector3 dir = GetDir(transform.position);

            transform.position = new Vector3(randomWidth, randomHeight , randomDepth);
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
            float scale = (maxSize / 10);
            length *= scale;

            while (timeElapsed < length) {
                SetObstacleSize(timeElapsed / length * maxSize);

                timeElapsed += Time.deltaTime;
                yield return null;
            }

            SetObstacleSize(maxSize);
        }

        void SetObstacleSize(float size) {
            transform.localScale = new Vector3(size, size, size);
        }

        public void StartGrowRoutine() {

            StartCoroutine(GrowObstacle(4f));
        }
    }
}
