using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Player : MonoBehaviour
{
    // Player stats
    [Header("Player stats")]
    public float maxHealth = 100.0f;
    [SerializeField]
    private float _movementSpeed = 4.0f;
    private float health = 100.0f;

    public float movementSpeed
    {
        get { return _movementSpeed; }
        set
        {
            _movementSpeed = value;
            if (controller != null)
            {
                controller.MoveSpeed = _movementSpeed;
                controller.SprintSpeed = _movementSpeed;
            }
        }
    }


    // Components
    private Rigidbody rb = null;
    private Camera cam = null;
    private CapsuleCollider playerCollider = null;
    private StarterAssets.StarterAssetsInputs input = null;
    private StarterAssets.FirstPersonController controller = null;


    private float camXRotation = 0.0f;
    private float camYRotation = 0.0f;

    public List<GameObject> interactionList;

    // Start is called before the first frame update
    void Start()
    {
        // Get references to necessary components
        rb = GetComponent<Rigidbody>();

        cam = GetComponentInChildren<Camera>();

        playerCollider = GetComponentInChildren<CapsuleCollider>();

        controller = GetComponent<StarterAssets.FirstPersonController>();

        input = GetComponent<StarterAssets.StarterAssetsInputs>();
        input.interactInputEvent.AddListener(Interaction);


        // Update stats
        health = maxHealth;
        movementSpeed = _movementSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interaction()
    {
        if(interactionList.Count > 0)
        {
            Pickable p;
            if (interactionList[0].TryGetComponent<Pickable>(out p))
            {
                p.OnPlayerInteract();
            }
        }
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

    public void AddPickable(GameObject p)
    {
        interactionList.Add(p);
    }

    public void RemovePickable(GameObject p)
    {
        interactionList.Remove(p);
    }
}
