using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVPmode : Photon.PunBehaviour
{

    private bool skillsActivated = false;
    private int readyCount = 0;
    private List<Movement> heroes = new List<Movement>();

    // Use this for initialization
    void Start()
    {
        PhotonNetwork.sendRate = 30;
        PhotonNetwork.sendRateOnSerialize = 30;

        Movement.invisibilityCooldownC = 6f;
        Movement.TpFlaskCooldownC = 3f;
        Movement.TripmineCooldownC = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        //Ready();
    }

    private void Ready()
    {
        print(readyCount);
        print(heroes.Count);

        if (!PhotonNetwork.isMasterClient)
        {
            return;
        }


        if (!skillsActivated)
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

            if (FindObjectOfType<Movement>() && readyCount == FindObjectsOfType<Movement>().Length)
            {
                photonView.RPC("ActivateSkills", PhotonTargets.AllBuffered, null);

            }


        }
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
