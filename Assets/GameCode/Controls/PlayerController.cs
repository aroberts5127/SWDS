using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{

    #region Variables
    public PlayerMotor Motor;

    public int curHealth;

    [SerializeField]
    private int maxBombs;
    private int curBombs;

    public int CurrentBombs
    {
        set { curBombs = value; }
        get { return curBombs; }
    }

    public bool isInvulnerable;

    public Coroutine DamageRoutine;

    private int currentLazerCount;
    public List<Transform> LaserPositions;

    #endregion



    #region MovementVariables

    private Vector2 direction;
    private Vector2 touchPos;

    #endregion
    // Use this for initialization
    void Start()
    {
        if (Cannon_Global.Instance.Player == null)
        {
            Cannon_Global.Instance.Player = this;
        }
        else
        {
            DestroyImmediate(this.gameObject);
        }
        if (Motor == null)
        {
            Motor = this.gameObject.GetComponent<PlayerMotor>();
        }
        
        isInvulnerable = false;
        curHealth = Cannon_GlobalSettings.MAXSTARTINGLIVES;
        curBombs = 0;
        currentLazerCount = 0;

        Cannon_Global.Instance.playerReady = true;
    }

    void Update()
    {
        if (Cannon_Global.Instance.GameRunning)
        {
#if UNITY_EDITOR
            if (!Input.anyKey)
            {
                Motor.GetComponentInChildren<Animator>().speed = 1;
                Motor.CurrentDirection = Direction.NONE;
                Motor.GetComponentInChildren<Animator>().SetFloat("Walking Direction", 0);
                Motor.GetComponentInChildren<Animator>().SetBool("Moving", false);
                
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                Motor.GetComponentInChildren<Animator>().speed = 1 / Motor.animSpeed;
                Motor.CurrentDirection = Direction.LEFT;
                Motor.GetComponentInChildren<Animator>().SetFloat("Walking Direction", -1);
                Motor.GetComponentInChildren<Animator>().SetBool("Moving", true);
                
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                Motor.GetComponentInChildren<Animator>().speed = 1 / Motor.animSpeed;
                Motor.CurrentDirection = Direction.RIGHT;
                Motor.GetComponentInChildren<Animator>().SetFloat("Walking Direction", 1);
                Motor.GetComponentInChildren<Animator>().SetBool("Moving", true);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                curBombs = 3;
                Cannon_Global.Instance.Presentation.UpdatePresentation();
            }
#endif
#if UNITY_ANDROID
            if (Input.touchCount > 0)
            {
                Touch CurrentTouch = Input.GetTouch(0);
                if (EventSystem.current.IsPointerOverGameObject(0))
                {
                    return;
                }
                Vector3 screenPos = Camera.main.ScreenToWorldPoint(CurrentTouch.position);
                switch (CurrentTouch.phase)
                {
                    case (TouchPhase.Began):
                        touchPos = CurrentTouch.position;
                        Cannon_Global.Instance.Presentation.TouchIndicator.transform.position = new Vector3(screenPos.x, screenPos.y, -10);
                        Cannon_Global.Instance.Presentation.TouchIndicator.SetActive(true);
                        Cannon_Global.Instance.Presentation.TouchIndicator.transform.GetChild(0).gameObject.SetActive(true);
                        Cannon_Global.Instance.Presentation.TouchIndicator.transform.GetChild(1).gameObject.SetActive(true);
                        Cannon_Global.Instance.Presentation.TouchIndicator.transform.GetChild(2).gameObject.SetActive(true);
                        Motor.CurrentDirection = Direction.NONE;
                        break;
                    case (TouchPhase.Moved):
                        direction = CurrentTouch.position - touchPos;
                        if(direction.x > 0.1f)
                        {
                            Cannon_Global.Instance.Presentation.TouchIndicator.transform.GetChild(0).gameObject.SetActive(false);
                            Cannon_Global.Instance.Presentation.TouchIndicator.transform.GetChild(1).gameObject.SetActive(true);
                            Cannon_Global.Instance.Presentation.TouchIndicator.transform.GetChild(2).gameObject.SetActive(true);
                            Motor.CurrentDirection = Direction.RIGHT;
                            Motor.GetComponentInChildren<Animator>().speed = 1 / Motor.animSpeed;
                            Motor.GetComponentInChildren<Animator>().SetFloat("Walking Direction", 1);
                            Motor.GetComponentInChildren<Animator>().SetBool("Moving", true);
                        }
                        else if(direction.x < -0.1f)
                        {
                            Cannon_Global.Instance.Presentation.TouchIndicator.transform.GetChild(0).gameObject.SetActive(true);
                            Cannon_Global.Instance.Presentation.TouchIndicator.transform.GetChild(1).gameObject.SetActive(true);
                            Cannon_Global.Instance.Presentation.TouchIndicator.transform.GetChild(2).gameObject.SetActive(false);
                            Motor.CurrentDirection = Direction.LEFT;
                            Motor.GetComponentInChildren<Animator>().speed = 1 / Motor.animSpeed;
                            Motor.GetComponentInChildren<Animator>().SetFloat("Walking Direction", -1);
                            Motor.GetComponentInChildren<Animator>().SetBool("Moving", true);
                        }
                        break;
                    case (TouchPhase.Ended):
                        Cannon_Global.Instance.Presentation.TouchIndicator.SetActive(false);
                        Motor.CurrentDirection = Direction.NONE;
                        Motor.GetComponentInChildren<Animator>().speed = 1;
                        Motor.GetComponentInChildren<Animator>().SetFloat("Walking Direction", 0);
                        Motor.GetComponentInChildren<Animator>().SetBool("Moving", false);
                        break;
                }
            }
#endif
        }
    }

    public void PublicTakeDamage()
    {
        //Debug.Log("Hit");
        if (DamageRoutine == null)
            DamageRoutine = StartCoroutine(TakeDamage());
    }
    private IEnumerator TakeDamage()
    {
        isInvulnerable = true;
        //Debug.Log("Taking Damage");
        curHealth--;
        Motor.GetComponentInChildren<Animator>().speed = 3;
        this.GetComponentInChildren<Animator>().SetBool("Hit", true);
        //Debug.Log(curHealth);
        if (curHealth == 0)
        {
            PlayerDies();
            this.GetComponentInChildren<Animator>().SetBool("Hit", false);
            yield return null;
        }
        else
        {
            Cannon_Global.Instance.Assets.ImmuneBarrier.SetActive(true);
            Cannon_Global.Instance.Assets.LeftWall.GetComponent<EnemyColliderDetection>().enabled = false;
            Cannon_Global.Instance.Assets.RightWall.GetComponent<EnemyColliderDetection>().enabled = false;
            Cannon_EventHandler.instance.playerHitHandler();
            yield return new WaitForSeconds(0.1f);
            this.GetComponentInChildren<Animator>().SetBool("Hit", false);
            while (Cannon_Global.Instance.Assets.ImmuneBarrier.activeSelf)
            {
                yield return null;
            }
            Cannon_Global.Instance.Assets.LeftWall.GetComponent<EnemyColliderDetection>().enabled = true;
            Cannon_Global.Instance.Assets.RightWall.GetComponent<EnemyColliderDetection>().enabled = true;

        }
        DamageRoutine = null;
        isInvulnerable = false;
        yield break;
    }

    private void PlayerDies()
    {
        Motor.CurrentDirection = Direction.NONE;
        Motor.GetComponentInChildren<Animator>().SetFloat("Walking Direction", 0);
        Motor.GetComponentInChildren<Animator>().SetBool("Moving", false);
        Motor.GetComponentInChildren<Animator>().SetBool("Playing", false);      
        Cannon_Global.Instance.Presentation.EndGamePresentation();
        isInvulnerable = false;
        curBombs = 0;
    }

    #region Collect Pickup Functions
    public void CollectPickup(PickupType type)
    {
        switch (type)
        {
            case (PickupType.BOMB):
                CollectBomb();
                break;

            case (PickupType.LIFE):
                CollectExtraLife();
                break;

            case (PickupType.SINGLE):
                Motor.UpgradeGun(ShotType.SINGLE);
                Cannon_Global.Instance.Presentation.SpawnWeaponGainObj(ShotType.SINGLE);

                break;

            case (PickupType.TURRET):
                SpawnLazerCannon();
                break;

            case (PickupType.SPREAD):
                Motor.UpgradeGun(ShotType.SPREAD);
                Cannon_Global.Instance.Presentation.SpawnWeaponGainObj(ShotType.SPREAD);
                break;
        }
    }

    private void CollectBomb()
    {
        
        if(curBombs < maxBombs)
        {
            curBombs++;
        }
        Cannon_EventHandler.instance.collectBombHandler();
    }

    private void CollectExtraLife()
    {
        curHealth++;
        Cannon_Global.Instance.Presentation.UpdatePresentation();
    }

    private void SpawnLazerCannon()
    {
        if(currentLazerCount < 2)
        {
            //Debug.Log(currentLazerCount);
            currentLazerCount++;
            GameObject laser = Instantiate(Cannon_Global.Instance.Assets.LaserPrefab, LaserPositions[currentLazerCount - 1], false);
            laser.transform.position = LaserPositions[currentLazerCount - 1].position;
            laser.GetComponent<Gun_Laser>().active = true;
        }
    }
    
    #endregion

    public IEnumerator OnClickUseBomb()
    {
        Cannon_Global.Instance.Presentation.UseBombButton.interactable = false;
        if (curBombs > 0)
        {
            Cannon_Global.Instance.Assets.PulseBomb.SetActive(true);
            while (Cannon_Global.Instance.Assets.PulseBomb.activeSelf)
            {
                yield return null;
            }
            curBombs--;
            Cannon_EventHandler.instance.useBombHandler();
        }
        else
        {
            yield return null;
        }
        if (curBombs > 0)
        {
            Cannon_Global.Instance.Presentation.UseBombButton.interactable = true;
        }
    }

    public void SetCharacterToReady()
    {
        this.transform.GetChild(0).GetComponent<Animator> ().SetBool("Playing", true);
        
    }


}
