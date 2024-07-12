using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform aiTarget;
    NavMeshAgent navAgent;

    // Enemy stats
    private float health = 10.0f;
    private float stunTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
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

    public void Damage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
        } 
        else
        {
            stunTime = 1.5f;
            navAgent.SetDestination(navAgent.transform.position);
        }
    }
}
