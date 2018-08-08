using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour {

    public Transform startPoint;
    public Vector3 endPoint;
    private LineRenderer laserLine;
    private RaycastHit hit1;
    private RaycastHit hit2;
    private Ray direction;

    private Vector3 explosionPos;
    private GameObject snake;

    private ParticleSystem[] particles;
    private bool singleExplosion = true;



    // Use this for initialization
    void Start () {
        Invoke("LineRendererMethods", 1);
        particles = GetComponentsInChildren<ParticleSystem>();
        direction = new Ray(transform.position, transform.forward* -1);
        snake = GameObject.Find("Snake(Clone)");
        explosionPos = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z));
}

// Update is called once per frame
    void Update () {

        snake = GameObject.Find("Snake(Clone)");


        if (Physics.Raycast(direction, out hit2, hit1.distance - 0.2f))
        {
            ExplosionDamage();
            Destroy(GetComponent<LineRenderer>());
            particles[0].Play();
            particles[1].Play();
            Invoke("SelfDestruction", 1);
        }
    }

    void LineRendererMethods()
    {
        startPoint = transform.Find("Start");
        Ray direction = new Ray(transform.position, transform.forward * -1);
        Physics.Raycast(direction, out hit1, 50f, LayerMask.GetMask("Envoirmental"));

        endPoint = hit1.point;
        laserLine = GetComponent<LineRenderer>();

        laserLine.SetPosition(0, startPoint.position);
        laserLine.SetPosition(1, endPoint);
        laserLine.startWidth = 0.1f;
    }

    void SelfDestruction()
    {
        PhotonNetwork.Destroy(gameObject);
    }

    void ExplosionDamage()
    {

        if (hit2.transform.gameObject.GetComponent<Movement>() && explosionPos == hit2.transform.gameObject.GetComponent<Movement>().pos)
        {
            hit2.transform.GetComponent<Movement>().destroySpriteInt = true;
            hit2.transform.GetComponent<Movement>().speed = 0.01f;

        }
        else
        {

            Debug.Log("missed");
            print(explosionPos);
            if (hit2.transform.gameObject.GetComponent<Movement>())
            {
                print(hit2.transform.gameObject.GetComponent<Movement>().pos);

            }
        }

        if (hit2.transform.gameObject.tag == "Snake")
        {

            foreach (Transform child in snake.transform)
            {
                if (explosionPos == child.transform.position && singleExplosion == true)
                {
                    PhotonNetwork.Destroy(snake.transform.GetChild(snake.transform.childCount - 1).GetComponent<ball>().gameObject);
                    PhotonNetwork.Destroy(snake.transform.GetChild(snake.transform.childCount - 2).GetComponent<ball>().gameObject);
                    PhotonNetwork.Destroy(snake.transform.GetChild(snake.transform.childCount - 3).GetComponent<ball>().gameObject);
                    singleExplosion = false;

                    Pathfinding.snake_Length -= 3;
                    Pathfinding.damage_To_Snake += 3;
                }
            }
        }
    }
   
}
