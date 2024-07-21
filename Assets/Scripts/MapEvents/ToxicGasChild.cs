using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToxicGasChild : MonoBehaviour
{

    public static UnityEvent<GameObject, bool> onPlayerEnterTrigger = new UnityEvent<GameObject, bool>();
    public bool doDamage = false;
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
                int damage = (int)((2 / player.maxHealth) * 100);
                player.Damage(damage);
                player.transform.gameObject.GetComponent<DamageOverTime>().ResetTimer();
                player.transform.gameObject.GetComponent<DamageOverTime>().DamageTime(damage);
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
                player.transform.gameObject.GetComponent<DamageOverTime>().DamageTime(0);
            }
        }
    }
}
