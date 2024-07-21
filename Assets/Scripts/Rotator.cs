using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float speed = 1;
    public float maxRotation = 90;
    public bool forward = true;

    private float initialRotation = 0;
    private float currentRotation = 0;

    private void Start()
    {
        initialRotation = transform.rotation.eulerAngles.y;
        currentRotation = 0;
    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * speed * (forward ? 1 : -1));
        currentRotation += Time.deltaTime * speed * (forward ? 1 : -1);

        if (forward)
        {
            if (currentRotation > maxRotation) forward = false;
        }
        else
        {
            if (currentRotation < -maxRotation) forward = true;
        }
    }
}
