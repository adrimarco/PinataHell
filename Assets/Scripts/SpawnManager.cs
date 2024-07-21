using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public float maxSpawnDistanceFromPlayer = 2500f;
    [SerializeField] int maxTryPoints = 10;
    [SerializeField] GameObject spawnPrefab;


    float spawnTime = 0.0f;
    [SerializeField] float maxSpawnTime = 10.0f;

    // Simultaneous enemies counter
    int enemiesCount = 0;
    int maxEnemiesCount = 20;
    // Enemies that appear at each round counter
    int roundEnemiesCount = 0;
    int maxRoundEnemiesCount = 20;

    Transform playerTransform;
    List<Transform> spawnPoints;
    List<float> spawnPointsDistances = new List<float>();
    
    float generateNewDistancesTime = 0.0f;
    [SerializeField] float maxGenerateNewDistancesTime = 3.0f;

    // Rounds stats
    [Space(4)]
    [Header("Round progression")]
    public short initialEnemies = 4;
    public short enemiesIncrementPerRound = 4;
    public float forceNewRoundTime = 20;
    [Space(2)]
    public float baseEnemyHealth = 5;
    public float enemyHealthIncrementPerRound = 2;
    [Space(2)]
    public float baseEnemyDamage = 10;
    public float enemyDamageIncrementPerRound = 3;

    private int round = 1;
    private float enemyDamage = 1;
    private float enemyHealth = 1;
    private float newRoundTimer = 20;



    // Start is called before the first frame update
    void Start()
    {
        round = 1;
        maxRoundEnemiesCount = initialEnemies;
        enemyDamage = baseEnemyDamage;
        enemyHealth = baseEnemyHealth;

        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        spawnPoints = new List<Transform>(GameObject.Find("SpawnPointsContainer").GetComponentsInChildren<Transform>());
        spawnPoints.RemoveAt(0);

        for (int i = 0; i < spawnPoints.Count; i++)
        {
            Vector3 pos = spawnPoints[i].position;
            spawnPointsDistances.Add(Mathf.Pow(pos.x - playerTransform.position.x, 2) + Mathf.Pow(pos.y - playerTransform.position.y, 2) + Mathf.Pow(pos.z - playerTransform.position.z, 2));
        }

        Enemy.onEnemyDead.AddListener(OnEnemyDeath);
    }

    // Update is called once per frame
    void Update()
    {
        if (generateNewDistancesTime > 0.0f) generateNewDistancesTime -= Time.deltaTime;

        // When all enemies are dead or have past some time since last spawn, start new round
        if (roundEnemiesCount >= maxRoundEnemiesCount)
        {
            newRoundTimer -= Time.deltaTime;
            if (newRoundTimer <= 0) NewRound();
        }

        if (spawnTime > 0.0f)
            spawnTime -= Time.deltaTime;
        else if (enemiesCount < maxEnemiesCount && roundEnemiesCount < maxRoundEnemiesCount)
        {
            spawnEnemyOnRandomSpawnPoint();
        }
    }


    void spawnEnemyOnRandomSpawnPoint()
    {
        GameObject newEnemy = Instantiate(spawnPrefab, GetRandomSpawnPosition(), Quaternion.identity);

        Enemy e = newEnemy.GetComponentInChildren<Enemy>();
        if (e != null)
        {
            e.healthComp.health = enemyHealth;
            e.attackDamage = enemyDamage;
        }

        spawnTime = maxSpawnTime;
        enemiesCount++;
        roundEnemiesCount++;

        // When all enemies of the round have spawned, start the timer for the next round
        if (roundEnemiesCount >= maxRoundEnemiesCount) newRoundTimer = forceNewRoundTime;
    }

    private Vector3 GetRandomSpawnPosition()
    {
        if (generateNewDistancesTime <= 0.0f)
        {
            sortSpawnPointsByDistance();

            generateNewDistancesTime = maxGenerateNewDistancesTime;
        }

        List<Transform> poolSpawnPoints = new List<Transform>();
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            if (Math.Abs(spawnPointsDistances[i]) < maxSpawnDistanceFromPlayer)
            {
                poolSpawnPoints.Add(spawnPoints[i]);
            }
            else
            {
                break;
            }
        }

        if (poolSpawnPoints.Count > 0)
        {
            bool positionFound = false;
            for (int i = 0; i < maxTryPoints; i++)
            {
                int randomPos = UnityEngine.Random.Range(0, poolSpawnPoints.Count - 1);
                Ray lineOfSight = new Ray(poolSpawnPoints[randomPos].position, poolSpawnPoints[randomPos].position - playerTransform.position);
                RaycastHit hit;
                if (Physics.Raycast(lineOfSight, out hit))
                {
                    if (hit.collider != null)
                    {
                        return poolSpawnPoints[randomPos].position;
                    }
                }
            }

            // In case that the enemy was not spawned in maxTryPoints trys, spawn the enemy at the closest spawnpoint
            if (!positionFound)
            {
                return poolSpawnPoints[0].position;
            }
        }

        return Vector3.zero;
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
                float distance = Mathf.Pow(spawnPosition.x - playerPosition.x, 2) + Mathf.Pow(spawnPosition.y - playerPosition.y, 2) + Mathf.Pow(spawnPosition.z - playerPosition.z, 2);
                float nextDistance = Mathf.Pow(nextSpawnPosition.x - playerPosition.x, 2) + Mathf.Pow(nextSpawnPosition.y - playerPosition.y, 2) + Mathf.Pow(nextSpawnPosition.z - playerPosition.z, 2);

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

        // If all enemies are dead, start the next round in a short time
        if (enemiesCount == 0 && roundEnemiesCount >= maxRoundEnemiesCount)
        {
            newRoundTimer = 2;
        }
    }

    private void NewRound()
    {
        round += 1;
        enemyHealth += enemyHealthIncrementPerRound;
        enemyDamage += enemyDamageIncrementPerRound;
        roundEnemiesCount = 0;
        maxRoundEnemiesCount += enemiesIncrementPerRound;
    }
}
