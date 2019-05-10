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

        [Header("Sounds")]
        [SerializeField]
        PlayerSounds sounds;

        // Delegate for handling mouse click input. 
        // You'd need other delgates for other forms of input for the master (moving...etc)
        public delegate void OnClickHandler();

        public event OnClickHandler onClick;

        public delegate void UpdateEventHandler(Vector2 velocity);

        public event UpdateEventHandler updateEvent;

        public delegate void CollisionEventHandler();

        public CollisionEventHandler onPlayerCollision;

        public delegate void UIMeterChangeHandler();

        public UIMeterChangeHandler onMeterChanged;

        public delegate void SoundChangeHandler(float volume);

        public SoundChangeHandler onSoundChanged;

        public delegate void PlayerLostHandler();

        public PlayerLostHandler onPlayerLost;

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

            updateEvent += movementController.RotateEntity;

            onPlayerCollision += playerProperties.DecaySanityByAmount;
        }

        // Initialises all components underneath master.
        public override void InitialiseAll() {

            base.InitialiseAll();

            particles.Initialise();

            movementController.Initialise();

            projectiles.Initialise();

            playerProperties.Initialise();

            sounds.Initialise();

            movementController.onCollision += OnPlayerHit;

            playerProperties.onPlayerLose += OnPlayerLose;

            onMeterChanged += playerProperties.DecaySanityConstant;

            playerProperties.OnSoundChanged += sounds.AdjustAudioSourceVolume;

            onPlayerLost += sounds.StopBackgroundSound;
        }

        // Gets the approrpiate components for master. 
        public override void SetUpReferences() {

            base.SetUpReferences();

            particles = GetComponent<ParticleController>();

            movementController = GetComponent<MovementController>();

            projectiles = GetComponent<ProjectileController>();

            playerProperties = GetComponent<PlayerProperties>();

            sounds = GetComponentInChildren<PlayerSounds>();
        }

        // Processes all user (or script based input.
        public override void ClickEvent() {

            onClick?.Invoke();
        }

        public override void OnUIChange() {

            onMeterChanged?.Invoke();
        }

        public override void MoveToward(Vector2 mouseCoordinates) {

            updateEvent?.Invoke(mouseCoordinates);
        }

		public override void RotateEntity(Vector2 input) {
			updateEvent?.Invoke(input);
		}

		public void OnPlayerHit() {
        
            onPlayerCollision?.Invoke();
            
        }

        public void OnPlayerLose() {

            onPlayerLost?.Invoke();

            GameMaster.instance.OnPlayerLose();

            UIMaster.instance.OnPlayerLost();
        }
    }
}

