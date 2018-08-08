using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWork : Photon.PunBehaviour {

    private Camera[] cams = new Camera[2];

    // Use this for initialization
    void Start () {
        cams[0] = GameObject.Find("MetroCam").GetComponent<Camera>();
        cams[0].enabled = true;

        cams[1] = GameObject.Find("WastelandCam").GetComponent<Camera>();
        cams[1].enabled = false;
    }
	
	// Update is called once per frame
	void Update () {

        if (photonView.isMine == false && PhotonNetwork.connected == true)
        {
            return;
        }
        MoveCamera();

    }

    void MoveCamera()
    {
        cams[0] = GameObject.Find("MetroCam").GetComponent<Camera>();
        cams[1] = GameObject.Find("WastelandCam").GetComponent<Camera>();



        float clampedX = Mathf.Clamp(transform.position.x, 5.85f, 19.15f);
        float clampedZ = Mathf.Clamp(transform.position.z, 3.5f, 21.5f);

        if (Movement.LocalPlayerInstance.GetComponent<Movement>().wasteland)
        {
            cams[1].transform.position = new Vector3(clampedX, transform.position.y + 15, clampedZ);

        }
        else
        {
            cams[0].transform.position = new Vector3(clampedX, transform.position.y + 9.5f, clampedZ);

        }
    }

    public void Wasteland()
    {
        Movement.LocalPlayerInstance.GetComponent<Movement>().wasteland = !Movement.LocalPlayerInstance.GetComponent<Movement>().wasteland;
        if (cams[0].enabled)
        {
            cams[1].enabled = true;

            cams[0].enabled = false;
        }
        else
        {
            cams[0].enabled = true;

            cams[1].enabled = false;
        }


    }
}
