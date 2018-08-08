using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Photon.PunBehaviour {

	// Use this for initialization


    public void Deactivate()
    {
        gameObject.SetActive(false);
        Physics.IgnoreCollision(GetComponent<Collider>(), transform.root.GetComponent<Collider>());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name != transform.root.gameObject.name && other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Movement>().destroySpriteInt = true;
        }
    }
}
