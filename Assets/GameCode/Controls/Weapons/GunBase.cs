using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShotType { SINGLE, SPREAD}

public class GunBase : MonoBehaviour {

    public Transform AmmoSpawn;
    public float FireRate;

    public int SpreadShotLevel;
    public int SingleShotLevel;

    private float singleShotCurFireRate;
    private float spreadShotCurFireRate;

    private float spreadShotMaxFireRate = 2.5f;
    private float singleShotMaxFireRate = 4.0f;

    public float singleShotFireRateMod;
    public float spreadShotFireRateMod;

    public float singleShotBaseFireRate;
    public float spreadShotBaseFireRate;


    public GameObject singleShotAmmunition;
    public GameObject spreadShotAmmunition;
    public AudioSource GunSound;
    public ShotType currentShotType;
    public Coroutine FireRoutine;

    // Use this for initialization
    void Start() {
        Cannon_Global.Instance.CurrentGun = this;
        AmmoSpawn = this.transform.GetChild(0);
        currentShotType = ShotType.SINGLE;
    }

    private void FixedUpdate()
    {
        if (Cannon_Global.Instance.CurrentGameState == GameState.PLAYING)
        {
            if (FireRoutine == null && !Cannon_Global.Instance.Player.isInvulnerable)
                FireRoutine = StartCoroutine(AutoFire());
        }
    }

    public IEnumerator AutoFire()
    {
        switch (currentShotType) {
            case ShotType.SINGLE:
                GameObject sbullet = Instantiate(singleShotAmmunition, Cannon_Global.Instance.Assets.BulletParent, false);
                sbullet.transform.position = AmmoSpawn.position;
                GunSound.PlayOneShot(Cannon_Global.Instance.Assets.GunSound, .5f * Cannon_Global.Instance.Audio.masterVolume * Cannon_Global.Instance.Audio.soundVolume);
                break;
            case ShotType.SPREAD:
                GameObject spbullet1 = Instantiate(spreadShotAmmunition, Cannon_Global.Instance.Assets.BulletParent, false);
                spbullet1.transform.position = AmmoSpawn.position;
                GunSound.PlayOneShot(Cannon_Global.Instance.Assets.GunSound, .5f * Cannon_Global.Instance.Audio.masterVolume * Cannon_Global.Instance.Audio.soundVolume);
                break;
        }
        yield return new WaitForSeconds(1 / FireRate);
        FireRoutine = null;
    }

    public void UpgradeWeapon(ShotType s)
    {
        switch (s) {
                   
            case ShotType.SINGLE:
                if (SingleShotLevel < 3)
                    this.SingleShotLevel++;
                if (this.singleShotCurFireRate < this.singleShotMaxFireRate)
                {
                    this.singleShotCurFireRate += singleShotFireRateMod;
                    if (this.singleShotCurFireRate > this.singleShotMaxFireRate)
                        this.singleShotCurFireRate = this.singleShotMaxFireRate;
                }
                singleShotAmmunition = Cannon_Global.Instance.Assets.SingleShotBulletTypes[this.SingleShotLevel - 1];
                break;

            case ShotType.SPREAD:
                if (SpreadShotLevel < 3)
                    this.SpreadShotLevel++;
                if (this.spreadShotCurFireRate < this.spreadShotMaxFireRate)
                {
                    this.spreadShotCurFireRate += spreadShotFireRateMod;
                    if (this.spreadShotCurFireRate > this.spreadShotMaxFireRate)
                        this.spreadShotCurFireRate = this.spreadShotMaxFireRate;
                }
                spreadShotAmmunition = Cannon_Global.Instance.Assets.SpreadShotBulletTypes[this.SpreadShotLevel - 1];
                break;
        }
    }

    public void ResetFireRate()
    { 
        singleShotCurFireRate = singleShotBaseFireRate;
        spreadShotCurFireRate = spreadShotBaseFireRate;
    }

    public void UpdateShotSpeed()
    {
        switch (currentShotType)
        {
            case ShotType.SINGLE:
                FireRate = singleShotCurFireRate;
                break;
            case ShotType.SPREAD:
                FireRate = spreadShotCurFireRate;
                break;
        }
    }
}
