using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatManager : MonoBehaviour
{
    public PlayerManager player;
    public float timeToPauseMovement = 0.15f;
    public float timeToDelay = 0.05f;
    public float hitStopTime;

    public bool secondAttackQueued = false;
    AnimatorStateInfo currentState;

    public bool isAttacking = false;
    public AttackState attackState;

    public Vector3 offset;
    public float colliderSize;
    public LayerMask whatIsEnemy;

    public bool stopAllAttacks = false;
    public GameObject bloodSplatter;

    float originalPitch;

    [Header("Damage and Heal Values")]
    public float healPerHit;

    public enum AttackState
    {
        Idle,
        Attack1,
        Attack2,
    }
    private void Awake()
    {
        player = GetComponentInParent<PlayerManager>();
    }
    private void Start()
    {
        originalPitch = player.playerAudioManager.src.pitch;
    }

    private void Update()
    {
        if (player.isDead) return;
        HandleAttackMovement();
        currentState = player.animator.GetCurrentAnimatorStateInfo(1);
    }

    public void HandleAttackMovement()
    {
        switch (attackState)
        {
            case AttackState.Idle:
                if (player.playerInputManager.isAttacking && isAttacking == false)
                {
                    isAttacking = true;
                    StartCoroutine(DelayMovementPause(timeToDelay));
                    player.playerLocomotionManager.StopJumpMovements(timeToPauseMovement + timeToDelay);
                    player.animator.CrossFade("Attack1", 0.1f);
                    attackState = AttackState.Attack1;
                }
                break;

            case AttackState.Attack1:
                if (currentState.IsName("Attack1") && player.playerInputManager.isAttacking)
                {
                    Debug.Log("Entered state 2");
                    attackState = AttackState.Attack2;
                }
                else if (!currentState.IsName("Attack1"))
                {
                    attackState = AttackState.Idle;
                    isAttacking = false;
                }
                break;

            case AttackState.Attack2:
                if (!player.animator.GetBool("SecondAttack"))
                {
                    player.animator.SetBool("SecondAttack", true);
                    player.playerLocomotionManager.StopAllMovements(timeToPauseMovement);
                }

                // Ensure the attack flag is reset
                isAttacking = false;

                // Only return to Idle after both Attack1 and Attack2 animations have finished
                if (!currentState.IsName("Attack2") && !currentState.IsName("Attack1"))
                {
                    player.animator.SetBool("SecondAttack", false);
                    attackState = AttackState.Idle;
                }
                break;

        }

    }

    private IEnumerator DelayMovementPause(float time)
    {
        yield return new WaitForSeconds(time);

        player.playerLocomotionManager.StopAllMovements(timeToPauseMovement);
    }

    public void OpenSwordCollider()
    {
        Vector3 adjustedOffset = new Vector3(
            Mathf.Sign(transform.localScale.x) * offset.x,
            offset.y,
            offset.z
        );

        // Calculate the position with the adjusted offset
        Vector3 position = transform.position + adjustedOffset;

        // Detect enemies within the circle
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(position, colliderSize, whatIsEnemy);

        if (enemyColliders.Length > 0)
        {
            HitStop.instance.Stop(hitStopTime);
        }

        foreach (Collider2D collider in enemyColliders)
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                player.ChangeScale(healPerHit, false);
                EnemyHealthManager enemyHealthManager = collider.gameObject.GetComponent<EnemyHealthManager>();

                if (enemyHealthManager != null)
                {
                    enemyHealthManager.TakeScaleDamage();
                }
                GameObject blood = null;
                if (transform.position.x - collider.gameObject.transform.position.x < 0)
                     blood = Instantiate(bloodSplatter, collider.gameObject.transform.position, Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z));
                else if (transform.position.x - collider.gameObject.transform.position.x > 0)
                    blood = Instantiate(bloodSplatter, collider.gameObject.transform.position, Quaternion.Euler(transform.rotation.x, 180f, transform.rotation.z));

                if (blood != null)
                    Destroy(blood, 1f);
            }
        }
    }

    void PlaySlashSFX()
    {
        player.playerAudioManager.src.pitch = Random.Range(1f, 2f);
        player.playerAudioManager.src.PlayOneShot(player.playerAudioManager.attack, 0.25f);
        StartCoroutine(resetPitch());
    }

    public IEnumerator resetPitch()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        player.playerAudioManager.src.pitch = originalPitch;
    }

    void OnDrawGizmos()
    {
        Vector3 adjustedOffset = new Vector3(
           Mathf.Sign(transform.localScale.x) * offset.x,
           offset.y,
           offset.z
       );

        // Calculate the position with the adjusted offset
        Vector3 circleCenter = transform.position + adjustedOffset;

        // Set the color of the Gizmos
        Gizmos.color = Color.red;

        // Calculate the center of the circle

        // Draw the wireframe circle
        Gizmos.DrawWireSphere(circleCenter, colliderSize);
    }
}
