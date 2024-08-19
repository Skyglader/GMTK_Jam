using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingAudio : MonoBehaviour
{

    public PlayerManager player;
    public AudioSource src;
    public AudioClip clip;
    public bool played = false;
    private void Awake()
    {
        player = GetComponentInParent<PlayerManager>(); 
        src = GetComponent<AudioSource>();
        
    }

    private void Start()
    {
        src.clip = clip;
        src.Pause();
    }
    private void Update()
    {
        if (player.animator.GetBool("IsMoving") && !played)
        {
            src.Play();
            played = true;
        }
        else if (!player.animator.GetBool("IsMoving") && played)
        {
            src.Pause();
            played = false;
        }
    }
}
