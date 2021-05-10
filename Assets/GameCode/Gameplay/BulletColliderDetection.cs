using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletColliderDetection : MonoBehaviour {


    private void OnTriggerEnter(Collider collision)
    {

        if(collision.gameObject.tag == "Bullet")
        {
            Destroy(collision.gameObject);
        }
    }
}
