using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Change bool GameRunning instances to CurrentGameState Checks;
public enum GameState { START, LOADING, PLAYING, ENDGAME}
public enum GunState { NONE, CANNON, LAZER }

public class Cannon_Global : MonoBehaviour {

    public static Cannon_Global Instance;
    public Cannon_Assets Assets;
    public Cannon_Presentation Presentation;
    public Cannon_Audio Audio;
    public PlayerController Player;
    public GunBase CurrentGun;
    public GameState CurrentGameState;

    public float MovementMaxMin;
    public bool GameRunning = false;

    public int MaxEnemyCount;
    public int CurrentEnemyCount;
    public int GamePhase;

    [HideInInspector]
    public bool presentationFinished;
    [HideInInspector]
    public bool playerReady;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        if (Assets == null)
            Assets = this.GetComponent<Cannon_Assets>();
        if (Presentation == null)
            Presentation = this.GetComponent<Cannon_Presentation>();
        if (Audio == null)
            Audio = this.GetComponent<Cannon_Audio>();
    }
    void Start () {
        StartCoroutine(LoadedIn());
	}

    private IEnumerator LoadedIn()
    {
        while(presentationFinished != true && playerReady != true)
            yield return null;
        if (!PlayerPrefs.HasKey(Cannon_GlobalSettings.HASPLAYED))
        {
            Cannon_SaveLoad.InitializeSaveData();
        }
        GamePhase = 1;
        CurrentGameState = GameState.START;
    }
}
