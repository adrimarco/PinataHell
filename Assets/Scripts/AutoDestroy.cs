using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float time = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyObject", Mathf.Max(time, 0));
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
