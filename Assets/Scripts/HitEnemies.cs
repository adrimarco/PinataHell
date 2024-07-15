using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEnemies : MonoBehaviour
{
    private Animator animator;
    private Camera cam;
    private bool attacking = false;
    private CapsuleCollider batCollider;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        animator = GetComponent<Animator>();
        batCollider = GetComponent<CapsuleCollider>();
        batCollider.enabled = false;
    }

    public void Attack()
    {
        if (!attacking) {
            attacking = true;
            animator.Play("BatHit");
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!attacking) return;

        if (other.attachedRigidbody != null)
        {
            Vector3 hitDirection = other.transform.position - transform.position;
            hitDirection.y = Mathf.Max(hitDirection.y, 0);

            other.attachedRigidbody.AddForce(hitDirection.normalized * 10, ForceMode.Impulse);
        }

        EnemyCollider enemyCollider;
        if (other.gameObject.TryGetComponent<EnemyCollider>(out enemyCollider))
        {
            enemyCollider.enemy.Damage(5);
        }
    }

    public void EnableBatCollision()
    {
        batCollider.enabled = true;
    }

    public void DisableBatCollision()
    {
        batCollider.enabled = false;
    }

    public void AttackFinished()
    {
        attacking = false;
    }
}
