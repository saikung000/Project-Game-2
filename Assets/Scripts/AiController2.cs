using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
public class AiController2 : NetworkBehaviour
{
    public Transform core;
    public List<Transform> playerList = new List<Transform>();

    public GameObject[] players;

    public Transform targetPlayer;

    public int damage = 30;
    NavMeshAgent agent;
    public float distanceBomb = 10;

    public float distanceBombPlayer = 2;

    public float speed;
    private float currentSpeed;

    public bool active;
    public Animator anim;
    public EnemyHealth eh;

    public GameObject boomPar;
    // Use this for initialization
    void Start()
    {
        try
        {
            anim = GetComponent<Animator>();
        }
        catch
        {

        }
        eh = GetComponent<EnemyHealth>();
        currentSpeed = speed;
        agent = GetComponent<NavMeshAgent>();
        core = GameObject.FindGameObjectWithTag("Core").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.isGameOver || GameManager.isGameClear)
        {

            agent.Stop();
            Destroy(this.gameObject);
            return;
        }
        if (!active)
        {
            agent.Stop();
            return;
        }
        if (eh.currentHealth <= 0)
        {
            agent.Stop();
            return;
        }
        agent.speed = currentSpeed;
        findnearplayer();
        if (targetPlayer != null)
        {
            agent.SetDestination(targetPlayer.position);
            if (Vector3.Distance(transform.position, targetPlayer.position) < distanceBombPlayer)
            {
                PlayerHealth ph = targetPlayer.gameObject.GetComponent<PlayerHealth>();
                ph.TakeDamage(damage);
                var par = (GameObject)Instantiate(boomPar, transform.position, transform.rotation);

                NetworkServer.Spawn(par);
                Destroy(par, 5);
                Destroy(this.gameObject);
                //Network.Destroy(this.gameObject);

            }
            if (Vector3.Distance(transform.position, targetPlayer.position) < 6)
            {
                anim.SetBool("Near", true);
            }
            else
            {
                anim.SetBool("Near", false);
            }
        }
        else
        {
            agent.SetDestination(core.position);
            if (Vector3.Distance(transform.position, core.position) < distanceBomb)
            {
                CoreHealth ch = core.gameObject.GetComponent<CoreHealth>();
                ch.TakeDamage(damage);
                var par = (GameObject)Instantiate(boomPar, transform.position, transform.rotation);
                NetworkServer.Spawn(par);
                Destroy(par, 5);
                Destroy(this.gameObject);
                //Network.Destroy(this.gameObject);

            }
        }


    }
    public void findnearplayer()
    {
        float minDist = Mathf.Infinity;
        players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject p in players)
        {
            if (!p.GetComponent<PlayerHealth>().isdead)
            {
                float dist = Vector3.Distance(p.transform.position, transform.position);
                if (dist < minDist)
                {
                    targetPlayer = p.transform;
                    minDist = dist;
                }
            }
            else
            {
                if (targetPlayer == p.transform)
                {
                    minDist = Mathf.Infinity;
                    targetPlayer = null;
                }
            }
        }
    }

    public void Active()
    {
        if (active)
        {
            active = false;
        }
        else
        {
            active = true;
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "SlowArea")
        {
            currentSpeed = speed - (speed * (50f / 100f));
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "SlowArea")
        {
            currentSpeed = speed;
        }
    }
}

