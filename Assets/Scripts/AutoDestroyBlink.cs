using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyBlink : AutoDestroy
{
    public MeshRenderer mesh = null;
    public float blinkTime = 10f;

    private void Update()
    {
        if (mesh == null) return;

        time -= Time.deltaTime;

        if (time < blinkTime / 2)
        {
            mesh.enabled = time % 0.5 > 0.2f;
        }
        else if (time < blinkTime)
        {
            mesh.enabled = time % 1 > 0.2f;
        }
    }
}
