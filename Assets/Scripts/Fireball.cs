using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    public float damage;

    private Rigidbody rb;
    private List<EnemyHealth> enemiesHit = new List<EnemyHealth>();

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        direction.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        EnemyCollider enemy;
        if (other.TryGetComponent<EnemyCollider>(out enemy))
        {
            if (!enemiesHit.Contains(enemy.enemyHealth))
            {
                enemiesHit.Add(enemy.enemyHealth);
                enemy.Damage(damage);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
