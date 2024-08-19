using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    public AudioSource src;
    public AudioClip attack;
    public AudioClip jump;
    public AudioClip dash;
    public AudioClip run;
    public AudioClip death;
    // Start is called before the first frame update
    private void Awake()
    {
        src = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
