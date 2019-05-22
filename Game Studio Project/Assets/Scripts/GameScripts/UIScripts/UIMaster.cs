﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMaster : Master
{

    public static UIMaster instance;

    bool gameStarted;

    public bool GameStarted { get { return gameStarted; } set { gameStarted = value; OnGameLevelStarted(value); } }

    [Header("Time text")]
    [SerializeField]
    UITime time;

    [Header("Speed Text")]
    [SerializeField]
    UISpeed speed;

    [Header("Menu Manager")]
    [SerializeField]
    MenuManager menuManager;

    [Header("Insanity Meter")]
    [SerializeField]
    UIMeter insanityMeter;

    [Header("Camera Effects")]
    [SerializeField]
    CameraEffects camEffects;

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

    private void Awake() {

        SetUpReferences();

        instance = this;

        onUpdateEvent += time.IncrementTime;

        onSpeedUpdate += speed.IncrementSpeed;

        onUIStatusChange += time.ChangeTextStatus;

        onUIStatusChange += speed.ChangeTextStatus;

        onUIStatusChange += insanityMeter.ChangeMeterStatus;

        onMeterChange += insanityMeter.IncrementMeter;

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

        time.Initialise();
        
        speed.Initialise();

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
        menuManager.ShowMainMenu();
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

        time = GetComponent<UITime>();

        speed = GetComponent<UISpeed>();

        menuManager = GetComponent<MenuManager>();

        insanityMeter = GetComponent<UIMeter>();
    }

    public void StartLoadingScreenAsLoading()
    {
        menuManager.StartLoadingAsBlack();
        menuManager.ShowLoadingScreen();
        menuManager.ResetFadeIn();
    }
}
