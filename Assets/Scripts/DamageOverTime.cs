using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTime : MonoBehaviour
{

    float timeBetweenHits = 1.0f;
    float timeHit = 0.0f;
    float damagePerSecond = 0.0f;

    public void DamageTime(int damage)
    {
        damagePerSecond = damage;
    }

    public void ResetTimer()
    {
        timeHit = timeBetweenHits;
    }

    private void Update()
    {
        if (timeHit > 0.0f)
        {
            timeHit -= Time.deltaTime;
        }
        else
        {
            if (damagePerSecond > 0.0f)
            {
                transform.gameObject.GetComponent<Player>().Damage(damagePerSecond);
            }
            timeHit = timeBetweenHits;
        }
    }
}
