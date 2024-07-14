using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class SpawnManager : MonoBehaviour
{
    public float maxSpawnDistanceFromPlayer = 50f;
    public float minSpawnDistanceFromPlayer = 20f;
    [SerializeField] int maxTryPoints = 10;
    int pointsCount = 0;
    [SerializeField] GameObject spawnPrefab;


    float spawnTime = 0.0f;
    [SerializeField] float maxSpawnTime = 10.0f;

    float enemiesCount = 0;
    [SerializeField] int maxEnemiesCount = 20;


    //Events
    public static UnityEvent onEnemyDeath;
    // Start is called before the first frame update
    void Start()
    {
        if (onEnemyDeath == null)
            onEnemyDeath = new UnityEvent();

        onEnemyDeath.AddListener(OnEnemyDeath);
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnTime > 0.0f)
            spawnTime -= Time.fixedDeltaTime;
        else if(enemiesCount < maxEnemiesCount) spawnEnemyOnRandomPosition();
    }


    void spawnEnemyOnRandomPosition()
    {

        //Vector3 minDistance = new Vector3(1.0f, 1.0f, 1.0f) * minSpawnDistanceFromPlayer + transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        for (int i = 0; i < maxTryPoints; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * maxSpawnDistanceFromPlayer;
            randomDirection += transform.position;
            if (NavMesh.SamplePosition(randomDirection, out hit, maxSpawnDistanceFromPlayer, 1))
            {
                float pointDistance = Vector3.Distance(transform.position, hit.position);
                if (pointDistance > minSpawnDistanceFromPlayer) {
                    finalPosition = hit.position;
                    break;
                }
                if (Vector3.Distance(transform.position, hit.position) > Vector3.Distance(transform.position, finalPosition)) {
                    finalPosition = hit.position;
                }
                pointsCount++;
            }
        }

        Instantiate(spawnPrefab, finalPosition, Quaternion.identity);

        spawnTime = maxSpawnTime;
        enemiesCount++;
        pointsCount = 0;
    }

    private void OnEnemyDeath()
    {
        enemiesCount--;
    }
}
