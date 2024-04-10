using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerHealth : NetworkBehaviour
{

    // Use this for initialization

    public int maxHealth = 150;
    [SyncVar(hook = "OnHealthChange")]

    public int Health = 100;
    [SyncVar(hook = "OnCurrentHealthChange")]
    public int currentHealth;

    private NetworkStartPosition[] spawnPoints;

    public float respawnWaitTime = 10;
    // [SyncVar(hook = "OnHealthChange")]
    public float timerespawn;
    // [SyncVar]
    public bool isdead;
    public int normallayer, deadlayer;

    public PlayerLevel playerLevel;

    public GameManager gm;


    void Start()
    {
        Health = maxHealth;
        currentHealth = Health;
        isdead = false;
        timerespawn = respawnWaitTime;

        if (isLocalPlayer)
        {
            spawnPoints = FindObjectsOfType<NetworkStartPosition>();
        }
        playerLevel = GetComponent<PlayerLevel>();
        // RpcRespawn();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    void Update()
    {
        Health = maxHealth + 10 * (playerLevel.lvlskill1 - 1);
        if (GameManager.isGameClear || GameManager.isGameOver)
        {

            return;
        }

        if (isdead)
        {

            timerespawn -= Time.deltaTime;
            if (gm != null)
            {
                if (!gm.isStartWave)
                {
                    timerespawn = 0;
                    isdead = false;
                    currentHealth = Health;
                    this.gameObject.layer = normallayer;
                    RpcRespawn();
                    GetComponent<PlayerShoot>().bulletsLeft = GetComponent<PlayerShoot>().bulletsPerClip;
                }
            }
            if (timerespawn < 0)
            {
                timerespawn = 0;
                isdead = false;
                currentHealth = Health;
                this.gameObject.layer = normallayer;
                /* 
                if (!isServer)
                    {
                    RpcRespawn();               
                    }
                */
                RpcRespawn();
                GetComponent<PlayerShoot>().bulletsLeft = GetComponent<PlayerShoot>().bulletsPerClip;

            }
        }
        else
        {
            if (currentHealth <= 0)
            {
                Debug.Log("Dead!");
                timerespawn = respawnWaitTime;
                isdead = true;
                // currentHealth = maxHealth;
                this.gameObject.layer = deadlayer;

            }
            if (currentHealth > Health)
            {
                currentHealth = Health;
            }
        }

    }
    public void TakeDamage(int amount)
    {
        if (!isServer)
            return;

        currentHealth -= amount;
        //   Debug.Log(""+currentHealth +" " + amount);
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Dead!");
        }

    }
    public void GetHealth(int amount)
    {
        print("GetHealth " + amount);
        if (!isServer)
            return;

        currentHealth += amount;


    }
    void OnHealthChange(int hlth)
    {
        // print(hlth);
        Health = hlth;

    }
    void OnCurrentHealthChange(int hlth)
    {
        //  print(hlth);
        currentHealth = hlth;
    }


    [ClientRpc]
    void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            print("spawn");
            // Set the spawn point to origin as a default value
            Vector3 spawnPoint = Vector3.zero;

            // If there is a spawn point array and the array is not empty, pick one at random
            if (spawnPoints != null && spawnPoints.Length > 0)
            {
                spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
            }

            // Set the player’s position to the chosen spawn point
            transform.position = spawnPoint;


        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Pitfall")
        {
            currentHealth = 0;
        }
    }
}
