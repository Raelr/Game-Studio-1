using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AlternativeArchitecture;

namespace AlternativeArchitecture {

    public class GameMaster : Master {
       
        private AlternativeArchitecture.GameSpawner spawner;
        private AlternativeArchitecture.GameProgression progression;
        private AlternativeArchitecture.GamePooler pooler;

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
            progression.Initialise();
            pooler.Initialise();
        }

        public override void SetUpReferences() {

            base.SetUpReferences();

            spawner = GetComponent<GameSpawner>();
            progression = GetComponent<GameProgression>();
            pooler = GetComponent<GamePooler>();
        }
    }
}

