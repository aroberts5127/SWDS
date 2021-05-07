using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventHelper : MonoBehaviour {

    public void DeactivateThisObject()
    {
        this.gameObject.SetActive(false);
    }

    public void DestroyThisObject()
    {
        Destroy(this.gameObject);
    }

    public void TurnOffAnimator()
    {
        this.GetComponent<Animator>().enabled = false;
    }

    public void SwitchGunIcons()
    {
        switch (Cannon_Global.Instance.CurrentGun.currentShotType) {
            case (ShotType.SINGLE):
                Cannon_Global.Instance.Presentation.SingleShotImage.transform.SetSiblingIndex(1);
                break;
            case (ShotType.SPREAD):
                Cannon_Global.Instance.Presentation.SpreadShotImage.transform.SetSiblingIndex(1);
                break;
        }
        Cannon_Global.Instance.Presentation.SwitchWeaponButton.GetComponent<Animator>().SetBool("Switch", false);
    }
}
