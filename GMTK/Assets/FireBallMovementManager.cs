using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class FireBallMovementManager : MonoBehaviour
{
    public Rigidbody2D rb;
    public float velocity = 10f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        rb.velocity = transform.up * velocity;
    }
}
