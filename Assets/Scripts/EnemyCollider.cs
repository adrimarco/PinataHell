using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollider : MonoBehaviour
{
    public Enemy enemy = null;
    private Rigidbody rb;
    private Animation anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animation>();
    }

    public void Damage(float damage)
    {
        if (enemy == null) return;

        bool isDead = enemy.Damage(damage);

        if (isDead)
        {
            OnEnemyDead();
        }
        else
        {
            anim.Play();
        }
    }

    public void OnEnemyDead()
    {
        enemy = null;

        EnemyLoot.Instance.GenerateDeadEnemy(transform);
        EnemyLoot.Instance.SpawnRandomReward(transform.position);
        Destroy(transform.parent.gameObject);
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
