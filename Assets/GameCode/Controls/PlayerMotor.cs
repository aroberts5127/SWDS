using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public enum Direction { LEFT, RIGHT, NONE}


public class PlayerMotor : MonoBehaviour {

    public Direction CurrentDirection;

    //0.05 - 0.25 (5 levels .04 each level)
    public float moveSpeed;
    public float animSpeed;

    //public float FireRate;
    //private float startingFireRate = 2.5f;
    //public GunState CurrentGunState;
    //public FireState CurrentFireState;


    private bool isFiring;

    // Use this for initialization
    void Start () {
        ResetTransform();
        CurrentDirection = Direction.NONE;
        isFiring = false;
        //ChangeGuns();
        
	}
	
	// Update is called once per frame
	void Update () {
        //if (!isFiring)
        //{
        //    StartCoroutine(AutoFire());
        //}

    }

    private void FixedUpdate()
    {
        switch (CurrentDirection)
        {
            case (Direction.LEFT):
                //this.transform.position.Set(this.transform.position.x, -3.1f, this.transform.position.z);
                if (this.transform.position.x > -Cannon_Global.Instance.MovementMaxMin)
                    this.transform.position += new Vector3(-1.0f, 0.0f, 0.0f) * moveSpeed;
                break;
            case (Direction.RIGHT):
                //this.transform.position.Set(this.transform.position.x, -3.1f, this.transform.position.z);
                if (this.transform.position.x < Cannon_Global.Instance.MovementMaxMin)
                    this.transform.position += new Vector3(1.0f, 0.0f, 0.0f) * moveSpeed;
                break;
            case (Direction.NONE):
                //this.transform.position.Set(this.transform.position.x, -3.0f, this.transform.position.z);
                break;
        }
    }


    /// <summary>
    /// DEPRECATED
    /// While the Game Is Running this handles the timing of firing bullets from the
    /// Various weapons the player may end up using.
    /// </summary>
    /// <returns></returns>
    public IEnumerator AutoFire()
    {
        yield return null;
/*
        while (Cannon_Global.Instance.GameRunning)
        {
            isFiring = true;
            switch (CurrentGunState) {
                case (GunState.CANNON):
                    switch (CurrentFireState)
                    {
                        case (FireState.SINGLE):
                            GameObject newBullet = Instantiate(Cannon_Global.Instance.Assets.BulletPrefab, Cannon_Global.Instance.Assets.BulletParent, false);
                            newBullet.transform.position = Cannon_Global.Instance.Assets.BulletSpawn[0].position;

                            break;
                        case (FireState.DOUBLE):
                            GameObject newBullet2 = Instantiate(Cannon_Global.Instance.Assets.BulletPrefab, Cannon_Global.Instance.Assets.BulletParent, false);
                            newBullet2.transform.position = Cannon_Global.Instance.Assets.BulletSpawn[0].position;
                            GameObject newBullet3 = Instantiate(Cannon_Global.Instance.Assets.BulletPrefab, Cannon_Global.Instance.Assets.BulletParent, false);
                            newBullet3.transform.position = Cannon_Global.Instance.Assets.BulletSpawn[1].position;
                            break;
                    }
                    break;
                case (GunState.LAZER):
                    switch (CurrentFireState)
                    {
                        case (FireState.SINGLE):
                            Debug.Log("Here");
                            RaycastHit[] hit;
                            hit = Physics.RaycastAll(Cannon_Global.Instance.Assets.BulletSpawn[0].position, Vector3.up * 10);
                            //Debug.DrawRay(Cannon_Global.Instance.Assets.BulletSpawn[0].position, Vector3.up * 10, Color.red);
                            foreach(RaycastHit h in hit)
                            {
                                if (h.collider.gameObject.tag == "Enemy")
                                {
                                    //h.collider.gameObject.GetComponent<Enemy>().TakeDamage(1, false);
                                }
                            }

                            break;
                        case (FireState.DOUBLE):
                            RaycastHit[] hit1;
                            RaycastHit[] hit2;
                            hit1 = Physics.RaycastAll(Cannon_Global.Instance.Assets.BulletSpawn[0].position, Vector3.up * 10);
                            hit2 = Physics.RaycastAll(Cannon_Global.Instance.Assets.BulletSpawn[1].position, Vector3.up * 10);
                            foreach (RaycastHit h1 in hit1)
                            {
                                if (h1.collider.gameObject.tag == "Enemy")
                                {
                                    //Debug.Log("Hitting Enemy");
                                    //h1.collider.gameObject.GetComponent<Enemy>().TakeDamage(1, false);
                                }
                            }
                            foreach (RaycastHit h2 in hit2)
                            {
                                if (h2.collider.gameObject.tag == "Enemy")
                                {
                                    //Debug.Log("Hitting Enemy");
                                    //h2.collider.gameObject.GetComponent<Enemy>().TakeDamage(1, false);
                                }
                            }
                            break;
                    }
                    break;
        }
            if (FireRate == 0)
            {
                yield return null;
            }
            else
                yield return new WaitForSeconds(1 / FireRate);
        }
        isFiring = false;
       */
    }

