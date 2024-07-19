using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRecover : MonoBehaviour
{
    [Range(0f, 1f)]
    public float health = 0.3f;

    private void OnTriggerStay(Collider other)
    {
        Player player;
        if (other.gameObject.TryGetComponent<Player>(out player))
        {
            if (!player.IsFullyHealed())
            {
                player.Heal(health * player.maxHealth);
                Destroy(gameObject);
            }
        }
    }
}
