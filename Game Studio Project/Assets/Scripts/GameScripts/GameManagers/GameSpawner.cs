using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlternativeArchitecture {

    public class GameSpawner : InitialisedEntity {

        [Header("Prefab References")]
        [SerializeField]
        PlayerMaster player;
        
        [Header("Script References")]
        [SerializeField]
        GamePooler pooler = null;

        [SerializeField]
        CameraLookat camLookat = null;

        public override void Initialise() {

            base.Initialise();

            if (player == null)
            {
                player = SpawnPlayer();
                player.gameObject.Show();
            }
            // perhaps do something with the new playermaster reference (e.g. store it into data somewhere)

            //place holder
            camLookat.SetTarget(player.transform.GetChild(0));
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

