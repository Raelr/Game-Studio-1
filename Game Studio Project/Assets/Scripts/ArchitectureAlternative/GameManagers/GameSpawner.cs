using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlternativeArchitecture {

    public class GameSpawner : InitialisedEntity {

        [Header("Prefab References")]
        [SerializeField]
        PlayerMaster player = null;
        
        [Header("Script References")]
        [SerializeField]
        GamePooler pooler = null;

        public override void Initialise() {

            base.Initialise();

            SpawnPlayer();
        }

        public void SpawnPlayer() {
            GameObject newPlayer = pooler.RetrieveOrCreate(ObjectType.PLAYER);
            newPlayer.Show();
        }

        public void SpawnObject(ObjectType objectType)
        {
           GameObject spawnedObject = pooler.RetrieveOrCreate(objectType);
        }






        // placeholder script for spawning objects over time
        private float spawnCounter, spawnRate = 1;
        private void FixedUpdate()
        {
            if (spawnCounter > spawnRate)
            {
                spawnCounter = 0;
                SpawnObject(ObjectType.OBSTACLE_SPHERE);
            }
            spawnCounter += Time.deltaTime;
        }


    }
}

