using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : Photon.PunBehaviour
{

    public float speed = 1f;
    private GameObject panel;
    public GameObject teleportationFlask;
    public GameObject tripmine;
    public GameObject melee;
    private LaserScript laser;
    private static bool tpFlaskReady = true;
    private static bool tripmineReady = true;
    private bool InvisibilityReady = true;
    public Vector3 pos;
    public bool invisibility = false;
    public static int skill_Count;
    public static float survive_Time;
    public static bool catseye = false;
    public static bool infiniteOxygen = false;
    private CameraWork cameraWork;
    public static Movement instance;
    private bool destroySeq = false;
    public bool destroySpriteInt = false;
    public bool ready = false;
    private bool readyInt = true;
    public bool skillsActivated = false;
    public bool gameModePVE = true;
    private GameObject personalLight;

    public static float invisibilityCooldownC = 10;
    public static float TripmineCooldownC = 5;
    public static float TpFlaskCooldownC = 5;

    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;


    private Animator cooldowwnAnimator;



    public bool wasteland = false;
    public static Vector3 direction;

    private void Awake()
    {
        // #Important
        // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
        if (photonView.isMine)
        {
            LocalPlayerInstance = gameObject;
        }
        // #Critical
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {


        cameraWork = GetComponent<CameraWork>();

        panel = FindObjectOfType<Canvas>().transform.Find("GameOver Panel").gameObject;

        melee = transform.Find("Melee").gameObject;

        direction = new Vector3(0, 0, 1);
        //cooldowwnAnimator = GameObject.Find("SkillCooldowns").GetComponent<Animator>();
        cooldowwnAnimator = FindObjectOfType<Canvas>().transform.Find("Skill Panel").transform.Find("SkillCooldowns").GetComponent<Animator>();

        if (FindObjectOfType<PVPmode>())
        {
            gameModePVE = false;
            cooldowwnAnimator.speed = 2f;

        }
        personalLight = transform.Find("Point light").gameObject;

        transform.Rotate(90, 0, 0);

    }

    // Update is called once per frame
    void Update () {

        if (photonView.isMine == false && PhotonNetwork.connected == true)
        {
            return;
        }

        if (readyInt && Input.GetKeyDown("r"))
        {
            photonView.RPC("GetReady", PhotonTargets.AllBuffered, null);
        }

        panel = FindObjectOfType<Canvas>().transform.Find("GameOver Panel").gameObject;

        cooldowwnAnimator = FindObjectOfType<Canvas>().transform.Find("Skill Panel").transform.Find("SkillCooldowns").GetComponent<Animator>();


        if (destroySpriteInt)
        {
            //Destroy(GetComponent<SpriteRenderer>());
            photonView.RPC("DestroySprite", PhotonTargets.AllBuffered, null);

            destroySpriteInt = false;
        }

        if (!GetComponent<SpriteRenderer>() && destroySeq == false)
        {
            Invoke("Death", 1f);
            destroySeq = true;
        }

        //Rotasyon kontrolü

        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (input != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(input);

        }

        transform.rotation = Quaternion.Euler(90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);



        survive_Time = Time.timeSinceLevelLoad;
        pos = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z));
        Move();
        if (!wasteland)
        {
            transform.position = new Vector3(transform.position.x, 1, transform.position.z);

        }
        else if (wasteland)
        {
            transform.position = new Vector3(transform.position.x, 12, transform.position.z);

        }
        if (skillsActivated)
        {
            Actions();

        }
        else if (!gameModePVE)
        {
            Actions();

        }

        Debug.DrawRay(transform.position + direction * 0.55f, direction * 0.1f);
    }





    void Move()
    {
        if (Input.GetKey("w"))
        {
            direction = new Vector3(0, 0, 1);
            GetComponent<CharacterController>().SimpleMove(direction * speed);

        }
        if (Input.GetKey("a"))
        {
            direction = new Vector3(-1, 0, 0);
            GetComponent<CharacterController>().SimpleMove(direction * speed);
        }
        if (Input.GetKey("s"))
        {
            direction = new Vector3(0, 0, -1);
            GetComponent<CharacterController>().SimpleMove(direction * speed);

        }
        if (Input.GetKey("d"))
        {
            direction = new Vector3(1, 0, 0);
            GetComponent<CharacterController>().SimpleMove(direction * speed);

        }

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, 1f, 24f), transform.position.y, Mathf.Clamp(transform.position.z, 1f, 24f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.isMine == false && PhotonNetwork.connected == true)
        {
            return;
        }

        if (other.gameObject.tag == "Snake")
        {
            Death();
            //GameManager.instance.LeaveRoom();

        }
        if (other.tag == "Portal")
        {
            transform.position = GameObject.FindGameObjectsWithTag("Portal")[1].transform.position + new Vector3(0,0, 0.2f);

        }
    }

    void Actions()
    {
        if (Input.GetKeyDown("e"))
        {
            Ladder[] ladders = FindObjectsOfType<Ladder>();
            float difference;
            foreach (Ladder ladder in ladders)
            {
                difference = (ladder.transform.position - transform.position).magnitude;
                if (difference < 0.75f)
                {
                    if (wasteland)
                    {
                        GetComponent<Animator>().Play("ClimbDown");

                    }
                    else
                    {
                        GetComponent<Animator>().Play("Climb");

                    }
                }
            }
        }
        else if (Input.GetKeyDown("f") && tpFlaskReady)
        {
            skill_Count += 1;
            ThrowTeleportationFlask();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && tripmineReady)
        {
            if (Physics.Raycast(transform.position, direction,   0.3f))
            {
                skill_Count += 1;
                PlaceTripmine();

            }
        }
        else if (Input.GetKeyDown("q") && InvisibilityReady)
        {
            skill_Count += 1;
            Beinvisible();
            photonView.RPC("ChangeTarget", PhotonTargets.MasterClient, 0);
        }
        else if (Input.GetKeyDown("r"))
        {
            photonView.RPC("MeleeHit", PhotonTargets.AllBuffered, null);
        }
    }

    [PunRPC]
    void GetReady()
    {       
            ready = true;
            readyInt = false;
    }

    [PunRPC]
    void MeleeHit()
    {
        melee.SetActive(true);

    }

    void ClimbUpLadder()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + 11, transform.position.z);
        cameraWork.Wasteland();
    }

    void ClimbDownLadder()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - 11, transform.position.z);
        cameraWork.Wasteland();

    }

    void ThrowTeleportationFlask()
    {
        GameObject tpFlask = PhotonNetwork.Instantiate(teleportationFlask.name, transform.position, Quaternion.identity, 0) as GameObject;
        tpFlaskReady = false;
        Invoke("TpFlaskCooldown", TpFlaskCooldownC);
        cooldowwnAnimator.Play("TpFlaskCooldown", cooldowwnAnimator.GetLayerIndex("TpFlaskLayer"));

        tpFlask.GetComponent<Rigidbody>().velocity = direction * 7f;
    }

    void TpFlaskCooldown()
    {
        tpFlaskReady = true;
    }

    void PlaceTripmine()
    {
        Vector3 minePosition = transform.position + (direction * 0.25f);
        GameObject tripmine0;

        if (PhotonNetwork.connected)
        {
            tripmine0 = PhotonNetwork.Instantiate(tripmine.name, minePosition, Quaternion.identity, 0) as GameObject;

        }
        //Offline deneme amaçlı serversız instantiate

        else
        {
            tripmine0 = Instantiate(tripmine, minePosition, Quaternion.identity) as GameObject;

        }


        if (direction == new Vector3(0,0,-1))
        {
            tripmine0.transform.Rotate(0, 180, 0);
        }
        else if (direction == new Vector3(-1, 0, 0))
        {
            tripmine0.transform.Rotate(0, 270, 0);
        }
        else if(direction == new Vector3(1, 0, 0))
        {
            tripmine0.transform.Rotate(0, 90, 0);
        }

        tripmineReady = false;
        Invoke("TripmineCooldown", TripmineCooldownC);
        cooldowwnAnimator.Play("TripmineCooldown", cooldowwnAnimator.GetLayerIndex("TripmineLayer"));
    }


    void TripmineCooldown()
    {
        tripmineReady = true;
    }

    void Beinvisible()
    {
        Color invisibilityColor = GetComponent<SpriteRenderer>().color;
        invisibilityColor.a = 0.5f;
        GetComponent<SpriteRenderer>().color = invisibilityColor;
        invisibility = true;
        InvisibilityReady = false;
        if (!gameModePVE)
        {
            Debug.Log("true invisibility casted");

            photonView.RPC("TrueInvisibility", PhotonTargets.OthersBuffered);
        }

        cooldowwnAnimator.Play("InvisibilityCooldown", cooldowwnAnimator.GetLayerIndex("InvisibilityLayer"));


        Invoke("DisableInvisibility", 3f);

        Invoke("InvisibilityCooldown", invisibilityCooldownC);
    }

    [PunRPC]
    void TrueInvisibility()
    {
        Color invisibilityColor = GetComponent<SpriteRenderer>().color;
        invisibilityColor.a = 0f;
        GetComponent<SpriteRenderer>().color = invisibilityColor;
        Debug.Log("true invisibility activated");
        personalLight.SetActive(false);
    }

    void DisableInvisibility()
    {
        Color invisibilityColor = GetComponent<SpriteRenderer>().color;
        invisibilityColor.a = 1f;
        GetComponent<SpriteRenderer>().color = invisibilityColor;
        invisibility = false;

        if (!gameModePVE)
        {

            photonView.RPC("DisableTrueInvisibility", PhotonTargets.OthersBuffered, null);
        }
    }

    [PunRPC]
    void DisableTrueInvisibility()
    {
        Color invisibilityColor = GetComponent<SpriteRenderer>().color;
        invisibilityColor.a = 1f;
        GetComponent<SpriteRenderer>().color = invisibilityColor;
        personalLight.SetActive(true);
    }

    void InvisibilityCooldown()
    {
        InvisibilityReady = true;
    }

    public static void CatseyeEnd()
    {
        catseye = false;
    }

    public void CooldownPotionDisableTrigger()
    {
        Invoke("CooldownPotionDisable", 10f);
    }

    void CooldownPotionDisable()
    {
        invisibilityCooldownC = 10f;
        TpFlaskCooldownC = 5f;
        TripmineCooldownC = 5f;
    }

    [PunRPC]
    void DestroySprite()
    {
        Destroy(GetComponent<SpriteRenderer>());
    }

    public void Death()
    {

        GameObject.Find("Canvas").transform.Find("GameOver Panel").gameObject.SetActive(true);
        PhotonNetwork.Destroy(gameObject);
        photonView.RPC("ChangeTarget", PhotonTargets.MasterClient, gameObject.GetPhotonView().viewID);
    }

    [PunRPC]
    void ChangeTarget(int deadHeroView)
    {
        Pathfinding.ChoosingHeroToChase(deadHeroView);
        Debug.Log("hedef değişti");
    }
}
