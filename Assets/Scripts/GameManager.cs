using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour
{

    [SyncVar(hook = "OnStartWaveChange")]
    public bool isStartWave;

    [SyncVar(hook = "OnWaitTimerChange")]
    public float waitTimer;

    public float timeWait;

    public List<EnemySpawner> listSpawner = new List<EnemySpawner>();


    public RandomEnemySpawner randomSpawner;

    [SyncVar(hook = "OnWaveChange")]
    public int wave = 0;

    public int maxWave;
    [SyncVar(hook = "OnGameStartChange")]
    public bool isGameStart;

    public static bool isGameOver = false;
    public static bool isGameClear = false;
    public GameObject bossprefab, boss;

    public Transform bossSpawnPoint;
    public GameObject[] EnemyArray;
    public bool hasBoss = false;
    void OnWaveChange(int Wave)
    {

        wave = Wave;
    }

    void OnWaitTimerChange(float WaitTime)
    {

        waitTimer = WaitTime;
    }
    void OnStartWaveChange(bool StartWave)
    {
        isStartWave = StartWave;
    }
    void OnGameStartChange(bool GameStart)
    {
        isGameStart = GameStart;
    }

    public override void OnStartServer()
    {
        isGameStart = true;
        wave = 0;
        isStartWave = false;
        waitTimer = timeWait;
        isGameOver = false;
        isGameClear = false;
    }
    // Use this for initialization
    void Start()
    {

        wave = 0;
        isStartWave = false;
        isGameOver = false;
        isGameClear = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.isGameOver)
        {
            return;
        }
        if (GameManager.isGameClear)
        {
            return;
        }
        if (!isGameStart)
        {
            return;
        }
        if (isStartWave && wave == maxWave + 1)
        {

            boss = GameObject.Find("Boss(Clone)");
            if (boss != null)
            {
                hasBoss = true;
            }

            if (boss == null && hasBoss)
            {
                Debug.Log("End");
                StartCoroutine(waitEnd());
            }

        }
        else if (isStartWave)
        {
            if (IsAllFinishSpawnComplete())
            {
                print("Finish");
                EnemyArray = GameObject.FindGameObjectsWithTag("Enemy");
                if (EnemyArray.Length == 0)
                {
                    print("Finish All");
                    if (wave == maxWave - 1)
                    {
                        waitTimer = timeWait * 2;
                    }
                    else
                    {
                        waitTimer = timeWait;
                    }
                    isStartWave = false;
                    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                    foreach (GameObject p in players)
                    {
                        p.GetComponent<PlayerLevel>().GetExp(1000);
                        p.GetComponent<PlayerLevel>().Lvlup();
                        p.GetComponent<PlayerHealth>().GetHealth(1000);
                    }
                }
            }

        }
        else if (!isStartWave && wave == maxWave)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0)
            {
                waitTimer = 0;
                wave++;
                isStartWave = true;
                if (isServer)
                {
                    var enemy = (GameObject)Instantiate(bossprefab, bossSpawnPoint.position, bossprefab.transform.rotation);

                    NetworkServer.Spawn(enemy);
                    int x = 0;
                    foreach (EnemySpawner ES in listSpawner)
                    {
                        enemy.GetComponent<BossControl>().spawnWavePoint.Add(ES.gameObject);
                        x++;
                    }

                    randomSpawner.StartWaveBoss();
                }


            }
        }
        else if (!isStartWave && wave < maxWave)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0)
            {
                waitTimer = 0;
                wave++;
                isStartWave = true;
                for (int i = 0; i < listSpawner.Count; ++i)
                {
                    listSpawner[i].wave = wave;
                    listSpawner[i].StartWave();
                }
                randomSpawner.wave = wave;
                randomSpawner.StartWave();
            }
        }

    }
    IEnumerator waitEnd()
    {

        yield return new WaitForSeconds(10);
        isStartWave = false;
        isGameClear = true;
    }

    private bool IsAllFinishSpawnComplete()
    {
        for (int i = 0; i < listSpawner.Count; ++i)
        {
            if (listSpawner[i].spawnFinish == false)
            {
                return false;
            }
        }
        if (randomSpawner.spawnFinish)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
