using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    private float playerJumpHeight = 0f;
    public float jumpPadHeight = 15f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != Player.Instance.gameObject) return;

        Player player = Player.Instance;

        float verticalVelocity = player.controller.GetVerticalVelocity();
        if (verticalVelocity < -4f)
        {
            player.controller.SetVerticalVelocity(verticalVelocity * -0.5f);
            return;
        }

        // Save player jump height if it is not modified
        if (player.controller.JumpHeight < jumpPadHeight) playerJumpHeight = player.controller.JumpHeight;

        // Update jump height
        player.controller.JumpHeight = jumpPadHeight;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject != Player.Instance.gameObject) return;

        Player player = Player.Instance;

        // Update jump height
        player.controller.JumpHeight = playerJumpHeight;
    }
}
