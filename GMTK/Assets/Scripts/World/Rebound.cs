using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rebound : MonoBehaviour
{
    public GameObject rebound;
    private void OnTriggerEnter2D(Collider2D other)
    {

        Debug.Log("entered rebound");
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Player"))
            other.gameObject.transform.position = rebound.transform.position;
    }
}
