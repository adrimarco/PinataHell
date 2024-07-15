using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRecover : MonoBehaviour
{
    public float health = 30.0f;

    private void OnTriggerStay(Collider other)
    {
        Player player;
        if (other.gameObject.TryGetComponent<Player>(out player))
        {
            if (!player.IsFullyHealed())
            {
                player.Heal(health);
                Destroy(gameObject);
            }
        }
    }
}
