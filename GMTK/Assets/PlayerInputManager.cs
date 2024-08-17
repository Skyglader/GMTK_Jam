using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    PlayerControls playerControls;

    [SerializeField] Vector2 movementInput;
    [SerializeField] Vector2 movementDir;
    public bool isJumping = false;
    public bool isAttacking = false;


    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Jumping.performed += i => isJumping = true;
            playerControls.PlayerMovement.Jumping.canceled += i => isJumping = false;
            playerControls.PlayerActions.Attack.performed += i => isAttacking = true;
            playerControls.PlayerActions.Attack.canceled += i => isAttacking = false;
        }

        playerControls.Enable();
    }

    public Vector2 GetMoveDirection()
    {
        movementDir = movementInput.normalized;
        return movementDir;
    }
}
