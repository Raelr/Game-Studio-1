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

        [Header("Properties")]
        [SerializeField]
        PlayerProperties playerProperties;

        // Delegate for handling mouse click input. 
        // You'd need other delgates for other forms of input for the master (moving...etc)
        public delegate void OnClickHandler();

        public event OnClickHandler onClick;

        public delegate void UpdateEventHandler(Vector2 velocity);

        public event UpdateEventHandler updateEvent;

        public delegate void CollisionEventHandler();

        public CollisionEventHandler onPlayerCollision;

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

            updateEvent += movementController.MoveEntity;
        }

        // Initialises all components underneath master.
        public override void InitialiseAll() {

            base.InitialiseAll();

            particles.Initialise();

            movementController.Initialise();

            projectiles.Initialise();

            playerProperties.Initialise();

            movementController.onCollision += OnPlayerHit;

            onPlayerCollision += playerProperties.DecrementLives;
        }

        // Gets the approrpiate components for master. 
        public override void SetUpReferences() {

            base.SetUpReferences();

            particles = GetComponent<ParticleController>();

            movementController = GetComponent<MovementController>();

            projectiles = GetComponent<ProjectileController>();

            playerProperties = GetComponent<PlayerProperties>();
        }

        // Processes all user (or script based) input.
        public override void ClickEvent() {

            onClick?.Invoke();
        }

        public override void MoveToward(Vector2 mouseCoordinates) {

            updateEvent?.Invoke(mouseCoordinates);
        }

        public void OnPlayerHit() {

            onPlayerCollision?.Invoke();
        }
    }
}

