using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject player;
    public GameObject getHitPar;
    public int damage;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        Transform T = other.transform;
        while (T.parent != null) T = T.parent;
		
        var hit = T.gameObject;
        if (hit.tag == "Enemy")
        {
            var health = hit.GetComponent<EnemyHealth>();
            if (health != null)
            {
                health.TakeDamage(damage, player);
            }
        }

        Destroy(gameObject);
        var hitPar = (GameObject)Instantiate(getHitPar, transform.position, transform.rotation);
        Destroy(hitPar, 1);
    }
    void OnCollisionEnter(Collision collision)
    {

    }
}
