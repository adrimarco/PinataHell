using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ToxicGas : MapEvents
{


    List<BoxCollider> areasGameObjects;
    List<ParticleSystem> particleSystems;
    GameObject activeArea;
    bool checkArea = true;

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
            activeArea.GetComponent<ToxicGasChild>().doDamage = false;
            Destroy(transform.gameObject);
        } 
    }

    public override void ActivateEfect()
    {
        onMapEventState.Invoke(true);
        active = true;
        checkArea = true;
        areasGameObjects = new List<BoxCollider>(transform.gameObject.GetComponentsInChildren<BoxCollider>());
        particleSystems = new List<ParticleSystem>();
        foreach (BoxCollider t in areasGameObjects)
        {
            t.gameObject.SetActive(true);
            particleSystems.Add(t.GetComponentInChildren<ParticleSystem>());
            particleSystems.Last<ParticleSystem>().gameObject.SetActive(false);
        }

        ToxicGasChild.onPlayerEnterTrigger.AddListener(OnPlayerEnterAreaTrigger);
    }

    private void OnPlayerEnterAreaTrigger(GameObject areaGameObject, bool enter)
    {
        if (enter && checkArea)
        {
            activeArea = areaGameObject;
            checkArea = false;
            for (int i = 0; i < areasGameObjects.Count; i++)
            {
                if (areasGameObjects[i].gameObject != activeArea)
                {
                    areasGameObjects[i].gameObject.SetActive(false);
                }else
                {
                    particleSystems[i].gameObject.SetActive(true);
                    // Reactivate object to start making damage to the player 
                    activeArea.GetComponent<BoxCollider>().gameObject.SetActive(false);
                    activeArea.GetComponent<BoxCollider>().gameObject.SetActive(true);
                }
            }
        }
    }
}
