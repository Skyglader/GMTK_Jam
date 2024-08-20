using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (SceneManager.GetActiveScene().name == "Game" && waveSpawner.numWaves > 1)
            textMeshPro.text = "Game Over\n" + "You lasted " + waveSpawner.numWaves + " rounds.\n" + "Your record is " + WorldEnemyWaveSpawner.bestNormalWave + ".";
        else if (SceneManager.GetActiveScene().name == "Game")
            textMeshPro.text = "Game Over\n" + "You lasted " + waveSpawner.numWaves + " round.\n" + "Your record is " + WorldEnemyWaveSpawner.bestNormalWave + ".";

        if (SceneManager.GetActiveScene().name == "Hard Game" && waveSpawner.numWaves > 1)
            textMeshPro.text = "Game Over\n" + "You lasted " + waveSpawner.numWaves + " rounds.\n" + "Your record is " + WorldEnemyWaveSpawner.bestHardWave + ".";
        else if (SceneManager.GetActiveScene().name == "Hard Game")
            textMeshPro.text = "Game Over\n" + "You lasted " + waveSpawner.numWaves + " round.\n" + "Your record is " + WorldEnemyWaveSpawner.bestHardWave + ".";
        else Debug.Log("null");
    }
}
