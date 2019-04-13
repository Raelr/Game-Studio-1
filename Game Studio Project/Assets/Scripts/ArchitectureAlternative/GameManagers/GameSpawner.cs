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
        }

        public void SpawnObject(ObjectType objectType)
        {
           GameObject spawnedObject = pooler.RetrieveOrCreate(objectType);
        }
    }
}

