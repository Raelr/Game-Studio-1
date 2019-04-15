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

            PlayerMaster newPlayer = SpawnPlayer();
            newPlayer.gameObject.Show();

            // perhaps do something with the new playermaster reference (e.g. store it into data somewhere)
        }

        public PlayerMaster SpawnPlayer() {
            return pooler.RetrieveOrCreate(ObjectType.PLAYER).GetComponent<PlayerMaster>();
        }

        public GameObject SpawnObject(ObjectType objectType)
        {
           return pooler.RetrieveOrCreate(objectType);
        }

    }
}

