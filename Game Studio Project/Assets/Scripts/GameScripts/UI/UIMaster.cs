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
    UILives lives;

    public delegate void UpdateEventHandler();

    public UpdateEventHandler onUpdateEvent;

    public delegate void UIEventChangeHandler(string description, int value);

    public UIEventChangeHandler onUIChange;

    private void Awake() {


        SetUpReferences();

        instance = this;

        onUpdateEvent += time.IncrementTime;

        onUIChange += lives.UpdateText;

        InitialiseAll();

    }

    private void Start() {

        Initialise();
    }

    private void Update() {
        
        if (gameStarted) {
            onUpdateEvent?.Invoke();
        }
    }

    public override void Initialise() {

        base.Initialise();

        gameStarted = true;
    }

    public override void InitialiseAll() {

        base.InitialiseAll();

        time.Initialise();

        lives.Initialise();
    }

    public override void SetUpReferences() {

        base.SetUpReferences();

        time = GetComponent<UITime>();

        lives = GetComponent<UILives>();
    }
}
