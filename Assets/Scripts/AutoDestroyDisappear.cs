using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyDisappear : AutoDestroy
{
    public float disappearTime = 1f;
    private Vector3 initialScale = Vector3.negativeInfinity;

    public override void Start()
    {
        base.Start();

        initialScale = transform.localScale;
    }

    private void Update()
    {
        time -= Time.deltaTime;

        if (time < disappearTime)
        {
            transform.localScale = (time / disappearTime) * initialScale;
        }
    }
}
