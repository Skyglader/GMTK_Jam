using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Initializations")]
    public Rigidbody2D rb;
    public PlayerInputManager playerInputManager;
    public GameObject playerModel;
    public SpriteRenderer spriteRenderer;
    public GameObject legs;
    public PlayerLocomotionManager playerLocomotionManager;
    public PlayerCombatManager playerCombatManager;
    public GameObject gameOver;
    public GameObject pauseMenu;
    public PlayerAudioManager playerAudioManager;

    public bool isPaused = false;

    [Header("Animation")]
    public Animator animator;
    public Animator legAnimations;

    [Header("Ground")]
    public bool isGrounded;
    public LayerMask whatIsGround;

    [Header("Scaling OverTime")]
    public float scaleRate;
    public float scaleLoss;
    public float nextTimeToScale;
    public float radiusOffset = 0.5f;

    [Header("Stats")]
    public float maxScale = 7f;
    public float deathThreshold = 2.75f;
    public bool isDead = false;

    [Header("Sprite")]
    Color originalColor;

    [SerializeField] private float groundCheckRadius = 2f;
    private void Awake()
    {
        Application.targetFrameRate = 60;
        rb = GetComponent<Rigidbody2D>();
        playerInputManager = GetComponent<PlayerInputManager>();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerCombatManager = GetComponent<PlayerCombatManager>();
        originalColor = spriteRenderer.color;
        playerAudioManager = GetComponent<PlayerAudioManager>();
        
    }
    private void Start()
    {
        nextTimeToScale = Time.time + scaleRate;
    }

    private void Update()
    {
        isGrounded = CheckGrounded();

        if (Time.time > nextTimeToScale)
        {
            nextTimeToScale = Time.time + scaleRate;
            ChangeScale(-scaleLoss, false);
        }

        // TESTINGGGGGGGGGg
        
    }


    public bool CheckGrounded()
    {
        bool touchGround = false;
        Collider2D[] groundColliders = Physics2D.OverlapCircleAll((Vector2)legs.transform.position, groundCheckRadius * transform.localScale.x * radiusOffset, whatIsGround);

        foreach (Collider2D collider in groundColliders)
        {
            
            if (collider.gameObject.CompareTag("Ground"))
            {
                touchGround = true;
            }
        }

        return touchGround;
    }

    public void ChangeScale(float scale, bool playHitAnim)
    {
        if (isDead) return;
        if (scale < 0 && playHitAnim)
        {
            StartCoroutine(FlashRed());
        }


        if (transform.localScale.x + scale < deathThreshold)
        {
            animator.CrossFade("PlayerDeath", 0.1f);
        }
        else if (transform.localScale.x + scale < maxScale)
        {
            transform.localScale = new Vector3(transform.localScale.x + scale, transform.localScale.y + scale, 1f);
        }
        else if (transform.localScale.x + scale >= maxScale)
        {
            transform.localScale = new Vector3(maxScale, maxScale, 1f);
        }
    }
    
    public void OnPlayerDeath()
    {
        isDead = true;
        playerLocomotionManager.stopGravity = true;
        playerLocomotionManager.StopAllMovements(Mathf.Infinity);
        rb.bodyType = RigidbodyType2D.Static;
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = false;

    }

    public void ActivateGameOver()
    {
        Time.timeScale = 0f;
        gameOver.SetActive(true);
    }

    public void TogglePauseMenu()
    {
        if (isPaused)
        {
            isPaused = false;
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            isPaused = true;
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    public IEnumerator FlashRed()
    {
        spriteRenderer.color = new Color(1f, 0f, 0f, originalColor.a);

        yield return new WaitForSecondsRealtime(0.1f);
        spriteRenderer.color = originalColor;
    }
    private void OnDrawGizmos()
    {
        // Draw a red wire sphere to represent the OverlapSphere
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(legs.transform.position, groundCheckRadius * transform.localScale.x * radiusOffset);
    }

}
