using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEnemyWaveSpawner : MonoBehaviour
{
    [Header("Spawners")]
    public Transform[] spawners;
    public List<GameObject> enemies = new();


    [Header("Wave Settings")]
    public float spawnRate = 1.0f;
    public float timeBetweenWaves = 1f;
    public int enemyCount = 6;
    public bool waveIsDone = true;
    public float distanceAwayFromPlayer;

    public int numWaves = 0;

    private void Awake()
    {
        spawners = gameObject.GetComponentsInChildren<Transform>();
    }
    private void Update()
    {
        if (waveIsDone)
        {
            StartCoroutine(waveSpawner());
        }
    }

    private IEnumerator waveSpawner()
    {
        numWaves++;
        waveIsDone = false;

        for (int i = 0; i < enemyCount; i++)
        {
            bool foundValidSpawner = false;

            while (!foundValidSpawner)
            {
                int randomSpawner = Random.Range(0, spawners.Length);
                int randomEnemy = Random.Range(0, enemies.Count);

                if (Mathf.Abs(spawners[randomSpawner].position.x - WorldGameObjectStorage.instance.player.transform.position.x) >= distanceAwayFromPlayer)
                {
                    foundValidSpawner = true;
                    Instantiate(enemies[randomEnemy], spawners[randomSpawner].position, Quaternion.identity);
                }
            }
            yield return new WaitForSeconds(spawnRate);
        }

        spawnRate -= 0.1f;
        enemyCount += 3;

        yield return new WaitForSeconds(timeBetweenWaves);

        waveIsDone = true;
    }
}
