using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PulseType { BOMB, IMMUNE, FREEZE}

public class PulseBombController : MonoBehaviour {

    
    public PulseType pType;

    private void OnCollisionEnter(Collision collision)
    {
        if (pType == PulseType.BOMB)
        {
            collision.collider.gameObject.GetComponentInParent<Enemy>().PublicTakeBombDamage();
        }
        if(pType == PulseType.IMMUNE)
        {
            collision.collider.gameObject.GetComponentInParent<Enemy>().PublicTakeBombDamage(false);
        }

    }


}
