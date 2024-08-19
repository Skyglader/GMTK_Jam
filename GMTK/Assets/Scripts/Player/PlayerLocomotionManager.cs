using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public bool hasJumped = false;

    [Header("Animations")]
    public bool startingMoveAnimationPlayed = false;


    [Header("Movement bools")]
    private bool stopMoving = false;
    private bool stopJumping = false;
    public bool stopGravity = false;
    public bool movementAnimationActive = false;

    [Header("Dashing")]
    public bool canDash = true;
    public bool isDashing;
    public float dashingPower = 24f;
    private float dashingTime = 0.2f;
    public float dashingCooldown = 1f;
    private TrailRenderer trailRenderer;
    private void Awake()
    {
        player = GetComponent<PlayerManager>();
        trailRenderer = GetComponent<TrailRenderer>();
        
    }

    private void Update()
    {
        if (isDashing)
        {
            return;
        }

        if (player.playerInputManager.isDashing && canDash && !stopMoving)
        {
            StartCoroutine(Dash());
        }

        HandleJumpingMovement();
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        ApplyGravity();
        HandlePlayerMovement();
    }

    private void ApplyGravity()
    {
        if (stopGravity)
        {
            return;
        }
        if (player.isGrounded)
        {
            player.rb.AddForce(new Vector2(0, gravityOnGround), ForceMode2D.Force);
        }    
        else if (!player.isGrounded)
        {
            player.rb.AddForce(new Vector2(0, gravityInAir), ForceMode2D.Force);
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
        if (stopJumping)
        {
            return;
        }


        if (player.isGrounded == true && player.playerInputManager.isJumping == true)
        {
            player.animator.SetBool("IsRising", true);
            jumpTime = jumpStartTime;
            player.rb.velocity = Vector2.up * jumpForce;
            hasJumped = true;
        }
        else if (player.playerInputManager.isJumping == true)
        {
            if (jumpTime > 0 && hasJumped)
            {
                player.rb.velocity = Vector2.up * jumpForce;
                jumpTime -= Time.deltaTime;
            }
        }
        else if (!player.playerInputManager.isJumping)
        {
            // Reset jump status when the jump button is released
            hasJumped = false;
        }
    }
    private void HandlePlayerMovement()
    {
        if (stopMoving)
        {
            player.rb.velocity = Vector2.zero;
            return;
        }

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
                if (!movementAnimationActive)
                {
                    player.playerAudioManager.src.clip = player.playerAudioManager.run;
                    //player.playerAudioManager.src.Play();
                    movementAnimationActive = true;
                }
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
            movementAnimationActive = false;
            //player.playerAudioManager.src.Pause();
            player.animator.SetBool("IsMoving", false);
            startingMoveAnimationPlayed = false;

            player.rb.velocity = new Vector2(0, player.rb.velocity.y);
        }

        
    }

    public void StopGroundMovements(float time=2f)
    {
        StartCoroutine(ResetGroundBool(time));
    }

    private IEnumerator ResetGroundBool(float time)
    {
        stopMoving = true;
        yield return new WaitForSeconds(time);
        stopMoving = false;
    }

    public void StopJumpMovements(float time = 2f)
    {
        StartCoroutine(ResetJumpBool(time));
    }

    private IEnumerator ResetJumpBool(float time)
    {
        stopJumping = true;
        yield return new WaitForSeconds(time);
        stopJumping = false;
    }

    public void StopAllMovements(float time=2f)
    {
        StopGroundMovements(time);
        StopJumpMovements(time);
    }

    private IEnumerator Dash()
    {
        Vector2 direction = player.playerInputManager.GetMoveDirection();
        canDash = false;
        isDashing = true;
        player.animator.SetBool("IsDashing", true);
        player.playerAudioManager.src.PlayOneShot(player.playerAudioManager.dash);
        float originalGravity = player.rb.gravityScale;
        player.rb.gravityScale = 0f;
        player.rb.velocity = new Vector2(direction.normalized.x * dashingPower, 0f);
        trailRenderer.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        trailRenderer.emitting = false;
        player.rb.gravityScale = originalGravity;
        player.animator.SetBool("IsDashing", false);
        isDashing = false;
        player.rb.velocity = new Vector2(5f, 0f);
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}
