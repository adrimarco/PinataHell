using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorIsLava : MapEvents
{
    Vector3 initPosition = new Vector3(0, -3, 0);
    Vector3 finalPosition = new Vector3(0, -0.6f, 0);
    float movementSpeed = 20f;
    GameObject lavaObject;
    

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
            StartCoroutine(MoveLava(finalPosition, initPosition, true));
        } 
    }

    public override void ActivateEfect()
    {
        onMapEventState.Invoke(true);
        active = true;

        transform.position = initPosition;
        StartCoroutine(MoveLava(initPosition, finalPosition, false));
    }


    public void OnTriggerEnter (Collider other)
    {
        if (!active) return;


        Player player;
        if (other.gameObject.TryGetComponent<Player>(out player))
        {
            player.Damage((int)((5 / player.maxHealth) * 100));
            player.transform.gameObject.GetComponent<DamageOverTime>().ResetTimer();
            player.transform.gameObject.GetComponent<DamageOverTime>().DamageTime((int)((5/player.maxHealth)*100));
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

    IEnumerator MoveLava(Vector3 init, Vector3 final, bool destroy)
    {
        float elapsedTime = 0;

        while (elapsedTime < movementSpeed)
        {
            transform.position = Vector3.Lerp(init, final, (elapsedTime / movementSpeed));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (destroy)
            Destroy(transform.gameObject);
    }
}
