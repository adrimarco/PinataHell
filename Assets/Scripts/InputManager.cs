using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private Player player = null;

    // Input actions
    private PlayerInput                 input = null;
    private PlayerInput.PlayerActions   inputActions;

    // Settings
    public float mouseXSensitivity = 1.0f;
    public float mouseYSensitivity = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        InputSetup();
    }

    // Update is called once per frame
    void Update()
    {
        player.MoveCamera(inputActions.Look.ReadValue<Vector2>() * new Vector2(mouseXSensitivity, mouseYSensitivity));
    }

    private void FixedUpdate()
    {
        player.AddMovement(inputActions.Move.ReadValue<Vector2>());
    }

    private void InputSetup()
    {
        input           = new PlayerInput();
        inputActions    = input.Player;

        input.Enable();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}