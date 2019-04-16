using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMaster : Master
{

    public static UIMaster instance;

    bool gameStarted;

    [SerializeField]
    UITime time;

    [SerializeField]
    UITextController lives;

    [SerializeField]
    UISpeed speed;

    public delegate void UpdateEventHandler();

    public UpdateEventHandler onUpdateEvent;

    public delegate void UIEventChangeHandler(string description, int value);

    public UIEventChangeHandler onUIChange;

    public delegate void UpdateSpeedHandler(float amount);

    public UpdateSpeedHandler onSpeedUpdate;

    public delegate void UIChangeHandler(bool value);

    public UIChangeHandler onUIStatusChange;

    public delegate void GameStartHandler();

    public GameStartHandler onGameStart;

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
            onSpeedUpdate.Invoke(0.1f);
        }
    }

    public override void Initialise() {

        base.Initialise();

        gameStarted = false;
    }

    public override void InitialiseAll() {

        base.InitialiseAll();

        time.Initialise();

        lives.Initialise();

        speed.Initialise();
    }

    public void onGameLevelStarted() {

        gameStarted = true;

        onUIStatusChange?.Invoke(gameStarted);
    }

    public override void SetUpReferences() {

        base.SetUpReferences();

        time = GetComponent<UITime>();

        lives = GetComponent<UITextController>();

        speed = GetComponent<UISpeed>();
    }
}
