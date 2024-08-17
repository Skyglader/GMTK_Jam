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

    [SerializeField] private float groundCheckRadius = 2f;
    private void Awake()
    {
        Application.targetFrameRate = 60;
        rb = GetComponent<Rigidbody2D>();
        playerInputManager = GetComponent<PlayerInputManager>();
        
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
            transform.localScale = new Vector3(transform.localScale.x - scaleLoss, transform.localScale.y - scaleLoss, transform.localScale.z);
        }
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

    private void OnDrawGizmos()
    {
        // Draw a red wire sphere to represent the OverlapSphere
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(legs.transform.position, groundCheckRadius * transform.localScale.x * radiusOffset);
    }

}
