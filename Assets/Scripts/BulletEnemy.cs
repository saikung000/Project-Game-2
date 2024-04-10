﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour {

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
		var hit = other.gameObject;
			if(hit.tag == "Player"){
			var health = hit.GetComponent<PlayerHealth>();
				if (health  != null)
				{
					health.TakeDamage(damage);
				}
			}
        Destroy(gameObject);
	}
	  void OnCollisionEnter(Collision collision)
    {
		
    }
}
