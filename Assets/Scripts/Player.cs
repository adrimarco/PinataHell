using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Player stats
    [Header("Player stats")]
    public  float movementSpeed = 10.0f;
    private float health        = 100.0f;
    public  float maxHealth     = 100.0f;


    // Components
    private Rigidbody rb = null;
    private Camera cam = null;


    private float camXRotation = 0.0f;
    private float camYRotation = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        rb = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddMovement(Vector2 direction)
    {
        Vector3 movement = GetMovementForward() * direction.y + GetMovementRight() * direction.x;

        movement = movement.normalized * movementSpeed;

        rb.AddForce(movement, ForceMode.Acceleration);
    }

    public void MoveCamera(Vector2 input) {
        camXRotation += input.x;
        camYRotation -= input.y;

        // Clamp vertical camera movement
        camYRotation = Mathf.Clamp(camYRotation, -70, 70);
        
        cam.transform.localRotation = Quaternion.Euler(camYRotation, camXRotation, 0);
    }

    public Vector3 GetMovementForward()
    {
        Vector3 forward = cam.transform.forward;
        forward.y = 0;

        return forward.normalized;
    }

    public Vector3 GetMovementRight()
    {
        return cam.transform.right;
    }
}
