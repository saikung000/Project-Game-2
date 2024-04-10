using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class RandomEnemySpawner : NetworkBehaviour
{

    public int wave;

    public List<int> numMosterSpawnInWave = new List<int>();
    public GameObject enemyPrefab;
    public float timeSpawner;

    public bool spawnFinish;

    public float areaX, areaZ;
    public GameObject[] spawnRandomPoint;

    public override void OnStartServer()
    {
        //wave = 1;
        // StartWave();

    }

    IEnumerator SpawnEnemy()
    {
        timeSpawner = Random.RandomRange(1f, 2f);


        spawnFinish = false;

        for (int i = 0; i < numMosterSpawnInWave[wave - 1]; i++)
        {

            int random = Random.Range(0, spawnRandomPoint.Length);
            if (!GameManager.isGameOver)
            {
                timeSpawner = Random.RandomRange(2f, 5f);
                var enemy = (GameObject)Instantiate(enemyPrefab, spawnRandomPoint[random].transform.position, spawnRandomPoint[random].transform.rotation);
                NetworkServer.Spawn(enemy);

                yield return new WaitForSeconds(timeSpawner);
            }
        }
        spawnFinish = true;



    }

    public void StartWave()
    {
        if (!isServer)
        {
            return;
        }

        StartCoroutine(SpawnEnemy());

    }
    public void StartWaveBoss()
    {
        if (!isServer)
        {
            return;
        }

        StartCoroutine(SpawnEnemyWaveBoss());

    }

    IEnumerator SpawnEnemyWaveBoss()
    {
        timeSpawner = Random.RandomRange(1f, 2f);

        spawnFinish = false;

        while (!GameManager.isGameOver && !GameManager.isGameClear)
        {
            if (GameManager.isGameOver || GameManager.isGameClear)
            {
                break;
            }
            int random = Random.Range(0, spawnRandomPoint.Length);
            if (!GameManager.isGameOver)
            {
                timeSpawner = Random.RandomRange(2f, 5f);
                var enemy = (GameObject)Instantiate(enemyPrefab, spawnRandomPoint[random].transform.position, spawnRandomPoint[random].transform.rotation);
                NetworkServer.Spawn(enemy);

                yield return new WaitForSeconds(timeSpawner);
            }
        }

        spawnFinish = true;



    }


}
