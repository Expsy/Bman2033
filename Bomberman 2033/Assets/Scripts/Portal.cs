using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Invoke("SelfDestruction", 2);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void SelfDestruction()
    {
        Destroy(gameObject);
    }
}
