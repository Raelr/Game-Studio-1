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

        public GameSpawner Spawner { get { return spawner; } }

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

            if (!PlayerPrefs.HasKey("Reset"))
            {
                PlayerPrefs.SetInt("Reset", 0);
            }

            UIMaster.instance.StartLoadingScreenAsLoading();

            GameStarted = PlayerPrefs.GetInt("Reset") == 1;

            SetUpReferences();
        }

        // Initialises the actual object (only after all others have been set up)
        private void Start() {

            Initialise();

            if (gameStarted)
            {
                StartGame();
            }
            else
            {
                UIMaster.instance.ShowMainMenu();
            }
        }

        private void FixedUpdate() {
            
            if (gameStarted) {
                onUpdateEvent?.Invoke();
            }
        }

        public void StartGame() {

            UIMaster.instance.HideMainMenu();

            ResumeGame();

            GameStarted = true;

            InitialiseAll();

            PlayerPrefs.SetInt("Reset", 0);
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
            GameStarted = false;
            OnPlayerLost?.Invoke();
            PauseGame();
        }
    }
}

