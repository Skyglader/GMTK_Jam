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
    public static int bestWave = 0;

    public static WorldEnemyWaveSpawner instance;

    private void Awake()
    {
        if (instance == null)  
            instance = this;
        else
            Destroy(gameObject);
        spawners = gameObject.GetComponentsInChildren<Transform>();
    }
    private void Update()
    {
        if (waveIsDone)
        {
            StartCoroutine(waveSpawner());
        }

        if (numWaves > bestWave)
        {
            bestWave = numWaves;
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
                    GameObject enemy = Instantiate(enemies[randomEnemy], spawners[randomSpawner].position, Quaternion.identity);


                    
                    AIManager storedEnemyManager;
                    storedEnemyManager = enemy.GetComponent<AIManager>();
                    if (storedEnemyManager.enemyClass == AIManager.EnemyClass.Wizard2)
                    {
                        storedEnemyManager.stopDistance = Random.Range(12, 19);
                    }
                    else if (storedEnemyManager.enemyClass == AIManager.EnemyClass.Reaper)
                    {
                        storedEnemyManager.stopDistance = Random.Range(5, 8);
                    }
                    else if (storedEnemyManager.enemyClass == AIManager.EnemyClass.Wizard)
                    {
                        storedEnemyManager.stopDistance = Random.Range(5, 8);
                    }


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
