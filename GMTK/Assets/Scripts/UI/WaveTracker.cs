using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveTracker : MonoBehaviour
{
    public TextMeshProUGUI waveText;
    public WorldEnemyWaveSpawner waveSpawner;

    private void Start()
    {
        waveSpawner = WorldEnemyWaveSpawner.instance;
    }
    private void Update()
    {
        waveText.text = "WAVE " + waveSpawner.numWaves;
    }
}
