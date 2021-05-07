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
            Cannon_EventHandler.instance.gainPointsHandler(pickupValue);
        }
        else
        {
            return;
        }
        Destroy(this.gameObject);
    }
}