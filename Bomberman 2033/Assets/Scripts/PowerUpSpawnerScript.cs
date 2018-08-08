using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawnerScript : MonoBehaviour {

    public GameObject catsEyePowerUpPrefab;
    public GameObject cooldownPowerUpPrefab;
    public GameObject infiniteOxygenPowerUpPrefab;
    public GameObject slowMotionPowerUpPrefab;

    private List<GameObject> powerUps = new List<GameObject>();
    private List<Vector3> spawnPositions = new List<Vector3>();



    // Use this for initialization
    void Start () {
        powerUps.Add(catsEyePowerUpPrefab);
        powerUps.Add(cooldownPowerUpPrefab);
        powerUps.Add(infiniteOxygenPowerUpPrefab);
        powerUps.Add(slowMotionPowerUpPrefab);

        foreach (Transform child in transform)
        {
            spawnPositions.Add(child.transform.position);

        }

        InvokeRepeating("Spawning", 1f, 5f);
    }

    // Update is called once per frame
    void Update () {
		
	}

    void Spawning()
    {
        int i;
        i = Random.Range(0, powerUps.Count);

        int j;
        j = Random.Range(0, spawnPositions.Count);
        GameObject spawnedPowerUp = Instantiate(powerUps[i], spawnPositions[j], Quaternion.identity) as GameObject;

        spawnPositions.Remove(spawnPositions[j]);

        StartCoroutine(DestroyPowerUp(spawnedPowerUp, 10f));

    }

    IEnumerator DestroyPowerUp(GameObject spawnedPowerUp, float delay)
    {
        yield return new WaitForSeconds(delay);

        spawnPositions.Add(spawnedPowerUp.transform.position);
        Destroy(spawnedPowerUp);

    }
}
