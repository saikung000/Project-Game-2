using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
public class AiController3 : NetworkBehaviour
{
    public Transform core;
    public List<Transform> playerList = new List<Transform>();

    public GameObject[] players;

    public Transform targetPlayer;

    public float RotationSpeed;

    //values for internal use
    private Quaternion B_lookRotation, lookRotation;
    private Vector3 B_direction, direction;

    public int damage = 50;
    NavMeshAgent agent;


    public GameObject bulletPrefab;
    public float bulletspeed;
    public Transform bulletSpawn;

    public float timeBetweenShots = 2.0f;

    public float timestamp = 0.0f;

    public GameObject body;
    public float distanceBomb = 10;

    public float distanceStop = 10;
    public float distanceShoot = 15;
    public float speed;
    private float currentSpeed;

    public bool active;
    public Animator anim;
    public EnemyHealth eh;
    // Use this for initialization
    void Start()
    {
        try{
             anim  = GetComponent<Animator>();
        }catch{

        }
        eh = GetComponent<EnemyHealth>();
        currentSpeed = speed;
        timestamp = Time.time + timeBetweenShots;
        agent = GetComponent<NavMeshAgent>();
        core = GameObject.FindGameObjectWithTag("Core").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.isGameOver)
        {
            agent.Stop();
            return;
        }
        if (!active)
        {
            agent.Stop();
            return;
        }
         if(eh.currentHealth <= 0){
             agent.Stop();
            return;
        }
        agent.speed = currentSpeed;
        findnearplayer();
        if (targetPlayer != null)
        {
            direction = (targetPlayer.position - transform.position).normalized;
            lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * RotationSpeed);
            agent.stoppingDistance = distanceStop;
            agent.SetDestination(targetPlayer.position);
            if (Vector3.Distance(transform.position, targetPlayer.position) < distanceShoot)
            {
                anim.SetBool("Move",false);
                //body.transform.LookAt(targetPlayer.position);

                //find the vector pointing from our position to the target
                B_direction = (targetPlayer.position - body.transform.position).normalized;

                //create the rotation we need to be in to look at the target
                B_lookRotation = Quaternion.LookRotation(B_direction);
               /* if(Time.time > timestamp-2.2){
                    anim.SetTrigger("Shoot");
                }*/
                if (Time.time > timestamp)
                {

                    //print("Shoot");
                     
                    CmdFire();
                    timestamp = Time.time + timeBetweenShots;
                }
                //Network.Destroy(this.gameObject);

            }
            else
            {
                // _direction = ( - body.transform.position).normalized;
                B_lookRotation = Quaternion.LookRotation(transform.forward);
                 anim.SetBool("Move",true);


            }
        }
        else
        {
            direction = (core.position - transform.position).normalized;
            lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * RotationSpeed);
            agent.SetDestination(core.position);
            B_direction = (core.position - body.transform.position).normalized;
            B_lookRotation = Quaternion.LookRotation(B_direction);
             agent.stoppingDistance = distanceBomb;
            if (Vector3.Distance(transform.position, core.position) < distanceBomb)
            {
                CoreHealth ch = core.gameObject.GetComponent<CoreHealth>();
                ch.TakeDamage(200);
                Destroy(this.gameObject);
                //Network.Destroy(this.gameObject);


            }
        }
        body.transform.rotation = Quaternion.Slerp(body.transform.rotation, B_lookRotation, Time.deltaTime * RotationSpeed);


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

    [Command]
    void CmdFire()
    {
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);

        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletspeed;
        bullet.GetComponent<BulletEnemy>().damage = damage;
        NetworkServer.Spawn(bullet);

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
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

