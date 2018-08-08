using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WastelandUI : MonoBehaviour {

    private GameObject bother;
    private GameObject heroLight;
    

	// Use this for initialization
	void Start () {
        bother = GameObject.Find("Bother");
        heroLight = FindObjectOfType<Movement>().transform.GetComponentInChildren<Light>().gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        if (Movement.LocalPlayerInstance && Movement.LocalPlayerInstance.GetComponent<Movement>().wasteland)
        {
            bother.SetActive(true);
            heroLight.SetActive(false);
            RenderSettings.ambientIntensity = 0.80f;
        }
        else
        {
            bother.SetActive(false);
            if (Movement.LocalPlayerInstance)
            {
                heroLight.SetActive(true);

            }

            if (Movement.catseye)
            {
                RenderSettings.ambientIntensity -= 0.08f * Time.deltaTime;

                Invoke("CatseyeFade", 10);
            }
            else
            {
                RenderSettings.ambientIntensity = 0f;

            }

        }

    }

    void CatseyeFade()
    {
        Movement.catseye = false;
    }

}
