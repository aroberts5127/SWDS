using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Advertisements;
using TMPro;

public class Cannon_Presentation : MonoBehaviour, IUnityAdsListener {

    private static string gameID = "4123403";

    private int scoreMod;
    private int ScoreChangeValue = 1500;

    [Header("Start Menu UI")]
    public GameObject StartGameUIParent;
    public Button StartGameButton;
    public Button OptionsButton;
    public GameObject OptionsUIParent;
    public TextMeshProUGUI AccountNameTMP;
    public TextMeshProUGUI AccountCoinsTMP;

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
    private Animator switchWeaponButtonAnimator;

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
    public Button EndGameContinueButton;
    public Text EndGameScoreText;
    public GameObject HighScoreNotifyGO;


    private bool switchButtonStateBool;

    void Start () {
        Advertisement.Initialize(gameID);
        StartGamePresentation();
        SetAudioSliders();
        SetButtonFuntions();
        Score = 0;
        scoreMod = 0;
        Cannon_EventHandler.userLoadedEvent += updatePlayerName;
        Cannon_EventHandler.userCurrencyUpdateEvent += updateAccountCurrencyDisplay;
        Cannon_Global.Instance.presentationFinished = true;
        Cannon_EventHandler.gainPointsEvent += gainPointsCall;
        Cannon_EventHandler.playerHitEvent += UpdateLifeCounter;
        Cannon_EventHandler.useBombEvent += UpdateBombCounter;
        Cannon_EventHandler.collectBombEvent += UpdateBombCounterAndScore;
        
        Advertisement.AddListener(this);

        Cannon_EventHandler.instance.currencyUpdateHandler();
        Cannon_EventHandler.instance.userLoadedHandler();
        switchButtonStateBool = true;
        switchWeaponButtonAnimator = SwitchWeaponButton.GetComponent<Animator>();
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

    public void StartGamePresentation()
    {
        Cannon_Global.Instance.GameRunning = false;
        StartGameUIParent.SetActive(true);
        InGameUIParent.SetActive(false);
        EndGameUIParent.SetActive(false);
    }
    public void EndGamePresentation()
    {
        //ADS HERE!?
        //if (Advertisement.IsReady())
        //{
        //    Advertisement.Show("Video");
        //}
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
        Cannon_EventHandler.instance.resetEnemyCountHandler();
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

    public IEnumerator OnClickStartGame()
    {
        Cannon_Global.Instance.CurrentGameState = GameState.LOADING;
        StartGameUIParent.SetActive(false);
        InGameUIParent.SetActive(true);
        EndGameUIParent.SetActive(false);
        EndGameContinueButton.gameObject.SetActive(true);
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
        scoreMod = 0;
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
        EndGameContinueButton.onClick.AddListener(delegate { OnClick_PlayAdToContinue(); });
        OptionsButton.onClick.AddListener(delegate { OnClick_OpenOptions(); });
        OptionsCloseButton.onClick.AddListener(delegate { OnClick_CloseOptions(); });
        SwitchWeaponButton.onClick.AddListener(delegate { OnClickSwitchWeapons(); });
    }

    private void OnClick_PlayAdToContinue()
    {
        if (Advertisement.IsReady("video"))
        {
            Advertisement.Show("video");
        }

    }

    public void SpawnPointGainObj(int pointValue)
    {
        GameObject pointItem = Instantiate(Cannon_Global.Instance.Assets.PointGainObj, Cannon_Global.Instance.Assets.PointGainDisplayParent, false);
        pointItem.GetComponent<Text>().text = "+" + pointValue.ToString();
        pointItem.transform.SetParent(Cannon_Global.Instance.Assets.PointGainDisplayParent);
        pointItem.GetComponent<Animation>().Play();
    }

    public void SpawnBombGainObj()
    {
        GameObject bombItem = Instantiate(Cannon_Global.Instance.Assets.BombGainObj, Cannon_Global.Instance.Assets.BombGainDisplayParent, false);
        bombItem.transform.SetParent(Cannon_Global.Instance.Assets.BombGainDisplayParent);
    }

    public void SpawnWeaponGainObj(ShotType s)
    {
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
        WeaponItem.transform.SetParent(Cannon_Global.Instance.Assets.WeaponGainDisplayParent);
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
        switchWeaponButtonAnimator.SetBool("Switch", !switchButtonStateBool);
    }
    public void SwitchWeaponHelper(bool state)
    {
        switchWeaponButtonAnimator.SetBool("Switch", state);
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

    public void SpawnLoot(Transform spawnPoint)
    {
        //Debug.Log("Spawn Loot");

        int itemListLength = 1;
        ItemRarity itemList = ItemRarity.NONE;
        int itemQuality = Random.Range(0, 10);

        if (itemQuality == 0)
        {
            //UltraRare
            itemList = ItemRarity.ULTRARARE;
            itemListLength = Cannon_Global.Instance.Assets.Database.URItems.Length;
        }
        else if (itemQuality > 0 && itemQuality <= 2)
        {
            //Rare
            itemList = ItemRarity.RARE;
            itemListLength = Cannon_Global.Instance.Assets.Database.RItems.Length;
        }
        else if (itemQuality > 2 && itemQuality <= 5)
        {
            itemList = ItemRarity.UNCOMMON;
            itemListLength = Cannon_Global.Instance.Assets.Database.UCItems.Length;
        }
        else
        {
            itemList = ItemRarity.COMMON;
            itemListLength = Cannon_Global.Instance.Assets.Database.CItems.Length;
        }

        int r = Random.Range(0, itemListLength);
        ItemData[] list = Cannon_Global.Instance.Assets.Database.CItems;
        switch (itemList)
        {
            case (ItemRarity.COMMON):
                list = Cannon_Global.Instance.Assets.Database.CItems;
                break;
            case (ItemRarity.UNCOMMON):
                list = Cannon_Global.Instance.Assets.Database.UCItems;
                break;
            case (ItemRarity.RARE):
                list = Cannon_Global.Instance.Assets.Database.RItems;
                break;
            case (ItemRarity.ULTRARARE):
                list = Cannon_Global.Instance.Assets.Database.URItems;
                break;
            case (ItemRarity.NONE):
                return;
        }
        GameObject pickup = Instantiate(list[r].RepObject);
        pickup.transform.SetParent(Cannon_Global.Instance.Assets.PickupParent);
        pickup.transform.position = spawnPoint.position;
    }

    public void OnUnityAdsReady(string placementId)
    {
        
    }

    public void OnUnityAdsDidError(string message)
    {
        
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        //Probably want to send some kind of analytics back to me when I set up a webserver
    }

    public void OnUnityAdsDidFinish(string surfacingId, ShowResult showResult)
    {
        Debug.Log(surfacingId);
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            // Reward the user for watching the ad to completion.
            if(surfacingId == "video")
            {
                continueGameFromEndScreen();
            }
        }
        else if (showResult == ShowResult.Skipped)
        {
            if (surfacingId == "video")
            {
                continueGameFromEndScreen();
            }
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error.");
        }
    }

    private void continueGameFromEndScreen()
    {
        Cannon_Global.Instance.Player.curHealth = 1;
        Cannon_Global.Instance.CurrentGameState = GameState.LOADING;
        StartGameUIParent.SetActive(false);
        InGameUIParent.SetActive(true);
        EndGameUIParent.SetActive(false);
        Cannon_Global.Instance.Player.SetCharacterToReady();
        UpdatePresentation();
        Cannon_Global.Instance.CurrentGameState = GameState.PLAYING;
        Cannon_Global.Instance.GameRunning = true;
        EndGameContinueButton.gameObject.SetActive(false);
    }

    private void updatePlayerName()
    {
        AccountNameTMP.text = Cannon_Global.Instance.Account._name;
    }

    private void updateAccountCurrencyDisplay()
    {
        AccountCoinsTMP.text = Cannon_Global.Instance.Account.currency.ToString();
    }
}
