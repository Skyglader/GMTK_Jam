using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class AIManager : MonoBehaviour
{
    [SerializeField] private float movement;
    public Transform target;
    private Rigidbody2D rb;
    public SpriteRenderer sprite;
    public Animator animator;
    public Collider2D col;
    private Vector3 targetPosition;
    public float gravityScale = 14f;

    [Header("Check Grounded")]
    public bool isGrounded = false;
    public GameObject legs;
    public float groundCheckRadius;
    public float radiusOffset;
    public LayerMask whatIsGround;

    [Header("Distance Check")]
    public float stopDistance;
    private Vector3 previousPosition;
    private bool inRange = false;
    private bool canAttack = true;
    public float attackResetTime = 1.5f;

    [Header("Animation State")]
    AnimatorStateInfo animatorState;

    public EnemyClass enemyClass;
    public enum EnemyClass
    {
        Reaper,
        Wizard,
        Wizard2
    }

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
    }
    private void Start()
    {
        target = WorldGameObjectStorage.instance.player.transform;
    }

    private void Update()
    {
        isGrounded = CheckIsGrounded();
        animatorState = animator.GetCurrentAnimatorStateInfo(0);
    }
    private void FixedUpdate()
    {

        if (!isGrounded)
        {
            ApplyGravityOnGround();
        }
        else
        {
            HandleEnemyMovement();
        }
    }

    private void ApplyGravityOnGround()
    {
        Vector2 velocity = Physics2D.gravity * gravityScale * Time.deltaTime;

        // Apply the velocity to the kinematic Rigidbody2D
        rb.MovePosition(rb.position + velocity * Time.deltaTime);
    }

    private bool CheckIsGrounded()
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

    private void HandleEnemyMovement()
    {
        float distance = Vector2.Distance(transform.position, target.position);

        inRange = distance <= stopDistance;

        if (target != null && !inRange)
        {
            targetPosition = new Vector3(target.position.x, rb.position.y, 0);
            rb.position = Vector2.MoveTowards(transform.position, targetPosition, movement * Time.deltaTime);

            animator.SetBool("IsWalking", true);

        }
        else
        {
            animator.SetBool("IsWalking", false);

            if (inRange && canAttack)
            {
                canAttack = false;
                animator.SetTrigger("IsAttacking");
                StartCoroutine(ResetAttackBool(attackResetTime));

            }
        }

        if (!animatorState.IsName("Attack") || enemyClass == EnemyClass.Wizard2)
        {
            Debug.Log(animatorState.IsName("Attack"));
            if (target.transform.position.x - transform.position.x < 0)
            {
                //Enemy is moving left
                if (enemyClass == EnemyClass.Reaper)
                    sprite.flipX = false;
                else
                    sprite.flipX = true;
            }
            else if (target.transform.position.x - transform.position.x > 0)
            {
                if (enemyClass == EnemyClass.Reaper)
                    sprite.flipX = true;
                else
                    sprite.flipX = false;
            }
        }


        previousPosition = rb.position;

    }

    private IEnumerator ResetAttackBool(float time)
    {
        yield return new WaitForSeconds(time);
        canAttack = true;
    }

    private void OnDrawGizmos()
    {
        // Draw a red wire sphere to represent the OverlapSphere
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(legs.transform.position, groundCheckRadius * transform.localScale.x * radiusOffset);
    }
}
