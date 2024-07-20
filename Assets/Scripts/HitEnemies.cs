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
    public float weaponDamage = 5f;

    // List of enemies hit in the last attack
    private List<EnemyHealth> enemiesHit = new List<EnemyHealth>();

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

            other.attachedRigidbody.AddForce(hitDirection.normalized * 150, ForceMode.Acceleration);
        }

        EnemyCollider enemyCollider;
        if (other.gameObject.TryGetComponent<EnemyCollider>(out enemyCollider) && !enemiesHit.Contains(enemyCollider.enemyHealth))
        {
            enemyCollider.Damage(weaponDamage);
            enemiesHit.Add(enemyCollider.enemyHealth);
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
        enemiesHit.Clear();
    }
}
