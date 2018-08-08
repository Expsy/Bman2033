using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownPowerUpScript : MonoBehaviour {

    private Movement hero;

	// Use this for initialization
	void Start () {
        hero = FindObjectOfType<Movement>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Movement.invisibilityCooldownC = 5;
        Movement.TpFlaskCooldownC = 2.5f;
        Movement.TripmineCooldownC = 2.5f;

        hero.CooldownPotionDisableTrigger();

        Destroy(gameObject);
    }
}
