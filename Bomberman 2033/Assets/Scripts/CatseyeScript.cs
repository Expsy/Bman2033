using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatseyeScript : MonoBehaviour {



	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Movement.catseye = true;

        Destroy(gameObject);
    }

   
}
