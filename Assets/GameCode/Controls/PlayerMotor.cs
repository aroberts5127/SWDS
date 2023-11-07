using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public enum Direction { LEFT, RIGHT, NONE}


public class PlayerMotor : MonoBehaviour {

    public Direction CurrentDirection;
    public Animator animator;

    public float moveSpeed;
    public float animSpeed;

    // Use this for initialization
    void Start () {
        ResetTransform();
        CurrentDirection = Direction.NONE;
        //ChangeGuns();
        
	}

    private void FixedUpdate()
    {
        switch (CurrentDirection)
        {
            case (Direction.LEFT):
                if (this.transform.position.x > -Cannon_Global.Instance.MovementMaxMin)
                    this.transform.position += new Vector3(-1.0f, 0.0f, 0.0f) * moveSpeed;
                break;
            case (Direction.RIGHT):
                if (this.transform.position.x < Cannon_Global.Instance.MovementMaxMin)
                    this.transform.position += new Vector3(1.0f, 0.0f, 0.0f) * moveSpeed;
                break;
            case (Direction.NONE):
                break;
        }
    }

    public void ResetTransform()
    {
        this.transform.position = Cannon_Global.Instance.Assets.PlayerStart;
        Cannon_Global.Instance.CurrentGun.currentShotType = ShotType.SINGLE;
        Cannon_Global.Instance.CurrentGun.SingleShotLevel = 1;
        Cannon_Global.Instance.CurrentGun.SpreadShotLevel = 1;
        Cannon_Global.Instance.CurrentGun.ResetFireRate();
        Cannon_Global.Instance.CurrentGun.singleShotAmmunition = Cannon_Global.Instance.Assets.SingleShotBulletTypes[0];
        Cannon_Global.Instance.CurrentGun.spreadShotAmmunition = Cannon_Global.Instance.Assets.SpreadShotBulletTypes[0];
    }

    public void UpgradeGun(ShotType s)
    {
        Cannon_Global.Instance.CurrentGun.UpgradeWeapon(s);
    }


}
