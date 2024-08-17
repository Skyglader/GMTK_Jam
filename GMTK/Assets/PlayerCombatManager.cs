using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatManager : MonoBehaviour
{
    public PlayerManager player;
    public float timeToPauseMovement = 0.15f;
    public float timeToDelay = 0.05f;

    public bool secondAttackQueued = false;
    AnimatorStateInfo currentState;

    public bool isAttacking = false;
    public AttackState attackState;
    public enum AttackState
    {
        Idle,
        Attack1,
        Attack2,
    }
    private void Awake()
    {
        player = GetComponent<PlayerManager>();
    }

    private void Update()
    {
        HandleAttackMovement();
        currentState = player.animator.GetCurrentAnimatorStateInfo(1);
    }

    public void HandleAttackMovement()
    {
        switch (attackState)
        {
            case AttackState.Idle:
                if (player.playerInputManager.isAttacking && player.isGrounded && isAttacking == false)
                {
                    isAttacking = true;
                    StartCoroutine(DelayMovementPause(timeToDelay));
                    player.playerLocomotionManager.StopJumpMovements(timeToPauseMovement + timeToDelay);
                    player.animator.CrossFade("Attack1", 0.1f);
                    attackState = AttackState.Attack1;
                }
                break;

            case AttackState.Attack1:
                Debug.Log($"Current state {currentState}");
                Debug.Log($"Attacking: {player.playerInputManager.isAttacking}");
                Debug.Log($"Grounded: {player.isGrounded}");
                if (currentState.IsName("Attack1") && player.playerInputManager.isAttacking && player.isGrounded)
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
}
