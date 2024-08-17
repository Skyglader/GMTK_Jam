using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerLocomotionManager : MonoBehaviour
{
    [Header("Initializations")]
    public PlayerManager player;
    

    [Header("Gravity")]
    public float gravityInAir = -13f;
    public float gravityOnGround = -9.81f;
    private Vector3 velocity;

    [Header("Grounded Movement")]
    public float moveSpeed = 4f;

    [Header("Jumping")]
    public float jumpStartTime;
    public float jumpTime;
    public float jumpForce;

    [Header("Animations")]
    public bool startingMoveAnimationPlayed = false;
    
    private void Awake()
    {
        player = GetComponent<PlayerManager>();
        
    }

    private void Update()
    {
        HandleJumpingMovement();
    }

    private void FixedUpdate()
    {
        ApplyGravity();
        HandlePlayerMovement();
    }

    private void ApplyGravity()
    {

        if (player.isGrounded)
        {
            player.rb.AddForce(new Vector2(0, gravityOnGround), ForceMode2D.Force);
            Debug.Log("applying ground gravity");
        }    
        else if (!player.isGrounded)
        {
            Debug.Log("applying air gravity");
            player.rb.AddForce(new Vector2(0, gravityOnGround), ForceMode2D.Force);
        }
           
        HandleInAirAnimations();
     
    }

    private void HandleInAirAnimations()
    {
        if (player.rb.velocity.y < 0)
        {
            player.animator.SetBool("IsRising", false);
            player.animator.SetBool("IsFalling", true);
        }
        else if (player.isGrounded)
        {
            Debug.Log("IsFalling disabled");
            player.animator.SetBool("IsFalling", false);
            player.animator.SetBool("IsRising", false);

        }
    }

    private void HandleJumpingMovement()
    {
        if (player.isGrounded == true && player.playerInputManager.isJumping == true)
        {
            player.animator.SetBool("IsRising", true);
            jumpTime = jumpStartTime;
            player.rb.velocity = Vector2.up * jumpForce;
        }

        if (player.playerInputManager.isJumping == true)
        {
            if (jumpTime > 0)
            {
                player.rb.velocity = Vector2.up * jumpForce;
                jumpTime -= Time.deltaTime;
            }
        }
    }
    private void HandlePlayerMovement()
    {
        Vector2 direction = player.playerInputManager.GetMoveDirection();

        if (direction != Vector2.zero)
        {
            Vector2 targetVelocity = direction * moveSpeed;

            // Apply horizontal movement while preserving the current vertical velocity
            player.rb.velocity = new Vector2(targetVelocity.x, player.rb.velocity.y);

            if (direction.x < 0)
            {
                player.playerModel.transform.localScale = new Vector3(-1, 1, 1);

            }
            else if (direction.x > 0)
            {
                player.playerModel.transform.localScale = new Vector3(1, 1, 1);

            }

            if (player.isGrounded)
            {
                player.animator.SetBool("IsFalling", false);
                player.animator.SetBool("IsMoving", true);

                if (!startingMoveAnimationPlayed)
                {
                    startingMoveAnimationPlayed = true;
                    player.legAnimations.CrossFade("DashParticle", 0.1f);
                }
            }
            else
            {
                player.animator.SetBool("IsMoving", false);
            }
                
        }
        else
        {
            
             player.animator.SetBool("IsMoving", false);
            startingMoveAnimationPlayed = false;

            player.rb.velocity = new Vector2(0, player.rb.velocity.y);
        }

        

 
    }
}
