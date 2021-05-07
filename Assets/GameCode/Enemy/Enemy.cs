using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemRarity { NONE, COMMON, UNCOMMON, RARE, ULTRARARE}
public class Enemy : MonoBehaviour {
    public int curHealth;

    //public float BulletRecoil;
    //public SpriteRenderer HealthSprite;

    public Coroutine DamageRoutine;

    public AudioClip takeDamageSound;

    private bool canTakeDamage;
    private bool dying;

    public Transform TextObj;
	// Use this for initialization
	void Start () {
        canTakeDamage = true;
        //HealthSprite.sprite = Cannon_Global.Instance.Assets.EnemyHealthImageList[curHealth-1];
	}

    public void SetHealth(int h)
    {
        curHealth = h;
        UpdateHealthDisplay();
    }

    public void AddCollisionStrength(float colStr, Transform incTransform)
    {
        this.GetComponent<Rigidbody>().AddForce(incTransform.up * colStr, ForceMode.VelocityChange);
    }

    public void PublicTakeDamage(int p_incDamage = 1, GunState p_gs = GunState.CANNON, float p_fireDelay = 0.0f)
    {
        if (DamageRoutine == null)
        {
            DamageRoutine = StartCoroutine(TakeDamage(p_incDamage, p_gs, p_fireDelay));
        }

    }

    public IEnumerator TakeDamage(int incDamage, GunState gs, float fireDelay)
    {
        //Debug.Log("Before The IF");
        if (canTakeDamage)
        {
            //Debug.Log("In The IF");
            canTakeDamage = false;
            //Debug.Log("Enemy Takes Damage");
            curHealth -= incDamage;
            UpdateHealthDisplay();
            if (curHealth <= 0)
            {
                Die();
                yield return null;
            }
            this.GetComponent<AudioSource>().PlayOneShot(takeDamageSound, .1f * Cannon_Global.Instance.Audio.masterVolume * Cannon_Global.Instance.Audio.soundVolume);
            Cannon_Global.Instance.Presentation.Score += 10;
            Cannon_Global.Instance.Presentation.UpdateScore();
            Cannon_Global.Instance.Presentation.SpawnPointGainObj(10);
            //HealthSprite.sprite = Cannon_Global.Instance.Assets.EnemyHealthImageList[curHealth - 1];
            if (gs == GunState.LAZER)
            {
                yield return new WaitForSeconds(fireDelay);
                
            }
            else
            {
                yield return new WaitForEndOfFrame();
                
            }
            canTakeDamage = true;
        }
        DamageRoutine = null;
        //yield break;
        
    }

    public void PublicTakeBombDamage(bool earnPoints = true)
    {
        if(DamageRoutine != null)
        {
            StopCoroutine(DamageRoutine);
        }
        if(dying == false)
            Die(earnPoints);
    }
    /// <summary>
    /// Enemy Death. Happens when it's health falls to 0
    /// Runs conditional check to spawn powerups for the player to collect
    /// </summary>
    private void Die(bool earnRewards = true)
    {
        dying = true;
        Cannon_Global.Instance.Audio.EnemyDeadAudio();
        Cannon_Global.Instance.CurrentEnemyCount -= 1;
        if (earnRewards)
        {
            Cannon_Global.Instance.Presentation.Score += 50;
            Cannon_Global.Instance.Presentation.SpawnPointGainObj(50);
            Cannon_Global.Instance.Presentation.UpdateScore();
            int r = Random.Range(0, 100);
            if (r < 30)
            {
                SpawnLoot();
            }
        }
        Destroy(this.transform.parent.gameObject);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Bullet" || collision.gameObject.tag == "Pickup")
        {
            return;
        }

        if (collision.collider.gameObject.tag == "Player")
        {
            collision.collider.GetComponentInParent<PlayerController>().PublicTakeDamage();
        }
        else
        {
            if (collision.collider.gameObject.tag == "Enemy")
            {
                if (collision.collider.GetComponentInParent<EnemyColliderDetection>().ApplicableForce == ForcePushDirection.NONE)
                {
                    //Debug.Log(collision.gameObject.name);
                    //Debug.Log(collision.collider.GetComponentInParent<Rigidbody>().velocity);
                    //Debug.Log(collision.collider.GetComponentInParent<Rigidbody>().velocity.normalized);
                    this.GetComponent<Rigidbody>().AddForce(collision.collider.GetComponentInParent<EnemyColliderDetection>().EnemyForceMagnitude * (this.GetComponent<Rigidbody>().velocity.normalized - collision.collider.GetComponentInParent<Rigidbody>().velocity.normalized), ForceMode.VelocityChange);
                    //this.GetComponent<Rigidbody>().AddForce(collision.collider.GetComponentInParent<Rigidbody>().velocity.normalized * collision.collider.GetComponentInParent<EnemyColliderDetection>().EnemyForceMagnitude * this.GetComponentInParent<Rigidbody>().mass, ForceMode.Impulse);
                }
            }
            if (collision.collider.GetComponent<EnemyColliderDetection>() != null)
            {
                    Vector3 pushDirection = Vector3.up;
                    switch (collision.collider.GetComponent<EnemyColliderDetection>().ApplicableForce)
                    {
                        case (ForcePushDirection.RIGHT):
                            pushDirection = Vector3.right;
                            break;
                        case (ForcePushDirection.LEFT):
                            pushDirection = Vector3.left;
                            break;
                        case (ForcePushDirection.UP):
                            pushDirection = Vector3.up;
                            break;
                    }
                    this.GetComponent<Rigidbody>().AddForce(pushDirection * collision.collider.GetComponent<EnemyColliderDetection>().EnemyForceMagnitude * this.GetComponent<Rigidbody>().mass, ForceMode.Impulse);
                
            }
        }
    }

    private void SpawnLoot()
    {
        //Debug.Log("Spawn Loot");

        int itemListLength = 1;
        ItemRarity itemList = ItemRarity.NONE;
        int itemQuality = Random.Range(0, 10);

        if(itemQuality == 0)
        {
            //UltraRare
            itemList = ItemRarity.ULTRARARE;
            itemListLength = Cannon_Global.Instance.Assets.Database.URItems.Length;
        }
        else if(itemQuality > 0 && itemQuality <= 2)
        {
            //Rare
            itemList = ItemRarity.RARE;
            itemListLength = Cannon_Global.Instance.Assets.Database.RItems.Length;
        }
        else if(itemQuality > 2 && itemQuality <= 5)
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
        pickup.transform.position = this.transform.position;
    }

    public void UpdateHealthDisplay()
    {
        TextObj.GetChild(0).GetComponent<TextMesh>().text = curHealth.ToString();
    }

    //public void OnTriggerEnter(Collider other)
    //{
    //    if(other.gameObject.tag == "Bullet")
    //    {
    //        return;
    //    }
    //    if(other.gameObject.tag == "Player")
    //    {
    //        other.GetComponentInParent<PlayerController>().TakeDamage();
    //    }
    //    else
    //    {
    //        this.GetComponent<Rigidbody>().AddForce(new Vector3(0.0f, 1.0f, 0.0f) * other.GetComponent<EnemyColliderDetection>().EnemyForceMagnitude);
    //    }
    //}
}
