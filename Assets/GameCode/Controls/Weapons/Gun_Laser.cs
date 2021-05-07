using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Laser : MonoBehaviour {

    public float AudioVolume;
    private List<Collider> collidedWith;
    private int hitTargets = 1;
    //zprivate List<GameObject> LazerAmmo;

    public bool active;

    public Transform AmmoSpawn;
    public GameObject ammunition;
    public AudioSource GunSound;

    public Coroutine FireRoutine;
    public float FireRate;
    private GunState gunState;

	// Use this for initialization
	void Start () {
        gunState = GunState.LAZER;
        //setAudioVolume();
        //collidedWith = new List<Collider>();
        //active = false;
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (Cannon_Global.Instance.CurrentGameState == GameState.PLAYING && active == true)
        {
            if(FireRoutine == null)
                FireRoutine = StartCoroutine(AutoFire());
        }
    }

    public IEnumerator AutoFire()
    {
        /* Raycast Firing for Constant Lazer If I Wish To Reimplement It
        //Debug.Log("Here");
        RaycastHit[] hitList;
        //RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("Enemy");
        hitList = Physics.RaycastAll(AmmoSpawn.position, Vector3.up, 50.0f, mask);
        if (hitList.Length > 0)
        {
            hitList[0].collider.gameObject.GetComponentInParent<Enemy>().PublicTakeDamage(1, gunState, 1 / FireRate);
        }
        yield return new WaitForFixedUpdate();
        FireRoutine = null;
        */
        GameObject sbullet = Instantiate(ammunition, Cannon_Global.Instance.Assets.BulletParent, false);
        sbullet.transform.position = AmmoSpawn.position;
        GunSound.PlayOneShot(Cannon_Global.Instance.Assets.GunSound, .5f * Cannon_Global.Instance.Audio.masterVolume * Cannon_Global.Instance.Audio.soundVolume);

        yield return new WaitForSeconds(1 / FireRate);
        FireRoutine = null;
    }

    private void setAudioVolume()
    {
        AmmoSpawn.transform.GetComponentInChildren<AudioSource>().volume = AudioVolume * Cannon_Global.Instance.Audio.masterVolume * Cannon_Global.Instance.Audio.soundVolume;
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawRay(AmmoSpawn.position, Vector3.up * 10);
    //}


    //TODO - make this function so that it can be used during Pause as well if possible
    //And Turn off a little more smoothly. Shutting it off with SetActive() makes the ray disappea
    //outright. Can stop the particle effect and sound, then turn them off completely
    public void TurnOffGun()
    {
        AmmoSpawn.transform.GetChild(0).gameObject.SetActive(false);
        AmmoSpawn.transform.GetChild(1).gameObject.SetActive(false);
    }

    public void TurnOnGun()
    {
       AmmoSpawn.transform.GetChild(0).gameObject.SetActive(true);
    }
}
