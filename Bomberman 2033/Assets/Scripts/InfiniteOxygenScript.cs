using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteOxygenScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Movement.infiniteOxygen = true;
        O2_Script.infiniteOxygenOnceBool = true;
        O2_Script.O2Bar.fillAmount = 1;
    }
}
