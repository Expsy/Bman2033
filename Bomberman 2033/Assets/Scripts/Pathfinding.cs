using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pathfinding : Photon.PunBehaviour {

    public static float headSpeed;
    public static IDictionary<Vector3, bool> walkablePositions = new Dictionary<Vector3, bool>();
    IDictionary<Vector3, Vector3> nodeParents = new Dictionary<Vector3, Vector3>();
    public IList<Vector3> nodes = new List<Vector3>();
    IList<Vector3> path;
    public Vector3 previousNode;
    public GameObject ball;
    public GameObject food;
    public static int damage_To_Snake;
    public static int snake_Length = 1;


    private static Movement hero;
    public List<Vector3> possibleFoodPositions = new List<Vector3>();

    public bool moveCube = false;
    int i;

    // Use this for initialization
    void Start () {

        if (!PhotonNetwork.isMasterClient)
        {
            Destroy(GetComponent<Pathfinding>());
        }

        headSpeed = 6f;
        FindingNodes();
        ChoosingHeroToChase(0);
        MoveCube();

    }

    // Update is called once per frame
    void Update () {

        //Küpün hareket mekanizması
        if (moveCube)
        {
            float step = Time.deltaTime * headSpeed;


            transform.position = Vector3.MoveTowards(transform.position, path[i], step);
            transform.LookAt(path[i]);
            if (transform.position.Equals(path[i]) && i >= 0)
            {
                previousNode = transform.position;
                //previousNode = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z));
                i--;
            }

            if (i < 0)
            {

                moveCube = false;
                PhotonNetwork.Destroy(FindObjectOfType<Food>().gameObject);
                InstantiateBall();
                instantiateFood();
                MoveCube();




            }
        }
    }

    public static void ChoosingHeroToChase(int deadhero)
    {
        List<Movement> activePlayers = new List<Movement>();
        
        foreach (Movement hero in FindObjectsOfType<Movement>())
        {
            if (!activePlayers.Contains(hero))
            {
                activePlayers.Add(hero);
            }
        }
        if (deadhero !=0)
        {
            activePlayers.Remove(PhotonView.Find(deadhero).GetComponent<Movement>());

        }

        int i;
        i = Random.Range(0, activePlayers.Count);
        hero = activePlayers[i];
        //if (activePlayers[i].invisibility)
        //{
        //    return;
        //}
        //else
        //{
        //    hero = activePlayers[i];
        //}
    }



    //Küpü harekete geçiren metod
    public void MoveCube()
    {

            nodeParents.Clear();
            path = FindShortestPath();
            moveCube = true;




    }


    //Yürünebilecek nokların hepsini içeren listeyi oluşturma
    void FindingNodes()
    {
        for (int i = 1; i <= 24; i++)
        {
            for (int j = 1; j <= 24; j++)
            {
                nodes.Add(new Vector3(i, 1, j));
            }
        }

        foreach (Vector3 node in nodes)
        {
            if (!Physics.CheckSphere(node, 0.1f, LayerMask.GetMask("Envoirmental")))
            {
                walkablePositions.Add(node, true);

            }
        }
    }

    Vector3 BFS(Vector3 startPosition, Vector3 goalPosition)
    {
        Queue<Vector3> queue = new Queue<Vector3>();
        HashSet<Vector3> exploredNodes = new HashSet<Vector3>();
        queue.Enqueue(startPosition);

        while (queue.Count != 0)
        {
            Vector3 currentNode = queue.Dequeue();

            if (currentNode == goalPosition)
            {
                return currentNode;
            }
            IList<Vector3> nodes = GetWalkableNodes(currentNode);
            foreach (Vector3 node in nodes)
            {
                if (!exploredNodes.Contains(node))
                {
                    exploredNodes.Add(node);
                    nodeParents.Add(node, currentNode);
                    queue.Enqueue(node);

                }
            }
        }

        return startPosition;
    }

    IList<Vector3> GetWalkableNodes(Vector3 curr)
    {
        IList<Vector3> walkableNodes = new List<Vector3>();
        IList<Vector3> possibleNodes = new List<Vector3>() {
            new Vector3 (curr.x + 1, curr.y, curr.z),
            new Vector3 (curr.x - 1, curr.y, curr.z),
            new Vector3 (curr.x, curr.y, curr.z + 1),
            new Vector3 (curr.x, curr.y, curr.z - 1)
        };
        foreach (Vector3 node in possibleNodes)
        {
            if (CanMove(node))
            {
                walkableNodes.Add(node);
            }
        }
        return walkableNodes;
    }

    bool CanMove(Vector3 nextPosition)
    {
        return (walkablePositions.ContainsKey(nextPosition) ? walkablePositions[nextPosition] : false);
    }

    IList<Vector3> FindShortestPath()
    {

            IList<Vector3> path = new List<Vector3>();
            Vector3 goal = BFS(transform.position, FindObjectOfType<Food>().transform.position);
            if (goal == transform.position)
            {
                return null;
            }

            Vector3 curr = goal;
            while (curr != transform.position)
            {
                path.Add(curr);
                curr = nodeParents[curr];
            }
            i = path.Count - 1;

            return path;


    }

    void InstantiateBall()
    {
        GameObject ballx;
        ballx  = PhotonNetwork.Instantiate(ball.name, FindObjectOfType<ball>() ? FindObjectOfType<ball>().transform.position : transform.position, Quaternion.identity, 0) as GameObject;
        string name = "ball" + FindObjectsOfType<ball>().Length;
        ballx.name = name;
        ballx.layer = 8;
        ballx.transform.parent = GameObject.FindGameObjectWithTag("Snake").transform;

        snake_Length += 1;

    }

    void FindingFoodPositions(List<Vector3> NodesAroundHero)
    {
        foreach (KeyValuePair<Vector3, bool> node in walkablePositions)
        {
            for (int i = 0; i < 8; i++)
            {
                if (node.Key == NodesAroundHero[i])
                {
                    possibleFoodPositions.Add(node.Key);

                }
            }
        }
    }

    List<Vector3> FindingNodesAroundHero()
    {
        List<Vector3> nodes = new List<Vector3>();
        if (hero)
        {
            nodes.Add(new Vector3(Mathf.RoundToInt(hero.transform.position.x), 1, Mathf.RoundToInt(hero.transform.position.z)));
            nodes.Add(new Vector3(Mathf.RoundToInt(hero.transform.position.x + 1), 1, Mathf.RoundToInt(hero.transform.position.z)));
            nodes.Add(new Vector3(Mathf.RoundToInt(hero.transform.position.x + 2), 1, Mathf.RoundToInt(hero.transform.position.z)));
            nodes.Add(new Vector3(Mathf.RoundToInt(hero.transform.position.x - 1), 1, Mathf.RoundToInt(hero.transform.position.z)));
            nodes.Add(new Vector3(Mathf.RoundToInt(hero.transform.position.x - 2), 1, Mathf.RoundToInt(hero.transform.position.z)));
            nodes.Add(new Vector3(Mathf.RoundToInt(hero.transform.position.x), 1, Mathf.RoundToInt(hero.transform.position.z + 1)));
            nodes.Add(new Vector3(Mathf.RoundToInt(hero.transform.position.x), 1, Mathf.RoundToInt(hero.transform.position.z + 2)));
            nodes.Add(new Vector3(Mathf.RoundToInt(hero.transform.position.x), 1, Mathf.RoundToInt(hero.transform.position.z - 1)));
            nodes.Add(new Vector3(Mathf.RoundToInt(hero.transform.position.x), 1, Mathf.RoundToInt(hero.transform.position.z - 2)));
            nodes.Add(new Vector3(Mathf.RoundToInt(hero.transform.position.x + 1), 1, Mathf.RoundToInt(hero.transform.position.z + 1)));
            nodes.Add(new Vector3(Mathf.RoundToInt(hero.transform.position.x - 1), 1, Mathf.RoundToInt(hero.transform.position.z + 1)));
            nodes.Add(new Vector3(Mathf.RoundToInt(hero.transform.position.x + 1), 1, Mathf.RoundToInt(hero.transform.position.z - 1)));
            nodes.Add(new Vector3(Mathf.RoundToInt(hero.transform.position.x - 1), 1, Mathf.RoundToInt(hero.transform.position.z - 1)));
            nodes.Add(new Vector3(Mathf.RoundToInt(hero.transform.position.x), 1, Mathf.RoundToInt(hero.transform.position.z)));
        }





        return nodes;
    }

    public void instantiateFood()
    {
        possibleFoodPositions.Clear();
        FindingFoodPositions(FindingNodesAroundHero());
        possibleFoodPositions.Remove(transform.position);


        int i = Random.Range(0, possibleFoodPositions.Count);
        if(hero.invisibility || hero.wasteland)
        {
            RandomlyRoam();

        }
        else
        {
            PhotonNetwork.Instantiate(food.name, possibleFoodPositions[i], Quaternion.identity, 0);

        }

    }

    void RandomlyRoam()
    {
        List<Vector3> randomFoodPos = new List<Vector3>();


        foreach (KeyValuePair<Vector3, bool> pos in walkablePositions)
        {
            randomFoodPos.Add(pos.Key);
        }

        int j = Random.Range(0, randomFoodPos.Count);

        PhotonNetwork.Instantiate(food.name, randomFoodPos[j], Quaternion.identity, 0);
    }
}
