using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float time = 10f;

    // Start is called before the first frame update
    public virtual void Start()
    {
        Invoke("DestroyObject", Mathf.Max(time, 0));
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
