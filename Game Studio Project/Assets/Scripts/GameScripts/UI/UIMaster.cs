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

    public delegate void PlayerHitHandler();

    public PlayerHitHandler onPlayerHit;

    private void Awake() {


        SetUpReferences();

        InitialiseAll();

        instance = this;
    }

    private void Start() {

        Initialise();
    }

    private void Update() {
        
        if (gameStarted) {
            Debug.Log("Update");
            onUpdateEvent?.Invoke();
        }
    }

    public override void Initialise() {

        base.Initialise();

        gameStarted = true;

        onUpdateEvent += time.IncrementTime;

        onPlayerHit += lives.DecrementLives;
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
