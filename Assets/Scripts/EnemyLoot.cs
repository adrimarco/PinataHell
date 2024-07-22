using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLoot : MonoBehaviour
{
    public static EnemyLoot Instance {  get; private set; }

    public List<GameObject> skillsPickups;
    public GameObject lifePickup;
    public GameObject bomb;
    public List<GameObject> enemiesBodies;
    
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
            return;
        }
        else if (randomNumber < 550)
        {
            SpawnBomb(spawnLocation);
        }
        else if (randomNumber < 750)
        {
             SpawnLife(spawnLocation);
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

    public void SpawnBomb(Vector3 spawnLocation)
    {
        if (bomb == null) return;

        Instantiate(bomb, spawnLocation, Quaternion.identity);
    }

    public void GenerateDeadEnemy(Transform enemyTransform, Material mat = null)
    {
        if (enemiesBodies.Count == 0) return;

        int randomIndex = Random.Range(0, enemiesBodies.Count);

        GameObject body = Instantiate(enemiesBodies[randomIndex], enemyTransform.position, enemyTransform.rotation);

        OverrideMaterial om;
        if (mat != null && body.TryGetComponent<OverrideMaterial>(out om))
        {
            om.SetNewMaterial(mat);
        }
    }

    public void SpawnLife(Vector3 spawnLocation)
    {
        if (lifePickup != null)
            Instantiate(lifePickup, spawnLocation, Quaternion.identity);
    }
}
