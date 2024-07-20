using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MapEvents : MonoBehaviour
{
    public float maxEventDuration = 0.0f;
    [SerializeField] protected float eventDuration = 0.0f;
    public static UnityEvent<bool> onMapEventState = new UnityEvent<bool>();
    protected bool active = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void ActivateEfect() { }

}
