using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildToParent : MonoBehaviour
{
    public PlayerManager PlayerManager;

    private void Awake()
    {
        PlayerManager = GetComponentInParent<PlayerManager>();
    }

    public void playerDeath()
    {
        PlayerManager.OnPlayerDeath();
    }

    public void ActivateGameOver()
    {
        PlayerManager.ActivateGameOver();
    }

    public void PlayDeathSFX()
    {
        PlayerManager.playerAudioManager.src.PlayOneShot(PlayerManager.playerAudioManager.death, 1.0f);
    }
}
