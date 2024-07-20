using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEventsManager : MonoBehaviour
{

    [SerializeField] List<MapEvents> mapEventsList;
    public float maxTimerNextMapEvent = 240.0f;
    public float timerNextMapEvent = 0.0f;
    bool activeEffect = false;

    // Start is called before the first frame update
    void Start()
    {
        timerNextMapEvent = maxTimerNextMapEvent;
        MapEvents.onMapEventState.AddListener(OnMapEventState);
    }

    // Update is called once per frame
    void Update()
    {
        if (timerNextMapEvent > 0.0f)
            timerNextMapEvent -= Time.deltaTime;
        else if (!activeEffect)
        {
            activateRandomEfect();
            
            activeEffect = true;
        }
    }

    void activateRandomEfect()
    {
        int randomEfect = Random.Range(0, mapEventsList.Count);
        GameObject mapEventObject = Instantiate(mapEventsList[randomEfect].gameObject);
        mapEventObject.GetComponent<MapEvents>().ActivateEfect();
    }


    void OnMapEventState(bool active)
    {
        activeEffect = active;
        if (!active) { timerNextMapEvent = maxTimerNextMapEvent; }
    }
}
