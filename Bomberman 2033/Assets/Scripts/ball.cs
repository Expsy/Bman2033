using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball : Photon.PunBehaviour {

    public static float ballSpeed;
    private Pathfinding head;
    public Vector3 previousNode;
    ball[] balls;
    private ball targetBall;
    private bool speedUpBool = false;
    
	// Use this for initialization
	void Start () {

        if (!PhotonNetwork.isMasterClient)
        {
            Destroy(GetComponent<ball>());
        }

        if (FindObjectsOfType<ball>().Length < 2)
        {

            ballSpeed = 7f;

        }

        head = FindObjectOfType<Pathfinding>();
        balls = FindObjectsOfType<ball>();
        if (FindObjectsOfType<ball>().Length > 1)
        {
            targetBall = balls[1];
            targetBall.previousNode = transform.position;
        }

    }
	
	// Update is called once per frame
	void Update () {



        // Burası baştan yazılacak
        if (head.moveCube && !targetBall)
        {
            float step = Time.deltaTime * ballSpeed;
            transform.position = Vector3.MoveTowards(transform.position, head.previousNode, step);
            if (transform.position == head.previousNode)
            {
                previousNode = transform.position;
            }
        }
        else if (head.moveCube && targetBall)
        {
            float step = Time.deltaTime * ballSpeed;
            transform.position = Vector3.MoveTowards(transform.position, targetBall.previousNode, step);
            if (transform.position == targetBall.previousNode)
            {
                previousNode = transform.position;

            }
        }

        if (ballSpeed == 2.2f && speedUpBool == false)
        {
            speedUpBool = true;
            Invoke("SpeedBackUp", 8f);
        }
    }

    public void PrintPath()
    {

    }

    public void invk()
    {
        InvokeRepeating("PrintPath", 0.5f, 0.5f);

    }

    void SpeedBackUp()
    {
        ballSpeed = 6.6f;
        Pathfinding.headSpeed = 6f;
        speedUpBool = false;
    }
}
