using System.Collections;
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

    [Header("Lives Text")]
    [SerializeField]
    UITextController lives;

    [Header("Speed Text")]
    [SerializeField]
    UISpeed speed;

    [Header("Menu Manager")]
    [SerializeField]
    MenuManager menuManager;

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

    private void Awake() {

        SetUpReferences();

        instance = this;

        onUpdateEvent += time.IncrementTime;

        onUIChange += lives.UpdateText;

        onSpeedUpdate += speed.IncrementSpeed;

        onUIStatusChange += time.ChangeTextStatus;

        onUIStatusChange += lives.ChangeTextStatus;

        onUIStatusChange += speed.ChangeTextStatus;

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

        lives.Initialise();

        speed.Initialise();

        menuManager.Initialise();
    }

    public void OnGameLevelStarted(bool value) {

        gameStarted = value;

        onUIStatusChange?.Invoke(value);
    }

    public void OnPlayerLost() {

        Debug.Log("Player Lost");

        GameStarted = false;
        
        onPlayerLost?.Invoke();
    }

    public override void SetUpReferences() {

        base.SetUpReferences();

        time = GetComponent<UITime>();

        lives = GetComponent<UITextController>();

        speed = GetComponent<UISpeed>();

        menuManager = GetComponent<MenuManager>();
    }
}
