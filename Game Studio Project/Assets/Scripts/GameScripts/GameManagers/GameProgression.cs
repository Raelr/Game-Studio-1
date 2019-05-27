using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlternativeArchitecture
{
    public class GameProgression : InitialisedEntity
    {

        [Header("Script References")]
        [SerializeField]
        GameSpawner spawner = null;

        [Header("Controls")]
        [SerializeField]
        AnimationCurve spawnRateMultiplier; //time -> value

        [SerializeField]
        float spawnRate; // seconds

        private float spawnCounter;
        private float spawnResetCounter;

        public override void Initialise()
        {
            base.Initialise();
            // StartCoroutine(SpawnNeonRing(Random.Range(10,20)));
            StartCoroutine(SpawnNeonRing(3));
        }

        private IEnumerator SpawnNeonRing(float startDelay)
        {
            yield return new WaitForSeconds(startDelay);
            SpawnRing();
            StartCoroutine(SpawnNeonRing(Random.Range(4, 10)));
        }

        public void SpawnObstaclesOnInterval() {

            if (spawnResetCounter > spawnRate * spawnRateMultiplier.Evaluate(spawnCounter)) {
                spawnResetCounter = 0;
                SpawnObstacle();
            }
            spawnResetCounter += Time.deltaTime;
            spawnCounter += Time.deltaTime;
        }

        private void SpawnObstacle ()
        {
            GameObject newObstacle = spawner.SpawnObject(ObjectType.OBSTACLE_SPHERE);

            newObstacle.transform.localScale = Vector3.zero;

            if (newObstacle.isNull()) return;

            Obstacle obstacleScript = newObstacle.GetComponent<Obstacle>();
            obstacleScript.Setup(GamePooler.instance, //replace the game pooler with actual reference
                 ObjectType.OBSTACLE_SPHERE);
            newObstacle.Show();
            obstacleScript.StartGrowRoutine();
        }

        private void SpawnRing ()
        {
            GameObject newRing = spawner.SpawnObject(ObjectType.NEON_RING);
            if (newRing.isNull()) return;

            Obstacle obstacleScript = newRing.GetComponent<Obstacle>();
            obstacleScript.Setup(GamePooler.instance, //replace the game pooler with actual reference
                 ObjectType.NEON_RING);
            newRing.Show();
        }

    }
}