using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSkill1 : MonoBehaviour {
	public GameObject player;
	public int damage;

	public GameObject SpawnDamage;
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
		
		var bullet = (GameObject)Instantiate(SpawnDamage,transform.position,Quaternion.identity);	
		bullet.GetComponent<AreaDamage>().damage = this.damage;
        bullet.GetComponent<AreaDamage>().player = this.player;
        Destroy(gameObject);
	}
	  void OnCollisionEnter(Collision collision)
    {
		
    }
}
