using System.Collections;
using System.Collections.Generic;
using AlternativeArchitecture;
using UnityEngine;
using UnityEngine.UI;

public class UIMaster : Master
{
    public static UIMaster instance;

    bool gameStarted;

    public bool GameStarted { get { return gameStarted; } set { gameStarted = value; OnGameLevelStarted(value); } }

    Color levelColor;

    public Color LevelColor { set { levelColor = value; colorChanged?.Invoke(levelColor); } }

    [Header("Menu Manager")]
    [SerializeField]
    MenuManager menuManager = null;

    [Header("Insanity Meter")]
    [SerializeField]
    UIMeter insanityMeter = null;

    [Header("Camera Effects")]
    [SerializeField]
    CameraEffects camEffects = null;

    [Header("Text Controller")]
    [SerializeField]
    UITextController textController = null;

    [Header("Score")]
    [SerializeField]
    UITextController normalScore = null;

    [SerializeField]
    UITextController rushScore = null;

    [SerializeField]
    UITextController finalScore = null;

    ProgressionMode currentProgression;

    public delegate void UpdateEventHandler();

    public UpdateEventHandler onUpdateEvent;

    public delegate void UIEventChangeHandler(string description, int value);

    public UIEventChangeHandler onUIChange;

    public delegate void UpdateSpeedHandler(float amount);

    public UpdateSpeedHandler onSpeedUpdate;

    public delegate void UIChangeHandler(bool value);

    public UIChangeHandler onUIStatusChange;

    public delegate void PlayerLostHandler(bool enabled = false);

    public PlayerLostHandler onPlayerLost;

    public delegate void MeterChangeHandler(float incrementSpeed, bool reverse = false);

    public MeterChangeHandler onMeterChange;

    public delegate void UIColorChangeHandler(Color color);

    public event UIColorChangeHandler colorChanged;

    public delegate void UIResetHandler();

    public event UIResetHandler onReset;

    private void Awake() {

        SetUpReferences();

        instance = this;

        onUIStatusChange += insanityMeter.ChangeMeterStatus;

        onMeterChange += insanityMeter.IncrementMeter;

        colorChanged += insanityMeter.ChangeMeterColor;

        colorChanged += textController.ChangeTextColor;

        onUIStatusChange += textController.ChangeTextStatus;

        onReset += menuManager.RestartAfterFade;

        menuManager.onReset += SaveScore;

        InitialiseAll();
    }

    private void Start() {

        Initialise();
    }

    private void Update() {
        
        if (gameStarted) {

            onUpdateEvent?.Invoke();
            onSpeedUpdate?.Invoke(0.1f);
        }
    }

    public override void Initialise() {

        base.Initialise();

        GameStarted = false;

        onPlayerLost += menuManager.LoadLoseScreen;
    }

    public override void InitialiseAll() {

        base.InitialiseAll();

        menuManager.Initialise();

        insanityMeter.Initialise();

        camEffects.Initialise();
    }

    public void OnGameLevelStarted(bool value) {

        gameStarted = value;

        onUIStatusChange?.Invoke(value);
    }

    public void ShowMainMenu()
    {
        menuManager.ShowMenu();
    }

    public void HideMainMenu()
    {
        menuManager.HideMainMenu();
    }

    public void OnPlayerLost() {
        
        GameStarted = false;

        LoadFinalScore();
    }

    public void LoadFinalScore() {

        finalScore.UpdateText(textController.GetTextValue());

        int finalScoreValue = currentProgression == ProgressionMode.SLOW ? int.Parse(PlayerPrefs.GetString("normal")) : int.Parse(PlayerPrefs.GetString("rush"));

        bool enabled = finalScoreValue < int.Parse(textController.GetTextValue()) ? true : false;

        onPlayerLost?.Invoke(enabled);
    }

    public override void SetUpReferences() {

        base.SetUpReferences();

        menuManager = GetComponent<MenuManager>();

        insanityMeter = GetComponent<UIMeter>();

        textController = GetComponent<UITextController>();
    }

    public void StartLoadingScreenAsLoading()
    {
        menuManager.StartLoadingAsBlack();
        menuManager.ShowLoadingScreen();
        menuManager.ResetFadeIn();
    }

    public void UpdatePoints(float value) {
        textController.GainPoints(value);
    }

    public void ResetGame()
    {
        onReset?.Invoke();
    }

    
    public void SetCurrentProgression(ProgressionMode mode)
    {
        currentProgression = mode;
        int score = mode == ProgressionMode.SLOW ? int.Parse(PlayerPrefs.GetString("normal")) : int.Parse(PlayerPrefs.GetString("rush"));
        HighScoreUI.instance.SetHighScore(score);
    }

    public void SaveScore()
    {
        if (currentProgression == ProgressionMode.SLOW)
        {
            if (int.Parse(PlayerPrefs.GetString("normal")) < int.Parse(textController.GetTextValue()))
                PlayerPrefs.SetString("normal", textController.GetTextValue());
        }

        if (currentProgression == ProgressionMode.FAST)
        {
            if (int.Parse(PlayerPrefs.GetString("rush")) < int.Parse(textController.GetTextValue()))
                PlayerPrefs.SetString("rush", textController.GetTextValue());
        }
    }

    public void LoadInScores()
    {
        if (!PlayerPrefs.HasKey("normal"))
        {
            PlayerPrefs.SetString("normal", "5000");
        }
        if (!PlayerPrefs.HasKey("rush"))
        {
            PlayerPrefs.SetString("rush", "5000");
        }

        normalScore.UpdateText(PlayerPrefs.GetString("normal"));
        rushScore.UpdateText(PlayerPrefs.GetString("rush"));
    }
}
