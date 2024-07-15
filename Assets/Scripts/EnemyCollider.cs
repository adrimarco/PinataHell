using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollider : MonoBehaviour
{
    public Enemy enemy = null;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Damage(float damage)
    {
        if (enemy == null) return;

        bool isDead = enemy.Damage(damage);

        if (isDead) OnEnemyDead();
    }

    public void OnEnemyDead()
    {
        Ragdoll();
        
        enemy = null;
    }

    public void Ragdoll()
    {
        EnemyLoot.Instance.GenerateDeadEnemy(transform);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (enemy != null) return;

        if (collision.transform.position.y > transform.position.y + 0.5) return;

        // Calculate push direction
        Vector3 push = transform.position - collision.transform.position;
        push.y = 0;

        rb.AddForce(push.normalized * 5);
    }
}
