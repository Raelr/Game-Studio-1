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

        public event PlayerLostHandler onPlayerLost;

        public delegate void EscapeKeyPressedHandler();

        public event EscapeKeyPressedHandler onEscapeKeyPressed;

        public float spaceHeldCount;

        [SerializeField]
        ParticleSystem particles;

        // Sets up all references and sets up the components.
        private void Awake() {

            gameStarted = false;

            if (instance == null)
            {
                instance = this;
            }

            if (!PlayerPrefs.HasKey("Reset"))
            {
                PlayerPrefs.SetInt("Reset", 0);
            }

            UIMaster.instance.StartLoadingScreenAsLoading();

            onEscapeKeyPressed += UIMaster.instance.ShowMainMenu;

            GameStarted = PlayerPrefs.GetInt("Reset") == 1;

            spaceHeldCount = 0;

            SetUpReferences();

            LoadScores();

            particles?.Stop();
        }

        // Initialises the actual object (only after all others have been set up)
        private void Start() {

            Initialise();

            if (gameStarted)
            {
                if (PlayerPrefs.HasKey("normalMode")) {
                    progression.SetProgressionMode(PlayerPrefs.GetInt("normalMode") == 1);
                }
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

                if (Input.GetKey(KeyCode.Escape))
                {
                    IncrementSpaceCounter();

                } else if (Input.GetKeyUp(KeyCode.Escape))
                {
                    spaceHeldCount = 0;
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape)) {
                if (!GameStarted) {
                    onEscapeKeyPressed?.Invoke();
                }
            }
        }

        public override void EscapeEvent()
        {
            base.EscapeEvent();
            onEscapeKeyPressed?.Invoke();
        }

        public void StartGame() {

            UIMaster.instance.HideMainMenu();

            ResumeGame();

            GameStarted = true;

            InitialiseAll();

            PlayerPrefs.SetInt("Reset", 0);

            particles?.Play();
        }

        // Initialises variables and sets delegates.
        public override void Initialise() {

            base.Initialise();

            onUpdateEvent += progression.SpawnObstaclesOnInterval;

            onPlayerLost += sounds.StopBackgroundMusic;
        }

        static float originalTimeScale = 1;

        public static void PauseGame() {
            originalTimeScale = Time.timeScale;
            Time.timeScale = 0f;
        }

        public static void ResumeGame() {
            Time.timeScale = originalTimeScale;
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
            onPlayerLost?.Invoke();
            PauseGame();
        }

        public void IncrementSpaceCounter()
        {
            spaceHeldCount += Time.deltaTime;

            if (spaceHeldCount >= 1)
            {
                UIMaster.instance.ResetGame();
            }
        }

        void LoadScores()
        {
            UIMaster.instance.LoadInScores();
        }
    }
}

