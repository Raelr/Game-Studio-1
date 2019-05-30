using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlternativeArchitecture
{
    public class GameProgression : InitialisedEntity
    {

        [System.Serializable]
        public struct ObstacleChance {
            public List<int> chance;
            public List<ObjectType> choice;

        }
        
        [Header("Script References")]
        [SerializeField]
        GameSpawner spawner = null;

        private float progressionTimer;

        public float levelInterval = 30;
        
        [SerializeField]
        private int currentLevel = 1;

        public ProgressUI progressUI;

        private float spawnCounter = 0;
        public float spawnInterval = 0.1f;

        [SerializeField]
        public List<ObstacleChance> obstacleChances;


        public override void Initialise()
        {
            base.Initialise();
            progressionTimer = 0;
            progressUI.SetLevelProgress(0);
            SetGameColor(progressUI.SetLevel(currentLevel - 1));
        }

        public void SpawnObstaclesOnInterval() {
            progressionTimer += Time.deltaTime;

            progressUI.SetLevelProgress(progressionTimer / levelInterval);

            if (progressionTimer > levelInterval) {
                progressionTimer = 0;
                currentLevel ++;
                SetGameColor(progressUI.SetLevel(currentLevel - 1));
            }

            spawnCounter += Time.deltaTime;

            if (spawnCounter > spawnInterval) {
                spawnCounter = 0;
                SpawnObstacle(currentLevel);
            }
        }

        private void SetGameColor (Color newCol) {


            Debug.Log("set world color to " + newCol);

        }

        private void SpawnObstacle (int level) {
            ObjectType objectToSpawn = ChooseObstacle(level);

            if (objectToSpawn == ObjectType.NULL) return;

            GameObject newObstacle = spawner.SpawnObject(objectToSpawn);

            if (newObstacle.isNull()) return;

            newObstacle.transform.localScale = Vector3.zero;

            Obstacle obstacleScript = newObstacle.GetComponent<Obstacle>();
            obstacleScript.Setup(GamePooler.instance,
                 objectToSpawn);
            obstacleScript.levelForceMultiplier = level * 0.2f;
            newObstacle.Show();


            if (objectToSpawn == ObjectType.NEON_RING) {
                newObstacle.GetComponent<NeonRing>().StartRing();
            } 
            //else {
                obstacleScript.StartGrowRoutine();
          //  }
        }

        private ObjectType ChooseObstacle (int level) {
level = level - 1;

            //use chance based on the level
            float chance = Random.Range(0, 100);

            Debug.Log("A" + Time.time);
            ObstacleChance chanceData = obstacleChances[Mathf.Clamp(level, 0, obstacleChances.Count)];

            Debug.Log("B" + Time.time);

            foreach (int chanceValue in chanceData.chance) {
                ObjectType choiceValue = chanceData.choice[chanceData.chance.IndexOf(chanceValue)];
                if (chance < chanceValue) {
                    return choiceValue;
                }
            }
            
            return ObjectType.NULL;

        }

/* 
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


        public ProgressUI progressUI;

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
                SpawnBoost();
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

        private void SpawnBoost() {
            GameObject newObstacle = spawner.SpawnObject(ObjectType.OBSTACLE_BOOST);

           // newObstacle.transform.localScale = Vector3.zero;

            if (newObstacle.isNull()) return;

            Obstacle obstacleScript = newObstacle.GetComponent<Obstacle>();
            obstacleScript.Setup(GamePooler.instance, //replace the game pooler with actual reference
                 ObjectType.OBSTACLE_BOOST);
            newObstacle.Show();
            obstacleScript.StartGrowRoutine();
        }
*/
    }
}
