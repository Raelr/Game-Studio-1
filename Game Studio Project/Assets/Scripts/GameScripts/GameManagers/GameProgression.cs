﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlternativeArchitecture
{

    public enum ProgressionMode
    {
        SLOW,
        FAST
    }

    public class GameProgression : InitialisedEntity
    {

        [System.Serializable]
        public struct ObstacleChance {
            public List<int> chance;
            public List<int> choice;

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

        public AnimationCurve spawnInterval;

        public float gameSpeed = 1, gameSpeedMultiplier = 1;

        [SerializeField]
        public List<ObstacleChance> obstacleChances;

        public SkyboxChanger skyboxChanger;

        public ParticleSystem speedParticles;

        public GameProgression instance;
		
		private ProgressionMode currentMode;

        public override void Initialise()
        {
            base.Initialise();

            instance = this;

            progressionTimer = 0;
            progressUI.SetLevelProgress(0);
            SetGameLevelStartEffect(progressUI.SetLevel(currentLevel - 1));
            gameSpeed = Mathf.Clamp(currentLevel, 0, 5);
            
        }

        public void SetProgressionMode (bool isNormal)
        {
			currentMode = isNormal ? ProgressionMode.SLOW : ProgressionMode.FAST;
            switch (currentMode)
            {
                case ProgressionMode.SLOW:
                    levelInterval = 30;
                    Time.timeScale = 1;
                    PlayerPrefs.SetInt("normal", 1);
                    break;
                case ProgressionMode.FAST:
                    levelInterval = 10;
                    gameSpeedMultiplier = 1.1f;
                    Time.timeScale = 1.1f;
                    PlayerPrefs.SetInt("normal", 0);
                    break;
            }
        }
		
		public bool isTutorial () {
			return currentMode == ProgressionMode.SLOW;
		}

        public void SpawnObstaclesOnInterval() {
            progressionTimer += Time.deltaTime;

            progressUI.SetLevelProgress(progressionTimer / levelInterval);

            if (progressionTimer > levelInterval) {
                progressionTimer = 0;
                NextLevel();
                
            }

            spawnCounter += Time.deltaTime;

            if (spawnCounter > spawnInterval.Evaluate(currentLevel)) {
                spawnCounter = 0;
                SpawnObstacle(currentLevel);
            }
        }

        private void NextLevel () {

            currentLevel ++;

            gameSpeed = Mathf.Clamp(currentLevel, 0, 3); //level 5 is the max speed

            if (currentLevel > 6) //speed up after level 6
            {
                gameSpeed = currentLevel * 0.4f;
            }

            gameSpeed *= gameSpeedMultiplier;

            SetGameLevelEffect(progressUI.SetLevel(currentLevel - 1));
                
            var speedParticlesModule = speedParticles.main;
            speedParticlesModule.startSpeed = 7 * gameSpeed;
        }


        private void SetGameLevelEffect (int level) {

            skyboxChanger.SetSkybox(level);
        }

        private void SetGameLevelStartEffect (int level) {

            skyboxChanger.SetSkyboxStart(level);
        }

        private void SpawnObstacle (int level) {
            int objectToSpawn = ChooseObstacle(level);

            if (objectToSpawn == 0) return; //null

            GameObject newObstacle = spawner.SpawnObject(objectToSpawn);

            if (newObstacle.isNull()) return;
            newObstacle.transform.localScale = Vector3.zero;

            newObstacle.transform.localScale = Vector3.zero;

            Obstacle obstacleScript = newObstacle.GetComponent<Obstacle>();
            obstacleScript.Setup(GamePooler.instance,
                 objectToSpawn);
            obstacleScript.levelForceMultiplier = gameSpeed * 0.4f;

            float depthSpeed = Mathf.Clamp(gameSpeed, 0, 10);
            obstacleScript.minDepth = 1500;
            obstacleScript.maxDepth = 2000;
            obstacleScript.maxDepth *= 1 + (depthSpeed / 100);
            obstacleScript.minDepth *= 1 + (depthSpeed / 100); //pushes obstacle further away when going faster

            newObstacle.Show();

            if (objectToSpawn == 2 || objectToSpawn == 9 || objectToSpawn == 15) { //neon ring
                newObstacle.GetComponent<NeonRing>().StartRing();
            } 
            //else {
                obstacleScript.StartGrowRoutine(gameSpeed);
          //  }
        }

        private int ChooseObstacle (int level) {
            level = level - 1;

            //use chance based on the level
            float chance = Random.Range(0, 100);
            ObstacleChance chanceData;

            if (level > obstacleChances.Count - 1) //once you have completed all levels, select a random level
            {
                chanceData = obstacleChances[Random.Range(0, obstacleChances.Count)];
            }
            else
            {
                chanceData = obstacleChances[level];
            }


            foreach (int chanceValue in chanceData.chance) {
                int choiceValue = chanceData.choice[chanceData.chance.IndexOf(chanceValue)];
                if (chance < chanceValue) {
                    return choiceValue;
                }
            }
            
            return 0;

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
