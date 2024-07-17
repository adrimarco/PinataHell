using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    Transform aiTarget;
    NavMeshAgent navAgent;

    // Enemy stats
    public float health = 10.0f;
    public float attackDamage = 10.0f;
    public int candies = 10;
    private float stunTime = 0.0f;

    public static UnityEvent<int> onEnemyDead = new UnityEvent<int>();

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
            onEnemyDead.Invoke(candies);

            return true;
        }

        // Apply stun to enemy
        ApplyStun(1.5f);

        return false;
    }

    public void ApplyStun(float time)
    {
        stunTime = Mathf.Max(stunTime, time);
        navAgent.SetDestination(navAgent.transform.position);
    }

    public bool IsStunned()
    {
        return stunTime > 0f;
    }
}
