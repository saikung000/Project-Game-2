using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CoreHealth : NetworkBehaviour
{

    public Image hpbar;

    public int maxHealth = 1000;

    [SyncVar(hook = "OnHealthChange")]
    public int currentHealth;

    public bool CoreDestroy;
    // Use this for initialization
    void Start()
    {
        hpbar = GameObject.Find("CoreHpbar").GetComponent<Image>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {


        if (!CoreDestroy)
        {
            if (currentHealth <= 0)
            {
                GameManager.isGameOver = true;
                Debug.Log("CoreDestroy!");
                CoreDestroy = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            currentHealth += 100;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            currentHealth -= 1000;
        }


    }

    public void TakeDamage(int amount)
    {
        if (!isServer)
            return;
        currentHealth -= amount;

    }
    void OnHealthChange(int hlth)
    {
        print("Core : " + hlth);
        currentHealth = hlth;
        if (hpbar != null)
        {
            hpbar.fillAmount = (float)currentHealth / (float)maxHealth;
        }
    }
}
