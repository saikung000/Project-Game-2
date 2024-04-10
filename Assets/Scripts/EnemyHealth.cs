using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemyHealth : NetworkBehaviour
{
    public int exp;

    public int score;
    public int maxHealth = 100;
    [SyncVar(hook = "OnCurrentHealthChange")]
    public int currentHealth;

    public Animator anim;

    public float timedie = 1;
    public bool isDead = false;
    public GameObject ParDead;
    void Start()
    {
        try
        {
            anim = GetComponent<Animator>();
        }
        catch
        {

        }

        currentHealth = maxHealth;
    }
    public void TakeDamage(int amount, GameObject player)
    {
        if (!isServer)
        {
            return;
        }
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            player.GetComponent<PlayerLevel>().GetExp(exp);
            player.GetComponent<PlayerScore>().GetScore(score);
            currentHealth = 0;

            // Debug.Log("Dead!");
            try
            {
                if (isDead == false)
                {
                    CmdDead();
                    isDead = true;
                }
            }
            catch
            {

            }

            Destroy(this.gameObject, timedie);
        }
    }

    void OnCurrentHealthChange(int hlth)
    {
        //  print(hlth);
        currentHealth = hlth;
    }
    public float getCurrentHealth()
    {
        float curh = ((float)currentHealth / (float)maxHealth * 100);

        return curh;
    }
    [Command]
    public void CmdDead()
    {

        RpcplayDead();


    }
    [ClientRpc]
    public void RpcplayDead()
    {
        anim.SetTrigger("Dead");
        StartCoroutine(effectDie());

    }

    IEnumerator effectDie()
    {
        if (ParDead != null)
        {
            yield return new WaitForSeconds(timedie - 0.1f);
            var par = (GameObject)Instantiate(ParDead, transform.position, transform.rotation);
            Destroy(par, 5);
        }
    }
}
