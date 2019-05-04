using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AlternativeArchitecture;

namespace AlternativeArchitecture {

    public class GameMaster : Master {

        public static GameMaster instance;
       
        private AlternativeArchitecture.GameSpawner spawner;
        private AlternativeArchitecture.GameProgression progression;
        private AlternativeArchitecture.GamePooler pooler;

        [SerializeField]
        GameSounds sounds;

        bool gameStarted;

        public bool GameStarted { get { return gameStarted; } set { gameStarted = value; UIMaster.instance.OnGameLevelStarted(gameStarted);} }

        public delegate void UpdateEventHandler();

        public UpdateEventHandler onUpdateEvent;

        public delegate void PlayerLostHandler();

        public event PlayerLostHandler OnPlayerLost;

        // Sets up all references and sets up the components.
        private void Awake() {

            if (instance == null)
            {
                instance = this;
            }

            ResumeGame();

            gameStarted = false;

            SetUpReferences();
            
        }

        // Initialises the actual object (only after all others have been set up)
        private void Start() {

            Initialise();
        }

        private void FixedUpdate() {
            
            if (gameStarted) {
                onUpdateEvent?.Invoke();
            }
        }

        public void StartGame() {

            GameStarted = true;

            InitialiseAll();
        }

        // Initialises variables and sets delegates.
        public override void Initialise() {

            base.Initialise();

            onUpdateEvent += progression.SpawnObstaclesOnInterval;

            OnPlayerLost += sounds.StopBackgroundMusic;
        }

        public static void PauseGame() {

            Time.timeScale = 0f;

        }

        public static void ResumeGame() {

            Time.timeScale = 1f;
        }

        public override void InitialiseAll() {

            base.InitialiseAll();
            spawner.Initialise();
            progression.Initialise();
            pooler.Initialise();
            sounds.Initialise();
        }

        public override void SetUpReferences() {

            base.SetUpReferences();

            spawner = GetComponent<GameSpawner>();
            progression = GetComponent<GameProgression>();
            pooler = GetComponent<GamePooler>();
            sounds = GetComponent<GameSounds>();
        }

        public void OnPlayerLose()
        {
            OnPlayerLost?.Invoke();
            PauseGame();
        }
    }
}

