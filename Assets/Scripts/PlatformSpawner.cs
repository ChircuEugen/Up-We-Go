using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;

    public Transform platformPrefab;
    public Transform enemyPrefab;

    private int previousPosition;

    public bool keepSpawning = true;

    private void Start()
    {
        InvokeRepeating("SpawnPlatform", 0, 3.5f);
    }

    private void SpawnPlatform()
    {
        if (!keepSpawning) return;

        int randPlatform;
        if (previousPosition == 2)
        {
            randPlatform = Random.Range(0, spawnPoints.Length - 1);
        }
        else
        {
            randPlatform = 2;
        }

        previousPosition = randPlatform;

        int enemiesOnPlatform = Random.Range(0, 3);
        Transform spawnedPlatform = Instantiate(platformPrefab, spawnPoints[randPlatform].position, spawnPoints[randPlatform].rotation);
        EnemySpawnPoint[] enemySpawnPoints = spawnedPlatform.GetComponentsInChildren<EnemySpawnPoint>();

        for(int i=0; i<enemiesOnPlatform; i++)
        {
            Instantiate(enemyPrefab, enemySpawnPoints[i].transform.position, enemySpawnPoints[i].transform.rotation);
        }
    }
}
