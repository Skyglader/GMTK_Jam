using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverText : MonoBehaviour
{
    public TMPro.TextMeshProUGUI textMeshPro;
    public WorldEnemyWaveSpawner waveSpawner;
    private void Start()
    {
        
    }

    private void OnEnable()
    {
        waveSpawner = WorldEnemyWaveSpawner.instance;
        if (waveSpawner.numWaves > 1)
            textMeshPro.text = "Game Over\n" + "You lasted " + waveSpawner.numWaves + " rounds.";
        else
            textMeshPro.text = "Game Over\n" + "You lasted " + waveSpawner.numWaves + " round.";
    }
}
