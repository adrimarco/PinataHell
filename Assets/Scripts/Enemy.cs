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
    public EnemyHealth healthComp;

    // Enemy stats
    public float attackDamage = 10.0f;
    public int candies = 10;
    private float stunTime = 0.0f;

    public static UnityEvent<int> onEnemyDead = new UnityEvent<int>();

    // Start is called before the first frame update
    void Start()
    {
        aiTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        navAgent = GetComponent<NavMeshAgent>();
        healthComp = GetComponent<EnemyHealth>();
        healthComp.onDamaged.AddListener(OnDamaged);
        healthComp.onDead.AddListener(OnDead);
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

    
    public void OnDamaged()
    {
        ApplyStun(1.5f);
    }

    public void OnDead()
    {
        Destroy(gameObject);
        onEnemyDead.Invoke(candies);
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