    /// <summary>
    /// Used to Reset the player to the 'Zero' spot on the map
    /// Also Resets the gun.
    /// Used at start and on death/damage
    /// </summary>
    public void ResetTransform()
    {
        //Debug.Log("This Happens");
        //Debug.Log(Cannon_Global.Instance.CurrentGun.name);
        this.transform.position = Cannon_Global.Instance.Assets.PlayerStart;
        Cannon_Global.Instance.CurrentGun.currentShotType = ShotType.SINGLE;
        Cannon_Global.Instance.CurrentGun.SingleShotLevel = 1;
        Cannon_Global.Instance.CurrentGun.SpreadShotLevel = 1;
        Cannon_Global.Instance.CurrentGun.ResetFireRate();
        Cannon_Global.Instance.CurrentGun.singleShotAmmunition = Cannon_Global.Instance.Assets.SingleShotBulletTypes[0];
        Cannon_Global.Instance.CurrentGun.spreadShotAmmunition = Cannon_Global.Instance.Assets.SpreadShotBulletTypes[0];
        //ChangeGuns();
        //ChangeCannons(GunState.CANNON, FireState.SINGLE);
    }

    /// <summary>
    /// DEPRECATED
    /// This Function will update the FireState, Change The Physical Representation
    /// in the game of how many/what type of guns you are using, and modify any other
    /// properties or attributes that need adjusting
    /// 
    /// Will be called when the Player collects a powerup.
    /// </summary>
    /// <param name="fs"></param>
    public void ChangeCannons(GunState gs = GunState.CANNON)// FireState fs = FireState.SINGLE)//This will be modified to work with a Collectable
    {
/*        Cannon_Global.Instance.Assets.BulletSpawn.Clear();
        foreach (GameObject o in Cannon_Global.Instance.Assets.CannonList)
        {
            if (o.GetComponent<GunTypeHelper>().GunState != gs)
                o.SetActive(false);
            else
            {
                o.SetActive(true);
                foreach(Transform t in o.transform)
                {
                    if (t.gameObject.GetComponent<CannonStateHelper>().FireState == fs) {
                        t.gameObject.SetActive(true);
                        foreach(Transform x in t)
                        {
                            Cannon_Global.Instance.Assets.BulletSpawn.Add(x.GetChild(1));
                        }
                    }
                    else
                    {
                        t.gameObject.SetActive(false);
                    }
                }
            }
        }
        CurrentGunState = gs;
        CurrentFireState = fs;
        

        if(CurrentGunState == GunState.CANNON)
        {
            FireRate = startingFireRate;
        }
        else
        {
            FireRate = 4f;
        }
        */
    }

    /// <summary>
    /// DEPRECATED
    /// </summary>
    /// <param name="gs"></param>
    public void ChangeGuns(GunState gs = GunState.CANNON)
    {
        GameObject oldGun = Cannon_Global.Instance.CurrentGun.gameObject;
        GameObject gun;
        if(gs == GunState.CANNON)
            gun = Instantiate(Cannon_Global.Instance.Assets.GunObject, Cannon_Global.Instance.Player.transform, false);
        else if (gs == GunState.LAZER)
            gun = Instantiate(Cannon_Global.Instance.Assets.LaserObject, Cannon_Global.Instance.Player.transform, false);
        else
            gun = Instantiate(Cannon_Global.Instance.Assets.GunObject, Cannon_Global.Instance.Player.transform, false); ;
        ConstraintSource s = new ConstraintSource();
        s.sourceTransform = Cannon_Global.Instance.Assets.WeaponParent;
        s.weight = 1;
        gun.GetComponent<ParentConstraint>().SetSource(0, s);
        Destroy(oldGun);

        Cannon_Global.Instance.CurrentGun = gun.GetComponent<GunBase>();

        //CurrentGunState = gs;
        //CurrentFireState = FireState.SINGLE;
    }

    public void UpgradeGun(ShotType s)
    {
        //Debug.Log(gs + " Upgrading");
        Cannon_Global.Instance.CurrentGun.UpgradeWeapon(s);
        //CurrentFireState = FireState.DOUBLE;
    }


}
