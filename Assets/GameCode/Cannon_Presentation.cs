using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Cannon_Presentation : MonoBehaviour {

    private int scoreMod;
    private int ScoreChangeValue = 1500;

    [Header("Start Menu UI")]
    public GameObject StartGameUIParent;
    public Button StartGameButton;
    public Button OptionsButton;
    public GameObject OptionsUIParent;

    [Header("Options UI")]
    public Button OptionsCloseButton;
    public Slider MasterVolumeSlider;
    public Slider SfxSlider;
    public Slider MusicSlider;


    [Header("In Game UI")]
    public GameObject InGameUIParent;
    public Text ScoreText;
    public Text BombCountText;
    public Button UseBombButton;
    public GameObject TouchIndicator;
    public Image SingleShotImage;
    public Image SpreadShotImage;
    public Button SwitchWeaponButton;

    private int priv_Score;
    public int Score
    {
        get
        {
            return priv_Score;
        }
        set
        {
            priv_Score = value;
        }
    }

    public Text ReadyText;
    public Text GoText;

    [Header("End Game UI")]
    public GameObject EndGameUIParent;
    public Button EndGameRetryButton;
    public Button EndGameMenuButton;
    public Text EndGameScoreText;
    public GameObject HighScoreNotifyGO;

    // Use this for initialization
    void Start () {
        StartGamePresentation();
        SetAudioSliders();
        SetButtonFuntions();
        Score = 0;
        scoreMod = 0;
        Cannon_Global.Instance.presentationFinished = true;
        Cannon_EventHandler.gainPointsEvent += gainPointsCall;
        Cannon_EventHandler.playerHitEvent += UpdateLifeCounter;
        Cannon_EventHandler.useBombEvent += UpdateBombCounter;
        Cannon_EventHandler.collectBombEvent += UpdateBombCounterAndScore;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdatePresentation()
    {
        UpdateScore();
        UpdateBombCounter();
        UpdateLifeCounter();
    }

    private void gainPointsCall(int score)
    {
        Score += score;
        UpdateScore();
        SpawnPointGainObj(score);
    }

    private void UpdateScore()
    {
        //Debug.Log("Score: " + Score);
        if (Score < 9999999)
        {
            string scoreString = Score.ToString();
            int len = scoreString.Length;
            if (len < 7)
            {
                
                for (int i = 0; i < 7 - len; i++)
                {
                    scoreString = "0" + scoreString;
                }
            }
            ScoreText.text = scoreString;
            if(Score / ScoreChangeValue > scoreMod)
            {
                scoreMod = Score / ScoreChangeValue;
                Cannon_Global.Instance.GamePhase++;
            }
        }
        else
        {
            ScoreText.text = "9999999";
        }
    }

    private void UpdateBombCounter()
    {
        if (Cannon_Global.Instance.Player.CurrentBombs > 0)
        {
            UseBombButton.interactable = true;
        }
        else
        {
            UseBombButton.interactable = false;
        }
        BombCountText.text = Cannon_Global.Instance.Player.CurrentBombs.ToString();
    }
    private void UpdateBombCounterAndScore()
    {
        UpdateScore();
        UpdateBombCounter();
        SpawnBombGainObj();
    }

    private void UpdateLifeCounter()
    {
        //Debug.Log("Here");
        if(Cannon_Global.Instance.Player.curHealth > 3)
        {
            //Debug.Log("3+");
            Cannon_Global.Instance.Assets.HealthDisplayParent.GetChild(0).gameObject.SetActive(false);
            Cannon_Global.Instance.Assets.HealthDisplayParent.GetChild(1).gameObject.SetActive(true);
            Cannon_Global.Instance.Assets.HealthDisplayParent.GetChild(1).GetChild(1).GetComponent<Text>().text = "x" + Cannon_Global.Instance.Player.curHealth.ToString();
        }
        else
        {
            //Debug.Log("<3");
            Cannon_Global.Instance.Assets.HealthDisplayParent.GetChild(0).gameObject.SetActive(true);
            Cannon_Global.Instance.Assets.HealthDisplayParent.GetChild(1).gameObject.SetActive(false);

            for(int i = 0; i < 3; i++)
            {
                if(i < Cannon_Global.Instance.Player.curHealth)
                {
                    Cannon_Global.Instance.Assets.HealthDisplayParent.GetChild(0).GetChild(i).GetChild(0).gameObject.SetActive(true);
                    Cannon_Global.Instance.Assets.HealthDisplayParent.GetChild(0).GetChild(i).GetChild(1).gameObject.SetActive(false);
                }
                else
                {
                    Cannon_Global.Instance.Assets.HealthDisplayParent.GetChild(0).GetChild(i).GetChild(0).gameObject.SetActive(false);
                    Cannon_Global.Instance.Assets.HealthDisplayParent.GetChild(0).GetChild(i).GetChild(1).gameObject.SetActive(true);
                }
            }
        }
    }

    /// <summary>
    /// Sets the Presentation for the Start Screen, where you can navigate menus,
    /// settings, profile, and start the game
    /// </summary>
    public void StartGamePresentation()
    {
        Cannon_Global.Instance.GameRunning = false;
        StartGameUIParent.SetActive(true);
        InGameUIParent.SetActive(false);
        EndGameUIParent.SetActive(false);
        
    }

    /// <summary>
    /// Stops the game and presents the endgame presentation
    /// Tracks score, submits to server(eventually), and displays
    /// Titles, buttons, etc for continued user interaction
    /// </summary>
    public void EndGamePresentation()
    {
        Cannon_Global.Instance.CurrentGameState = GameState.ENDGAME;
        foreach(Transform t in Cannon_Global.Instance.Assets.EnemyParent)
        {
            Destroy(t.gameObject);
        }
        foreach (Transform t in Cannon_Global.Instance.Assets.BulletParent)
        {
            Destroy(t.gameObject);
        }
        EndGameScoreText.text = Score.ToString();
        Cannon_Global.Instance.CurrentEnemyCount = 0;
        Cannon_Global.Instance.GameRunning = false;
        StartGameUIParent.SetActive(false);
        InGameUIParent.SetActive(false);
        EndGameUIParent.SetActive(true);
        
        bool hs = CheckHighScore(Score);
        if (hs)
        {
            HighScoreNotifyGO.SetActive(true);
        }
        else
        {
            HighScoreNotifyGO.SetActive(false);
        }
    }

    /// <summary>
    /// Hides Start Screen Presentation Objects (Titles, Buttons, etc) and begins the game
    /// </summary>
    public IEnumerator OnClickStartGame()
    {
        Cannon_Global.Instance.CurrentGameState = GameState.LOADING;
        StartGameUIParent.SetActive(false);
        InGameUIParent.SetActive(true);
        EndGameUIParent.SetActive(false);
        
        UpdatePresentation();
        ReadyText.gameObject.SetActive(true);
        Cannon_Global.Instance.Player.SetCharacterToReady();
        while (ReadyText.gameObject.activeSelf)
        {
            yield return null;
        }
        GoText.gameObject.SetActive(true);
        while (GoText.gameObject.activeSelf)
        {
            yield return null;
        }

        Cannon_Global.Instance.CurrentGameState = GameState.PLAYING;
        Cannon_Global.Instance.GameRunning = true;
    }

    public void OnClick_Retry()
    {
        Score = 0;
        scoreMod = 0;
        Cannon_Global.Instance.Player.CurrentBombs = 0;
        Cannon_Global.Instance.Player.curHealth = Cannon_GlobalSettings.MAXSTARTINGLIVES;
        Cannon_Global.Instance.Player.Motor.ResetTransform();
        StartCoroutine(OnClickStartGame());
    }

    public void OnClick_ReturnToMenu()
    {
        Score = 0;
        Cannon_Global.Instance.Player.CurrentBombs = 0;
        Cannon_Global.Instance.Player.curHealth = Cannon_GlobalSettings.MAXSTARTINGLIVES;
        Cannon_Global.Instance.CurrentGameState = GameState.START;
        Cannon_Global.Instance.Player.Motor.ResetTransform();
        StartGameUIParent.SetActive(true);
        InGameUIParent.SetActive(false);
        EndGameUIParent.SetActive(false);
    }

    public void OnClick_OpenOptions()
    {
        OptionsUIParent.gameObject.SetActive(true);
    }

    public void OnClick_CloseOptions()
    {
        OptionsUIParent.gameObject.SetActive(false);
    }

    private void SetButtonFuntions()
    {
        StartGameButton.onClick.AddListener(delegate { StartCoroutine(OnClickStartGame()); });
        UseBombButton.onClick.AddListener(delegate { StartCoroutine(Cannon_Global.Instance.Player.OnClickUseBomb()); });
        EndGameRetryButton.onClick.AddListener(delegate { OnClick_Retry(); });
        EndGameMenuButton.onClick.AddListener(delegate { OnClick_ReturnToMenu(); });
        OptionsButton.onClick.AddListener(delegate { OnClick_OpenOptions(); });
        OptionsCloseButton.onClick.AddListener(delegate { OnClick_CloseOptions(); });
        SwitchWeaponButton.onClick.AddListener(delegate { OnClickSwitchWeapons(); });
    }

    public void SpawnPointGainObj(int pointValue)
    {
        GameObject pointItem = Instantiate(Cannon_Global.Instance.Assets.PointGainObj, Cannon_Global.Instance.Assets.PointGainDisplayParent, false);
        pointItem.GetComponent<Text>().text = "+" + pointValue.ToString();
        pointItem.transform.parent = Cannon_Global.Instance.Assets.PointGainDisplayParent;
        pointItem.GetComponent<Animation>().Play();
    }

    public void SpawnBombGainObj()
    {
        GameObject bombItem = Instantiate(Cannon_Global.Instance.Assets.BombGainObj, Cannon_Global.Instance.Assets.BombGainDisplayParent, false);
        bombItem.transform.parent = Cannon_Global.Instance.Assets.BombGainDisplayParent;
    }

    public void SpawnWeaponGainObj(ShotType s)
    {
        //Debug.Log(gs + " " + fs);
        string message = "";
        switch (s)
        {
            case (ShotType.SINGLE):
                message = "Single Fire Upgraded";
                break;
            case (ShotType.SPREAD):
                message = "Spread Shot Upgraded";
                break;
            
        }
        

        GameObject WeaponItem = Instantiate(Cannon_Global.Instance.Assets.WeaponGainObj, Cannon_Global.Instance.Assets.WeaponGainDisplayParent, false);
        WeaponItem.transform.parent = Cannon_Global.Instance.Assets.WeaponGainDisplayParent;
        WeaponItem.GetComponentInChildren<Text>().text = message;
    }

    public void OnClickSwitchWeapons()
    {
        if (Cannon_Global.Instance.CurrentGun.currentShotType == ShotType.SINGLE)
        {
            //Debug.Log("Switch To Spread Fire");
            Cannon_Global.Instance.CurrentGun.currentShotType = ShotType.SPREAD;
        }
        else
        {
            //Debug.Log("Switch to Single Fire");
            Cannon_Global.Instance.CurrentGun.currentShotType = ShotType.SINGLE;
            
        }
        Cannon_Global.Instance.CurrentGun.UpdateShotSpeed();
        SwitchWeaponButton.GetComponent<Animator>().SetBool("Switch", true);
    }

    public bool CheckHighScore(int scoreToCheck)
    {
        if (!PlayerPrefs.HasKey(Cannon_GlobalSettings.HIGHSCORE))
        {
            PlayerPrefs.SetInt(Cannon_GlobalSettings.HIGHSCORE, scoreToCheck);
            return true;
        }
        else
        {
            if(PlayerPrefs.GetInt(Cannon_GlobalSettings.HIGHSCORE) < scoreToCheck)
            {
                Cannon_SaveLoad.SaveHighScore(scoreToCheck);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public void SetAudioSliders()
    {
        MasterVolumeSlider.onValueChanged.AddListener(delegate { Cannon_Global.Instance.Audio.UpdateMasterVolume(); });
        SfxSlider.onValueChanged.AddListener(delegate { Cannon_Global.Instance.Audio.UpdateSFXVolume(); });
        MusicSlider.onValueChanged.AddListener(delegate { Cannon_Global.Instance.Audio.UpdateMusicVolume(); });
        if (PlayerPrefs.HasKey(Cannon_GlobalSettings.HASPLAYED))
        {
            MasterVolumeSlider.value = PlayerPrefs.GetFloat(Cannon_GlobalSettings.VOLUME_MASTER);
            SfxSlider.value = PlayerPrefs.GetFloat(Cannon_GlobalSettings.VOLUME_SOUNDS);
            MusicSlider.value = PlayerPrefs.GetFloat(Cannon_GlobalSettings.VOLUME_MUSIC);
        }
        else
        {
            MasterVolumeSlider.value = 1f;
            SfxSlider.value = 1f;
            MusicSlider.value = 1f;
        }
    }
}
