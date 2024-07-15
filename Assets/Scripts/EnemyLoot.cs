using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLoot : MonoBehaviour
{
    public static EnemyLoot Instance {  get; private set; }

    public List<GameObject> skillsPickups;
    public GameObject lifePickup;
    public GameObject bomb;
    
    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null) return;

        Instance = this;
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public void SpawnRandomReward(Vector3 spawnLocation)
    {
        int randomNumber = Random.Range(0, 1000);

        if (randomNumber < 500) 
        {
            //GetCandies();
        }
        else if (randomNumber < 550)
        {
            //SpawnBomb();
        }
        else if (randomNumber < 750)
        {
             //SpawnLife();
        }
        else
        {
            SpawnSkill(spawnLocation);
        }
    }

    public void SpawnSkill(Vector3 spawnLocation)
    {
        if (skillsPickups.Count == 0) return;

        int randomIndex = Random.Range(0, skillsPickups.Count);
        
        Instantiate(skillsPickups[randomIndex], spawnLocation, Quaternion.identity);
    }
}
