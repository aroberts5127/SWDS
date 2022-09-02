using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemRarity { NONE, COMMON, UNCOMMON, RARE, ULTRARARE}
public class Enemy : MonoBehaviour {
    public int curHealth;

    public Coroutine DamageRoutine;

    public AudioClip takeDamageSound;
    public AudioSource audioSource;

    public Rigidbody rb;

    private bool canTakeDamage;
    private bool dying;

    public Transform TextObj;
	void Start () {
        canTakeDamage = true;
	}

    public void SetHealth(int h)
    {
        curHealth = h;
        UpdateHealthDisplay();
    }

    public void AddCollisionStrength(float colStr, Transform incTransform)
    {
        rb.AddForce(incTransform.up * colStr, ForceMode.VelocityChange);
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
        if (canTakeDamage)
        {
            canTakeDamage = false;
            curHealth -= incDamage;
            UpdateHealthDisplay();
            if (curHealth <= 0)
            {
                Die();
                yield return null;
            }
            audioSource.PlayOneShot(takeDamageSound, .1f * Cannon_Global.Instance.Audio.masterVolume * Cannon_Global.Instance.Audio.soundVolume);
            Cannon_EventHandler.instance.gainPointsHandler(10);
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

    private void Die(bool earnRewards = true)
    {
        dying = true;
        Cannon_Global.Instance.Audio.EnemyDeadAudio();
        Cannon_Global.Instance.CurrentEnemyCount -= 1;
        if (earnRewards)
        {
            Cannon_EventHandler.instance.gainPointsHandler(50);
            int r = Random.Range(0, 100);
            if (r < 30)
            {
                Cannon_Global.Instance.Presentation.SpawnLoot(this.transform);
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
                    rb.AddForce(collision.collider.GetComponentInParent<EnemyColliderDetection>().EnemyForceMagnitude * (rb.velocity.normalized - collision.collider.GetComponentInParent<Rigidbody>().velocity.normalized), ForceMode.VelocityChange);
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
                    rb.AddForce(pushDirection * collision.collider.GetComponent<EnemyColliderDetection>().EnemyForceMagnitude * rb.mass, ForceMode.Impulse);
                
            }
        }
    }

    

    public void UpdateHealthDisplay()
    {
        TextObj.GetChild(0).GetComponent<TextMesh>().text = curHealth.ToString();
    }

}
