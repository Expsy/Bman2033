using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVEmode : Photon.PunBehaviour {

    private bool snakeActivated = false;
    private int readyCount = 0;
    private List<Movement> heroes = new List<Movement>();
    public GameObject snake;
    public GameObject food;
    private bool foodInt = false;
    private GameObject firstFood;
    private bool skillsActivated = false;


    // Use this for initialization
    void Start () {
        PhotonNetwork.sendRate = 30;
        PhotonNetwork.sendRateOnSerialize = 30;
	}
	
	// Update is called once per frame
	void Update () {

        if (!PhotonNetwork.isMasterClient)
        {
            return;
        }






        if (!snakeActivated)
        {
            foreach (Movement hero in FindObjectsOfType<Movement>())
            {
                if (!hero.ready)
                {
                    if (!heroes.Contains(hero))
                    {
                        heroes.Add(hero);

                    }
                }
            }
            for (int i = 0; i < heroes.Count; i++)
            {
                if (heroes[i].ready)
                {
                    readyCount += 1;

                    heroes.Remove(heroes[i]);
                }
            }

            if (FindObjectOfType<Movement>() && heroes.Count == 0)
            {
                Invoke("ActivateSnake", 2f);
                snakeActivated = true;
                photonView.RPC("ActivateSkills", PhotonTargets.AllBuffered, null);


                if (!foodInt)
                {
                    firstFood = PhotonNetwork.Instantiate(food.name, new Vector3(15, 1, 11), Quaternion.identity, 0) as GameObject;
                    foodInt = true;
                }

            }


        }
	}

    void ActivateSnake()
    {

        PhotonNetwork.Instantiate(snake.name, Vector3.zero, Quaternion.identity, 0);
        Debug.Log("snake is activated");
    }

    [PunRPC]
    void ActivateSkills()
    {

        foreach (Movement hero in FindObjectsOfType<Movement>())
        {
            hero.skillsActivated = true;
            skillsActivated = true;
        }
        Debug.Log("skills are activated");
    }



}
