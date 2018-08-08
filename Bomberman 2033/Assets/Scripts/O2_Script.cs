using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class O2_Script : MonoBehaviour {

    public static Image O2Bar;
    public static bool infiniteOxygenOnceBool = true;

	// Use this for initialization
	void Start () {
        O2Bar = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Movement.LocalPlayerInstance && Movement.LocalPlayerInstance.GetComponent<Movement>().wasteland)
        {
            if (Movement.infiniteOxygen && infiniteOxygenOnceBool)
            {
                Invoke("FixingOxygen", 10f);
                infiniteOxygenOnceBool = false;
            }
            else if (!Movement.infiniteOxygen)
            {
                O2Bar.fillAmount -= 0.1666667f * Time.deltaTime;
            }

        }
        else
        {
            O2Bar.fillAmount += 0.08f * Time.deltaTime;

        }

        if (O2Bar.fillAmount == 0)
        {
            Movement.LocalPlayerInstance.GetComponent<Movement>().destroySpriteInt = true;
        }

    }

    void FixingOxygen()
    {
        Movement.infiniteOxygen = false;
    }
}
