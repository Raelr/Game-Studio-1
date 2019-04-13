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

        public override void Initialise()
        {

            base.Initialise();
        }

        // placeholder script for spawning objects over time
        private float spawnCounter, spawnRate = 1;
        private void FixedUpdate()
        {
            if (spawnCounter > spawnRate)
            {
                spawnCounter = 0;
                GameObject newObject = spawner.SpawnObject(ObjectType.OBSTACLE_SPHERE);
                if (newObject.isNotNull())
                    newObject.Show();
            }
            spawnCounter += Time.deltaTime;
        }


    }
}