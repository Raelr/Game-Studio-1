using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlternativeArchitecture {

    public class GameSpawner : InitialisedEntity {

        [Header("Player prefab")]
        [SerializeField]
        PlayerMaster player = null;

        public override void Initialise() {

            base.Initialise();

            SpawnPlayer();
        }

        public void SpawnPlayer() {

            Instantiate(player);

        }
    }
}

