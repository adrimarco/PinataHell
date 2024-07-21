using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    public float health = 10.0f;

    public UnityEvent onDamaged;
    public UnityEvent onDead;

    public bool Damage(float damage)
    {
        health -= damage;

        onDamaged.Invoke();

        if (health <= 0)
        {
            onDead.Invoke();

            return true;
        }
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
