using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {
    public float speed;
    public int level;
    public float enemyCollisionStrength;

    private Collider col;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position += this.transform.up * speed;
	}

    public void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Enemy")
        {
            //Deal Damage
            other.GetComponentInParent<Enemy>().PublicTakeDamage();
            other.GetComponentInParent<Enemy>().AddCollisionStrength(enemyCollisionStrength, this.transform);
        }
        else
        {
            return;
        }
        //Debug.Log("Destroying Bullet! " + other.gameObject.name);
        if (level != 3)
        {
            Destroy(this.gameObject);
        }
        else
        {
            //Create Particle Effect
            this.speed = 0;
            this.gameObject.GetComponent<Collider>().enabled = false;
            this.transform.GetChild(0).gameObject.SetActive(false);
            for(int i = 1; i < this.transform.childCount; i++)
            {
                this.transform.GetChild(i).gameObject.SetActive(true);
            }
            StartCoroutine(BabyBulletTimer());
        }
    }

    private IEnumerator BabyBulletTimer()
    {
        while(this.transform.childCount > 1)
        {
            yield return null;
        }
        Destroy(this.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(this.transform.position, this.transform.up * 2);
    }

}
