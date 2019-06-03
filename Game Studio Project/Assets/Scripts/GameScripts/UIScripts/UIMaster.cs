using System.Collections;
using System.Collections.Generic;
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

    public delegate void UpdateEventHandler();

    public UpdateEventHandler onUpdateEvent;

    public delegate void UIEventChangeHandler(string description, int value);

    public UIEventChangeHandler onUIChange;

    public delegate void UpdateSpeedHandler(float amount);

    public UpdateSpeedHandler onSpeedUpdate;

    public delegate void UIChangeHandler(bool value);

    public UIChangeHandler onUIStatusChange;

    public delegate void PlayerLostHandler();

    public PlayerLostHandler onPlayerLost;

    public delegate void MeterChangeHandler(float incrementSpeed, bool reverse = false);

    public MeterChangeHandler onMeterChange;

    public delegate void UIColorChangeHandler(Color color);

    public event UIColorChangeHandler colorChanged;

    private void Awake() {

        SetUpReferences();

        instance = this;

        onUIStatusChange += insanityMeter.ChangeMeterStatus;

        onMeterChange += insanityMeter.IncrementMeter;

        colorChanged += insanityMeter.ChangeMeterColor;

        colorChanged += textController.ChangeTextColor;

        onUIStatusChange += textController.ChangeTextStatus;

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
        
        onPlayerLost?.Invoke();
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

}
