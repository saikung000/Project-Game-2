using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class BulletEnemyFollow : NetworkBehaviour
{

    public Vector3 location;
    public Vector3 velocity;
    public Vector3 acceleration;
    public float maxSpeed;
    public float maxForce;
    public GameObject[] players;
    public GameObject breakPar;

    public Transform targetPlayer;
    public int damage;

    public float nearplayer;

    public bool near = false;

    public Vector3 startVelocity;

    public Transform player;

    //[SyncVar(hook = "OnPlayerChange")]
    public string PlayerOject = "";



    void OnPlayerChange(string name)
    {

        PlayerOject = name;
    }



    // Use this for initialization
    void Start()
    {
        location = transform.position;
        velocity = startVelocity;
       Cmdfindplayer();
        Destroy(this.gameObject, 20);
    }


    void Update()
    {
        if (GameManager.isGameClear || GameManager.isGameOver)
        {
            Destroy(this.gameObject);
            return;
        }
      //  CmdSendplayerPosition();
        if (targetPlayer != null)
        {
            if (!near)
            {
                if (Vector3.Distance(this.transform.position, targetPlayer.transform.position) > nearplayer)
                {
                    //transform.LookAt(targetPlayer);
                    /* 
                    Vector3 dir = (targetPlayer.transform.position - transform.position).normalized * maxSpeed;
                    GetComponent<Rigidbody>().velocity = dir;
                    */
                    Steering(targetPlayer.transform.position);
                    Steer();

                }
                else
                {
                    near = true;
                }
            }
            else
            {

                Steer();
            }
            /*float amtToMove = maxForce * Time.deltaTime;
            transform.Translate(Vector3.forward* maxSpeed * amtToMove);
            */

        }
    }

    public void Steer()
    {

        velocity += acceleration;
        location += velocity * Time.deltaTime;
        transform.position = location;

        Debug.DrawLine(transform.position, transform.position + velocity * 4, Color.blue);

        Vector3 heading;
        heading = velocity.normalized;
        transform.rotation = Quaternion.LookRotation(new Vector3(heading.x, heading.y, heading.z));

        //transform.LookAt(velocity.normalized);
        acceleration *= 0;

    }

    public void Steering(Vector3 target)
    {

        Vector3 desired = target - transform.position;

        desired.Normalize();


        desired *= maxSpeed;
        Vector3 steer = desired - velocity;

        steer.Normalize();
        steer *= maxForce;

        ApplyForce(steer);

    }
    public void ApplyForce(Vector3 force)
    {
        acceleration += force;
    }
    void OnTriggerEnter(Collider other)
    {
        var hit = other.gameObject;
        if (hit.tag == "Player" || hit.tag == "Ground")
        {

            var health = hit.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
            print("destroy");
            var followPar = (GameObject)Instantiate(breakPar, transform.position, breakPar.transform.rotation);

            Destroy(followPar, 10);
            NetworkServer.Spawn(followPar);
            Destroy(gameObject);

        }
    }

    [Command]
    public void CmdSendplayerPosition()
    {

        //Rpcfindnearplayer();

    }

    [Command]
    public void Cmdfindplayer()
    {

        players = GameObject.FindGameObjectsWithTag("Player");
        int randomPlayer = Random.Range(0, players.Length);
        Rpcfindnearplayer(players[randomPlayer]);
       // player =  players[randomPlayer].transform;

    }
    [ClientRpc]
    public void Rpcfindnearplayer(GameObject target)
    {
        targetPlayer = target.transform;
    }
}

