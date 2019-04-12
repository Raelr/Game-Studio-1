using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlternativeArchitecture {

    // Interfaces with an input script to make the entity act in a certain manner.
    public class PlayerMaster : Master {

        [Header("Movement")]
        [SerializeField]
        MovementController movementController;

        // An example component which the master can also use.
        [Header("Particles")]
        [SerializeField]
        ParticleController particles;

        [Header("Projectiles")]
        [SerializeField]
        ProjectileController projectiles;

        // Delegate for handling mouse click input. 
        // You'd need other delgates for other forms of input for the master (moving...etc)
        public delegate void OnClickHandler();

        public event OnClickHandler onClick;

        private void Awake() {

            SetUpReferences();

            InitialiseAll();
        }

        private void Start() {

            Initialise();
        }

        // Initialises all variables and assigns appropriate delegates. 
        public override void Initialise() {

            base.Initialise();

            onClick += projectiles.FireProjectile;
        }

        // Initialises all components underneath master.
        public override void InitialiseAll() {

            base.InitialiseAll();

            particles.Initialise();

            movementController.Initialise();

            projectiles.Initialise();
        }

        // Gets the approrpiate components for master. 
        public override void SetUpReferences() {

            base.SetUpReferences();

            particles = GetComponent<ParticleController>();

            movementController = GetComponent<MovementController>();

            projectiles = GetComponent<ProjectileController>();
        }

        // Processes all user (or script based) input.
        public override void ProcessInput() {

            onClick?.Invoke();
        }
    }
}

