using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AlternativeArchitecture;

namespace AlternativeArchitecture {

    public class GameMaster : Master {

        [Header("Spawner")]
        [SerializeField]
        AlternativeArchitecture.GameSpawner spawner;

        private void Awake() {

            SetUpReferences();

            InitialiseAll();
        }

        private void Start() {

            Initialise();
        }

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

