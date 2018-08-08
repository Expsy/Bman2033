using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotionScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    private void OnTriggerEnter(Collider other)
    {
        ball.ballSpeed = 2.2f;
        Pathfinding.headSpeed = 2f;

        Destroy(gameObject);
    }
}
