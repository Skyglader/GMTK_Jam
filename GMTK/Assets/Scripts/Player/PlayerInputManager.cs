using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    PlayerControls playerControls;

    [SerializeField] Vector2 movementInput;
    [SerializeField] Vector2 movementDir;
    public bool isJumping = false;
    public bool isAttacking = false;
    public bool isDashing = false;
    public bool isPausing = false;

    private bool pauseInputProcessed = false;

    public PlayerManager playerManager;
    private void OnEnable()
    {
        playerManager = GetComponent<PlayerManager>();
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Jumping.performed += i => isJumping = true;
            playerControls.PlayerMovement.Jumping.canceled += i => isJumping = false;
            playerControls.PlayerActions.Attack.performed += i => isAttacking = true;
            playerControls.PlayerActions.Attack.canceled += i => isAttacking = false;
            playerControls.PlayerMovement.Dashing.performed += i => isDashing = true;
            playerControls.PlayerMovement.Dashing.canceled += i => isDashing = false;
            playerControls.PlayerActions.Pause.started += OnPauseInput;
        }

        playerControls.Enable();
    }
    private void OnPauseInput(InputAction.CallbackContext context)
    {
        if (!pauseInputProcessed)
        {
            isPausing = !isPausing;
            pauseInputProcessed = true;
        }
    }
    private void Update()
    {
        // Reset the pause input flag when the input is released
        if (playerControls.PlayerActions.Pause.triggered)
        {
            pauseInputProcessed = false;
            playerManager.TogglePauseMenu();
        }

        // Use the input states in your game logic
        if (isJumping)
        {
            // Handle jumping
        }

        if (isAttacking)
        {
            // Handle attacking
        }

        if (isDashing)
        {
            // Handle dashing
        }

        if (isPausing)
        {
            // Handle pausing
        }
    }
    public Vector2 GetMoveDirection()
    {
        movementDir = movementInput.normalized;
        return movementDir;
    }
}
