using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletColliderDetection : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter(Collider collision)
    {
        //Debug.Log(collision.gameObject.name);
        if(collision.gameObject.tag == "Bullet")
        {
            //Debug.Log("Destroying Bullet");
            Destroy(collision.gameObject);
        }
    }
}
