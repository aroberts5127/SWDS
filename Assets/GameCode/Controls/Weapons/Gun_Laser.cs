using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Laser : MonoBehaviour {

    public float AudioVolume;
    private List<Collider> collidedWith;
    //private int hitTargets = 1;
    //zprivate List<GameObject> LazerAmmo;

    public bool active;

    public Transform AmmoSpawn;
    public GameObject ammunition;
    public AudioSource GunSound;

    public Coroutine FireRoutine;
    public float FireRate;

	// Use this for initialization
	void Start () {
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
