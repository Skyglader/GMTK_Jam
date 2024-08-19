using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGameObjectStorage : MonoBehaviour
{
    public GameObject player;

    public static WorldGameObjectStorage instance;
    private void Awake()
    {
        if (instance == null)  
            instance = this;
        else
            Destroy(gameObject);
    }
}
