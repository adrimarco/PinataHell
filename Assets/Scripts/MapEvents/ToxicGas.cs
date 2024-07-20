using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicGas : MapEvents
{

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (eventDuration <= maxEventDuration)
        {
            eventDuration += Time.deltaTime;
        }
        else
        {
            onMapEventState.Invoke(false);
            active = false;
        } 
    }

    public override void ActivateEfect()
    {
        onMapEventState.Invoke(true);
        active = true;
    }


    public void OnTriggerEnter (Collider other)
    {
        if (!active) return;


        Player player;
        if (other.gameObject.TryGetComponent<Player>(out player))
        {
            int damage = (int)((2 / player.maxHealth) * 100);
            player.Damage(damage);
            player.transform.gameObject.GetComponent<DamageOverTime>().ResetTimer();
            player.transform.gameObject.GetComponent<DamageOverTime>().DamageTime(damage);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (!active) return;
        

        Player player;
        if (other.gameObject.TryGetComponent<Player>(out player))
        {
            player.transform.gameObject.GetComponent<DamageOverTime>().DamageTime(0);
        }
    }
}
