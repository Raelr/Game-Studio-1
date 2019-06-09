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

        [Header("Properties")]
        [SerializeField]
        PlayerProperties playerProperties;

        [Header("Sounds")]
        [SerializeField]
        PlayerSounds sounds;

        [Header("Temp")]
        [SerializeField]
        GameObject impactParticlePrefab = null;

        [Header("Temp2")]
        [SerializeField]
        public Transform impactTransform;

        [Header("Temp3")]
        [SerializeField]
        public Renderer shipRender;

        bool playerLost = false;

        private ParticleSystem impactParticle;


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


        public static PlayerMaster instance;


        private void Awake() {

            instance = this;

            SetUpReferences();

            InitialiseAll();
        }

        private void Start() {

            Initialise();
        }

        // Initialises all variables and assigns appropriate delegates.
        public override void Initialise() {

            base.Initialise();

            playerLost = false;

            updateEvent += movementController.RotateEntity;

            updateEvent += movementController.MultiplyPoints;

            onPlayerCollision += playerProperties.DecaySanityByAmount;

            onPlayerCollision += sounds.PlayerImpactSound;
        }

        // Initialises all components underneath master.
        public override void InitialiseAll() {

            base.InitialiseAll();

            particles.Initialise();

            movementController.Initialise();

            playerProperties.Initialise();

            sounds.Initialise();

            movementController.onCollision += OnPlayerHit;

			movementController.onNearMiss += OnPlayerNearMiss;

            playerProperties.onPlayerLose += OnPlayerLose;

            onMeterChanged += playerProperties.DecaySanityConstant;

            playerProperties.OnSoundChanged += sounds.AdjustAudioSourceVolume;

            onPlayerLost += sounds.StopBackgroundSound;

            movementController.onTimeChange += SetDecaySpeed;
        }

        // Gets the approrpiate components for master.
        public override void SetUpReferences() {

            base.SetUpReferences();

            particles = GetComponent<ParticleController>();

            movementController = GetComponent<MovementController>();

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

            CameraShake.instance.ShakeOnce();
            onPlayerCollision?.Invoke();

            //temp
            if (impactParticle == null)
            {
                GameObject particlePrefab = Instantiate(impactParticlePrefab, impactTransform);
                impactParticle = particlePrefab.GetComponent<ParticleSystem>();
            }
            impactParticle.Emit(30);
            StartCoroutine(TempShipFlash());
        }

		public void OnPlayerNearMiss() {
        
            playerProperties.ImproveSanity();
        }

		IEnumerator TempShipFlash ()
        {
            shipRender.material.SetFloat("_RimPower", 0.5f);
            shipRender.material.SetColor("_RimColor", Color.red);
            yield return new WaitForSeconds(0.2f);
            shipRender.material.SetFloat("_RimPower", 5.9f);
            shipRender.material.SetColor("_RimColor", Color.white);
        }

        public void OnPlayerLose() {

            if (!playerLost)
            {
                playerLost = true;

                onPlayerLost?.Invoke();

                GameMaster.instance.OnPlayerLose();

                CameraShake.instance.StopCameraShake();

                UIMaster.instance.OnPlayerLost();
            }
        }

        public void SetDecaySpeed(float speed)
        {
            playerProperties.InsanityDecaySpeed = speed;
        }
    }
}
