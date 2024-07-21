using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollider : MonoBehaviour
{
    public EnemyHealth enemyHealth = null;
    public MeshRenderer enemyMesh = null;
    private Rigidbody rb;
    private Animation anim;
    private AudioSource hitSound;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animation>();
        hitSound = GetComponent<AudioSource>();
    }

    public void Damage(float damage)
    {
        if (enemyHealth == null) return;

        bool isDead = enemyHealth.Damage(damage);

        if (isDead)
        {
            OnEnemyDead();
        }
        else
        {
            anim.Play();
            PlayHitSound();
        }
    }

    public void OnEnemyDead()
    {
        enemyHealth = null;

        EnemyLoot.Instance.GenerateDeadEnemy(transform, GetEnemyMaterial());
        EnemyLoot.Instance.SpawnRandomReward(transform.position);
        Destroy(transform.parent.gameObject);
    }

    private Material GetEnemyMaterial()
    {
        if (enemyMesh == null) return null;

        return enemyMesh.material;
    }

    private void PlayHitSound()
    {
        if (hitSound != null) hitSound.Play();
    }
}
