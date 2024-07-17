using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public MeshRenderer mesh = null;

    public float time = 10f;
    public float animationTime = 10f;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyObject", Mathf.Max(time, 0));
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        if (mesh == null) return;

        time -= Time.deltaTime;

        if (time < animationTime / 2)
        {
            mesh.enabled = time % 0.5 > 0.2f;
        } 
        else if (time < animationTime)
        {
            mesh.enabled = time % 1 > 0.2f;
        }
    }
}
