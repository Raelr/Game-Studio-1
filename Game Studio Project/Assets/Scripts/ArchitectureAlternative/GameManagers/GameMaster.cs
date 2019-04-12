using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AlternativeArchitecture;

namespace AlternativeArchitecture {

    public class GameMaster : Master {

        [Header("Spawner")]
        [SerializeField]
        AlternativeArchitecture.GameSpawner spawner;

        // Sets up all references and sets up the components.
        private void Awake() {

            SetUpReferences();

            InitialiseAll();
        }

        // Initialises the actual object (only after all others have been set up)
        private void Start() {

            Initialise();
        }

        // Initialises variables and sets delegates.
        public override void Initialise() {

            base.Initialise();
        }

        public override void InitialiseAll() {

            base.InitialiseAll();

            spawner.Initialise();
        }

        public override void SetUpReferences() {

            base.SetUpReferences();

            spawner = GetComponent<GameSpawner>();
        }
    }
}

