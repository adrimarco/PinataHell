using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Enemy enemy = null;

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

    private void FixedUpdate()
    {
        if (enemy != null && Vector3.Distance(enemy.transform.position, transform.position) > 5)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.position = enemy.transform.position;
            rb.velocity = Vector3.zero;
        }
    }
}
