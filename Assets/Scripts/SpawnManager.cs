using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public float maxSpawnDistanceFromPlayer = 2500f;
    [SerializeField] int maxTryPoints = 10;
    [SerializeField] GameObject spawnPrefab;


    float spawnTime = 0.0f;
    [SerializeField] float maxSpawnTime = 10.0f;

    int enemiesCount = 0;
    [SerializeField] int maxEnemiesCount = 20;

    Transform playerTransform;
    List<Transform> spawnPoints;
    List<float> spawnPointsDistances = new List<float>();
    
    float generateNewDistancesTime = 0.0f;
    [SerializeField] float maxGenerateNewDistancesTime = 3.0f;


    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        spawnPoints = new List<Transform>(GameObject.Find("SpawnPointsContainer").GetComponentsInChildren<Transform>());
        spawnPoints.RemoveAt(0);

        for (int i = 0; i < spawnPoints.Count; i++)
        {
            Vector3 pos = spawnPoints[i].position;
            spawnPointsDistances.Add(Mathf.Pow(pos.x + playerTransform.position.x, 2) - Mathf.Pow(pos.y + playerTransform.position.y, 2) - Mathf.Pow(pos.z + playerTransform.position.z, 2));
        }

        Enemy.onEnemyDead.AddListener(OnEnemyDeath);
    }

    // Update is called once per frame
    void Update()
    {
        if (generateNewDistancesTime > 0.0f) generateNewDistancesTime -= Time.fixedDeltaTime;

        if (spawnTime > 0.0f)
            spawnTime -= Time.fixedDeltaTime;
        else if(enemiesCount < maxEnemiesCount) spawnEnemyOnRandomSpawnPoint();
    }


    void spawnEnemyOnRandomSpawnPoint()
    {
        if (generateNewDistancesTime <= 0.0f)
        {
            sortSpawnPointsByDistance();

            generateNewDistancesTime = maxGenerateNewDistancesTime;
        }

        List<Transform> poolSpawnPoints = new List<Transform>();
        for (int i = 0; i < spawnPoints.Count; i++) {
            if (Math.Abs(spawnPointsDistances[i]) < maxSpawnDistanceFromPlayer)
            {
                poolSpawnPoints.Add(spawnPoints[i]);
            } else
            {
                break;
            }
        }

        if (poolSpawnPoints.Count > 0)
        {

            bool enemySpawned = false;
            for (int i = 0; i < maxTryPoints; i++)
            {
                int randomPos = UnityEngine.Random.Range(0, poolSpawnPoints.Count - 1);
                Ray lineOfSight = new Ray(poolSpawnPoints[randomPos].position, poolSpawnPoints[randomPos].position - playerTransform.position);
                RaycastHit hit;
                if (Physics.Raycast(lineOfSight, out hit))
                {
                    Debug.Log(hit.collider);
                    if (hit.collider != null)
                    {
                        Instantiate(spawnPrefab, poolSpawnPoints[randomPos].position, Quaternion.identity);

                        spawnTime = maxSpawnTime;
                        enemiesCount++;
                        enemySpawned = true;
                        break;
                    }
                }
            }

            // In case that the enemy was not spawned in maxTryPoints trys, spawn the enemy at the closest spawnpoint
            if (!enemySpawned)
            {
                Instantiate(spawnPrefab, poolSpawnPoints[0].position, Quaternion.identity);

                spawnTime = maxSpawnTime;
                enemiesCount++;
            }
        }
    }

    void sortSpawnPointsByDistance()
    {

        Vector3 playerPosition = playerTransform.position;

        bool swapped;
        for (int i = 0; i < spawnPoints.Count - 1; i++)
        {
           
            
            swapped = false;
            for (int j = 0; j < spawnPoints.Count - i - 1; j++)
            {
                
                Vector3 spawnPosition = spawnPoints[j].position;
                Vector3 nextSpawnPosition = spawnPoints[j+1].position;
                float distance = Mathf.Pow(spawnPosition.x + playerPosition.x, 2) - Mathf.Pow(spawnPosition.y + playerPosition.y, 2) - Mathf.Pow(spawnPosition.z + playerPosition.z, 2);
                float nextDistance = Mathf.Pow(nextSpawnPosition.x + playerPosition.x, 2) - Mathf.Pow(nextSpawnPosition.y + playerPosition.y, 2) - Mathf.Pow(nextSpawnPosition.z + playerPosition.z, 2);

                if (distance > nextDistance)
                {
                    Transform temp = spawnPoints[j];
                    spawnPoints[j] = spawnPoints[j + 1];
                    spawnPoints[j + 1] = temp;
                    swapped = true;
                }
            }

            if (swapped == false)
                break;
        }
        spawnPointsDistances.Sort();
    } 

    private void OnEnemyDeath(int _candies)
    {
        enemiesCount--;
    }
}
