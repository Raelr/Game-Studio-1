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
            if (newObstacle.isNull()) return;

            Obstacle obstacleScript = newObstacle.GetComponent<Obstacle>();
            obstacleScript.Setup(GamePooler.instance, //replace the game pooler with actual reference
                 ObjectType.OBSTACLE_SPHERE);
            newObstacle.Show();
        }

    }
}