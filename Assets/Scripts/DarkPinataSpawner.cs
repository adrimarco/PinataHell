using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkPinataSpawner : MonoBehaviour
{
    public GameObject darkPinataPrefab = null;

    private List<GameObject> spawnPoints = new List<GameObject>();
    private GameObject lastSpawnPoint = null;

    // Start is called before the first frame update
    void Start()
    {
        GetSpawnPointsFromChildren();
    }

    
    public void SpawnDarkPinata()
    {
        GameObject spawnPoint = GetRandomSpawnPoint();
        GameObject darkPinata = Instantiate(darkPinataPrefab, spawnPoint.transform);
    }

    private void GetSpawnPointsFromChildren()
    {
        Transform spawnsParent = transform.Find("DarkPinataSpawnPoints");
        if (spawnsParent == null) return;

        spawnPoints.Clear();
        for (short i = 0; i < spawnsParent.childCount; i++)
        {
            spawnPoints.Add(spawnsParent.GetChild(i).gameObject);
        }
    }

    private GameObject GetRandomSpawnPoint()
    {
        if (spawnPoints.Count == 0) return null;

        if (spawnPoints.Count == 1) return spawnPoints[0];

        // Get a spawn point different from lastSpawnPoint
        GameObject randomSpawnPoint = null;
        do
        {
            int randomIndex = Random.Range(0, spawnPoints.Count);
            randomSpawnPoint = spawnPoints[randomIndex];
        } while (randomSpawnPoint == lastSpawnPoint);

        lastSpawnPoint = randomSpawnPoint;

        return randomSpawnPoint;
    }
}
