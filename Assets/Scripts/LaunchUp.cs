using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchUp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rb;
        if (TryGetComponent<Rigidbody>(out rb))
        {
            rb.AddForce(Vector3.up * 5, ForceMode.Impulse);
        }
    }
}
