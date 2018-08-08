using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPFlask : MonoBehaviour {

    public GameObject portal;
    public static int cooldown;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        PortalExitCheck(collision.gameObject);

        if (collision.gameObject.tag == "Wall")
        {
            PhotonNetwork.Destroy(gameObject);
            if (Movement.direction == new Vector3(0,0,1))
            {
                GameObject Portal0 = PhotonNetwork.Instantiate(portal.name, collision.gameObject.transform.position - new Vector3(0, 0, 0.6f), Quaternion.identity, 0) as GameObject;
                GameObject Portal1 = PhotonNetwork.Instantiate(portal.name, PortalExitCheck(collision.gameObject) - new Vector3(0, 0, 0.4f), Quaternion.identity, 0) as GameObject;
            }
            else if (Movement.direction == new Vector3(0, 0, -1))
            {
                GameObject Portal0 = PhotonNetwork.Instantiate(portal.name, collision.contacts[0].point - new Vector3(0, 0, -0.1f), Quaternion.identity, 0) as GameObject;
                GameObject Portal1 = PhotonNetwork.Instantiate(portal.name, PortalExitCheck(collision.gameObject) - new Vector3(0, 0, -0.4f), Quaternion.identity, 0) as GameObject;
            }
            else if (Movement.direction == new Vector3(1, 0, 0))
            {
                GameObject Portal0 = PhotonNetwork.Instantiate(portal.name, collision.contacts[0].point - new Vector3(0.1f, 0, 0), Quaternion.identity, 0) as GameObject;
                Portal0.transform.Rotate(new Vector3(0, 90, 0));
                GameObject Portal1 = PhotonNetwork.Instantiate(portal.name, PortalExitCheck(collision.gameObject) - new Vector3(0.4f, 0, 0), Quaternion.identity, 0) as GameObject;
                Portal1.transform.Rotate(new Vector3(0, 90, 0));
            }
            else if (Movement.direction == new Vector3(-1, 0, 0))
            {
                GameObject Portal0 = PhotonNetwork.Instantiate(portal.name, collision.contacts[0].point - new Vector3(-0.1f, 0, 0), Quaternion.identity, 0) as GameObject;
                Portal0.transform.Rotate(new Vector3(0, 90, 0));
                GameObject Portal1 = PhotonNetwork.Instantiate(portal.name, PortalExitCheck(collision.gameObject) - new Vector3(-0.4f, 0, 0), Quaternion.identity, 0) as GameObject;
                Portal1.transform.Rotate(new Vector3(0, 90, 0));
            }
        }
        else
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    Vector3 PortalExitCheck(GameObject wall)
    {
        for (int i = 0; ; i++)
        {
            if (Movement.direction == new Vector3(0, 0, 1))
            {
                if (!Physics.CheckSphere(wall.transform.position + new Vector3(0, 0, i), 0.1f))
                {
                    return wall.transform.position + new Vector3(0, 0, i);
                }
            }
            else if (Movement.direction == new Vector3(0, 0, -1))
            {
                if (!Physics.CheckSphere(wall.transform.position + new Vector3(0, 0, -i), 0.1f))
                {
                    return wall.transform.position + new Vector3(0, 0, -i);
                }
            }
            else if (Movement.direction == new Vector3(1, 0, 0))
            {
                if (!Physics.CheckSphere(wall.transform.position + new Vector3(i, 0, 0), 0.1f))
                {
                    return wall.transform.position + new Vector3(i, 0, 0);
                }
            }
            else if (Movement.direction == new Vector3(-1, 0, 0))
            {
                if (!Physics.CheckSphere(wall.transform.position + new Vector3(-i, 0, 0), 0.1f))
                {
                    return wall.transform.position + new Vector3(-i, 0, 0);
                }
            }
        }
    }
}
