using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
public class AiController : NetworkBehaviour
{
    public int damage;
    public Transform core;
    NavMeshAgent agent;
    public float distance = 10;


    public float speed;
    private float currentSpeed;

    public bool active;
    public Animator anim;

    public EnemyHealth eh;

     public GameObject boomPar;
    // Use this for initialization
    void Start()
    {
        try{
             anim  = GetComponent<Animator>();
        }catch{

        }
        eh = GetComponent<EnemyHealth>();
        currentSpeed = speed;
        agent = GetComponent<NavMeshAgent>();
        core = GameObject.FindGameObjectWithTag("Core").transform;
    }

    // Update is called once per frame
    void Update()
    {
       if(GameManager.isGameClear || GameManager.isGameOver){
     
            agent.Stop();
            return;
        }
        if (!active )
        {
            agent.Stop();
            return;
        }
        if(eh.currentHealth <= 0){
             agent.Stop();
            return;
        }
        agent.speed = currentSpeed;
        agent.SetDestination(core.position);

        if (Vector3.Distance(transform.position, core.position) < distance)
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
