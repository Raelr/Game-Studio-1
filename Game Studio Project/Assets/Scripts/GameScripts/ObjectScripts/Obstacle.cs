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
        private Vector3 dir;
        [SerializeField]
        private bool randomRotate = true;

        public float forceMultiplier = 1;
        public float levelForceMultiplier = 1;

        [SerializeField]
        private Rigidbody rigid = null;

        private GamePooler gamePooler;
        private int objectType;

        [SerializeField]
        private float zDespawn = 0;

        private Vector3 Dir { get { return (origin1.position - player.position).normalized; } }
        public float Force { get { return force; } set { force = value; } }

        Renderer ren;

        float minWidth;
        float maxWidth;
        float minHeight;
        float maxHeight;

        public bool useRandomSize = true;
        public float randomMinSize = 10, randomMaxSize = 20;
        float maxSize;

        public float minDepth = 1500f, maxDepth = 2000;
        

        private void Start() {
            minWidth = -Screen.width;
            maxWidth = Screen.width;
            minHeight = -Screen.height;
            maxHeight = Screen.height;
        }

        public void Setup(GamePooler pooler, int type)
        {
            if (ren == null)
            {
                ren = GetComponentInChildren<Renderer>();
            }
            rigid.velocity = Vector3.zero;
            

            origin1 = GameObject.FindGameObjectWithTag("Player").transform;
            player = origin1.GetChild(0).transform;
            if (useRandomSize) maxSize = Random.Range(randomMinSize, randomMaxSize);
            if (randomRotate) transform.Rotate(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));

            RandomSpawn();

            gamePooler = pooler;
            objectType = type;

            isActive = true;
        }

        private void RandomSpawn() {
            float randomWidth = Random.Range(-500, 500);
            float randomHeight = Random.Range(-500, 500);
            float randomDepth = Random.Range(minDepth, maxDepth);

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
                if (Vector3.Distance(Vector3.zero, transform.position) > 50f && transform.position.z<zDespawn) {
                    BackToPool();
                }
            }

            // rigid.AddForce(Dir * force * forceMultiplier);

            Vector3 velocity = (Dir * force * forceMultiplier * levelForceMultiplier);

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

            SetObstacleSize(0.01f);

            yield return new WaitForSeconds(0.1f);

            float timeElapsed = 0;
            

            while (timeElapsed < length)
            {
                SetObstacleSize((maxSize * (timeElapsed / length))+0.01f);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            SetObstacleSize(maxSize);
            
        }

        void SetObstacleSize(float size) {
            transform.localScale = new Vector3(size, size, size);
        }

        public void StartGrowRoutine(float gameSpeed) {

            StartCoroutine(GrowObstacle(2f / gameSpeed));
        }
    }
}
