using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float explosionRadius = 2f;
    public float explosionTime = 5f;
    private bool exploded = false;

    MeshRenderer mesh = null;
    AudioSource sound = null;
    ParticleSystem particles = null;

    private void Start()
    {
        mesh = GetComponentInChildren<MeshRenderer>();
        sound = GetComponent<AudioSource>();
        particles = GetComponent<ParticleSystem>();
    }
    // Update is called once per frame
    void Update()
    {
        if (explosionTime <= 0)
        {
            Detonate();
        }
        else
        {
            explosionTime -= Time.deltaTime;
        }
    }

    public void Detonate()
    {
        if (exploded) return;

        exploded = true;

        List<EnemyHealth> enemiesHit = new List<EnemyHealth>();
        bool playerHit = false;
        Collider[] collidersHit = Physics.OverlapSphere(transform.position, explosionRadius);
        Player player = Player.Instance;

        foreach (Collider c in collidersHit)
        {
            if (c.isTrigger) continue;

            EnemyCollider enemy;
            if (c.gameObject == player.gameObject && !playerHit)
            {
                playerHit = true;
                player.Damage(0.25f * player.maxHealth);
            }
            else if (c.TryGetComponent<EnemyCollider>(out enemy) && !enemiesHit.Contains(enemy.enemyHealth))
            {
                enemiesHit.Add(enemy.enemyHealth);
                enemy.Damage(player.damageComp.weaponDamage * 2f);
            }
            Rigidbody rb;
            if (c.TryGetComponent<Rigidbody>(out rb))
            {
                Vector3 direction = (c.transform.position - transform.position).normalized;
                rb.AddForce(direction * 5, ForceMode.Impulse);
            }
        }


        if (mesh != null) mesh.enabled = false;
        if (sound != null) sound.Play();
        if (particles != null) particles.Play();
    }
}
