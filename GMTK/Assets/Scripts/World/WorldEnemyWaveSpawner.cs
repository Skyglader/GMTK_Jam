using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public static int bestNormalWave = 0;
    public static int bestHardWave = 0;

    public static WorldEnemyWaveSpawner instance;

    private void Awake()
    {
        if (instance == null)  
            instance = this;
        
     
        spawners = gameObject.GetComponentsInChildren<Transform>();
    }
    private void Update()
    {
        if (waveIsDone)
        {
            StartCoroutine(waveSpawner());
        }

        if (SceneManager.GetActiveScene().name == "Game" && numWaves > bestNormalWave)
        {
            bestNormalWave = numWaves;
        }
        else if (SceneManager.GetActiveScene().name == "Hard Game" && numWaves > bestHardWave)
        {
            bestHardWave = numWaves;
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
                        float randomSize = Random.Range(1.6f, 2.2f);
                        storedEnemyManager.stopDistance = Random.Range(12, 19);
                        storedEnemyManager.transform.localScale = new Vector3(randomSize, randomSize, 1f);
                        
                    }
                    else if (storedEnemyManager.enemyClass == AIManager.EnemyClass.Reaper)
                    {
                        float randomSize = Random.Range(1.6f, 3f);
                        storedEnemyManager.stopDistance = Random.Range(8, 9);
                        storedEnemyManager.transform.localScale = new Vector3(randomSize, randomSize, 1f);

                    }
                    else if (storedEnemyManager.enemyClass == AIManager.EnemyClass.Wizard)
                    {
                        storedEnemyManager.stopDistance = Random.Range(7, 9);
                        float randomSize = Random.Range(1.6f, 2.5f);
                        storedEnemyManager.transform.localScale = new Vector3(randomSize, randomSize, 1f);
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
