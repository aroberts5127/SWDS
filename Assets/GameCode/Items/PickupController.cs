using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickupType { NONE, BOMB, SINGLE, TURRET, LIFE, SPREAD}

public class PickupController : MonoBehaviour {

    public PickupType pType;
    public int pickupValue;

	// Use this for initialization


    private void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.tag == "Player")
        {
            Cannon_Global.Instance.Audio.PickupAudio();
            other.gameObject.GetComponentInParent<PlayerController>().CollectPickup(pType);
            Cannon_Global.Instance.Presentation.Score += pickupValue;
            Cannon_Global.Instance.Presentation.UpdateScore();
            Cannon_Global.Instance.Presentation.SpawnPointGainObj(pickupValue);
        }
        else
        {
            return;
        }
        Destroy(this.gameObject);
    }
}