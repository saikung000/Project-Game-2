using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemyFast : MonoBehaviour {
	public GameObject  EffectBomb;

	public float area;
	public int damage;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	/// <summary>
	/// OnTriggerEnter is called when the Collider other enters the trigger.
	/// </summary>
	/// <param name="other">The other Collider involved in this collision.</param>
	void OnTriggerEnter(Collider other)
	{
		Instantiate(EffectBomb,transform.position,transform.rotation);
		 var colls = Physics.OverlapSphere(transform.position,area);
        foreach (Collider enemy in colls)
        {
            if (enemy.tag == "Player")
            {
                var health = enemy.GetComponent<PlayerHealth>();
                if (health != null)
                {
                    health.TakeDamage(damage);
                }
            }
        }

        Destroy(gameObject);
	}
	  void OnCollisionEnter(Collision collision)
    {
		
    }
}
