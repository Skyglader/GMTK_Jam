using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class FireBallMovementManager : MonoBehaviour
{
    public Rigidbody2D rb;
    public float velocity = 10f;
    public float damage = 0.25f;
    public Collider2D col;
    public SpriteRenderer sprite;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.velocity = Vector2.zero;
            col.enabled = false;
            sprite.enabled = false;

            PlayerManager player = collision.gameObject.GetComponent<PlayerManager>();

            if (player != null)
            {
                player.ChangeScale(-damage, true);
            }
        }
    }
    private void Update()
    {
        rb.velocity = transform.up * velocity;
    }
}
