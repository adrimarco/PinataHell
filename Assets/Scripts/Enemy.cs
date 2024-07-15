using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    Transform aiTarget;
    NavMeshAgent navAgent;

    // Enemy stats
    public float health = 10.0f;
    public float attackDamage = 10.0f;
    private float stunTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        aiTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (aiTarget && navAgent)
        {
            if (stunTime > 0.0f)
            {
                stunTime -= Time.deltaTime;
            }
            else { 
                navAgent.SetDestination(aiTarget.position);
            }
        }
    }

    public bool Damage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
            SpawnManager.onEnemyDeath.Invoke();
            EnemyLoot.Instance.SpawnRandomReward(transform.position);

            return true;
        }

        // Apply stun to enemy
        ApplyStun(1.5f);
        navAgent.SetDestination(navAgent.transform.position);

        return false;
    }

    public void ApplyStun(float time)
    {
        stunTime = Mathf.Max(stunTime, time);
    }

    public bool IsStunned()
    {
        return stunTime > 0f;
    }
}
