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

    private void OnCollisionStay(Collision collision)
    {
        if (enemy == null || enemy.IsStunned()) return;

        Player player;
        if (collision.gameObject.TryGetComponent<Player>(out player))
        {
            player.Damage(enemy.attackDamage);
            enemy.ApplyStun(1.0f);
        }
    }
}
