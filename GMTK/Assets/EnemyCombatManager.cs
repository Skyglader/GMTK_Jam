using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UIElements;

public class EnemyCombatManager : MonoBehaviour
{
    public Vector3 offset;
    public float colliderSize;
    public LayerMask whatIsPlayer;
    public float enemyDamage = 0.5f;
    public bool wizard;

    public SpriteRenderer sprite;
    public AIManager manager;
    public GameObject fireBall;


    public GameObject fireBallStaff;
    public Vector3 staffOffset;
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        manager = GetComponentInParent<AIManager>();
    }
    public void OpenSwordCollider()
    {
        Vector3 adjustedOffset = new Vector3(Mathf.Sign(transform.localScale.x) * offset.x, offset.y, offset.z);
        if (manager.transform.position.x - manager.target.transform.position.x < 0)
        {
            adjustedOffset = new Vector3(Mathf.Sign(-transform.localScale.x) * offset.x, offset.y, offset.z);
        }


        // Calculate the position with the adjusted offset
        Vector3 position = transform.position + adjustedOffset;

        // Detect enemies within the circle
        Collider2D[] playerColliders = Physics2D.OverlapCircleAll(position, colliderSize * transform.parent.transform.localScale.y, whatIsPlayer);

        foreach (Collider2D collider in playerColliders)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
               PlayerManager player = collider.gameObject.GetComponent<PlayerManager>();

                if (player != null)
                {
                    player.ChangeScale(-enemyDamage, true);
                }
            }
        }
    }

    public void DisableColliders()
    {
        manager.col.enabled = false;
    }

    public void DestroySelf()
    {
        Destroy(transform.parent.gameObject);
    }
    
    public void SpawnFireBall()
    {
        Vector3 adjustedOffset = new Vector3(Mathf.Sign(fireBallStaff.transform.localScale.x) * staffOffset.x, staffOffset.y, staffOffset.z);
        if (manager.transform.position.x - manager.target.transform.position.x < 0)
        {
            adjustedOffset = new Vector3(Mathf.Sign(-fireBallStaff.transform.localScale.x) * staffOffset.x, staffOffset.y, staffOffset.z);
        }


        Vector3 position = fireBallStaff.transform.position + adjustedOffset;

        if (fireBall != null && fireBallStaff != null)
        {
            Debug.Log("Fired");
            // Calculate direction from fireball to player
            Vector3 direction = (manager.target.transform.position - position).normalized;

            // Calculate the rotation to face the player
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);

            // Instantiate the fireball and rotate it to face the player
            Instantiate(fireBall, position, rotation);
        }
    }


    void OnDrawGizmos()
    {
        Vector3 adjustedOffset = adjustedOffset = new Vector3(Mathf.Sign(transform.localScale.x) * offset.x, offset.y, offset.z);

        // hardcoding this shit
        if (manager.transform.position.x - manager.target.transform.position.x < 0)
        {
            adjustedOffset = new Vector3(Mathf.Sign(-transform.localScale.x) * offset.x, offset.y, offset.z);
        }
        


                // Calculate the position with the adjusted offset
                Vector3 circleCenter = transform.position + adjustedOffset;

        // Set the color of the Gizmos
        Gizmos.color = Color.red;

        // Calculate the center of the circle

        // Draw the wireframe circle
        Gizmos.DrawWireSphere(circleCenter, colliderSize * transform.parent.transform.localScale.y);


        if (fireBallStaff != null)
        {
            // Calculate the adjusted offset as it would be during spawn
            Vector3 staffAdjustedOffset = new Vector3(Mathf.Sign(fireBallStaff.transform.localScale.x) * staffOffset.x, staffOffset.y, staffOffset.z);
            if (manager != null && manager.target != null)
            {
                if (manager.transform.position.x - manager.target.transform.position.x < 0)
                {
                    staffAdjustedOffset = new Vector3(Mathf.Sign(-fireBallStaff.transform.localScale.x) * staffOffset.x, staffOffset.y, staffOffset.z);
                }
            }

            // Calculate the position where the fireball would spawn
            Vector3 spawnPosition = fireBallStaff.transform.position + staffAdjustedOffset;

            // Draw a sphere at the calculated position
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(spawnPosition, 1f); // Adjust the radius as needed
        }
    }

    
}
