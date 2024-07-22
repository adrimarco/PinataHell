using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToxicGasChild : MonoBehaviour
{

    public static UnityEvent<GameObject, bool> onPlayerEnterTrigger = new UnityEvent<GameObject, bool>();
    public bool doDamage = false;
    public bool playerInside = true;

    public void OnTriggerEnter(Collider other)
    {
        if(!doDamage)
        {
            Player player;
            if (other.gameObject.TryGetComponent<Player>(out player))
            {
                onPlayerEnterTrigger.Invoke(transform.gameObject, true);
                doDamage = true;
            }
        }else
        {
            Player player;
            if (other.gameObject.TryGetComponent<Player>(out player))
            {
                playerInside = true;
                int damage = (int)((2 / player.maxHealth) * 100);
                player.Damage(damage);
                player.GetComponent<DamageOverTime>().ResetTimer();
                player.GetComponent<DamageOverTime>().DamageTime(damage);
            }
        }
    }


    public void OnTriggerExit(Collider other)
    {
        if (!doDamage)
        {
            Player player;
            if (other.gameObject.TryGetComponent<Player>(out player))
            {
                onPlayerEnterTrigger.Invoke(transform.gameObject, false);
            }
        }
        else
        {
            Player player;
            if (other.gameObject.TryGetComponent<Player>(out player))
            {
                playerInside = false;
                player.GetComponent<DamageOverTime>().DamageTime(0);
            }
        }
    }
}
