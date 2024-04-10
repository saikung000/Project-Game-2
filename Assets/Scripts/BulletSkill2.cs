using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSkill2 : MonoBehaviour {
	public GameObject player;
	public int damage;

	public GameObject target;

	public LayerMask layerMask;
	// Use this for initialization
	void Start () {
		FindNearFrontPlayer();
		if(target !=null){
			transform.LookAt(target.transform);
			  Vector3 dir  =(target.transform.position - transform.position).normalized * 40;
			  GetComponent<Rigidbody>().velocity = dir;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(target !=null){
			transform.LookAt(target.transform);
			  Vector3 dir  =(target.transform.position - transform.position).normalized * 40;
			  GetComponent<Rigidbody>().velocity = dir;
		}
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
			if(hit.tag == "Enemy"){
			var health = hit.GetComponent<EnemyHealth>();
				if (health  != null)
				{
					health.TakeDamage(damage,player);
				}
			}
        Destroy(gameObject);
	}

	public void FindNearFrontPlayer(){
		/*float minDist = Mathf.Infinity;
		GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");

		foreach(GameObject p in enemy) {
				float dist = Vector3.Distance(p.transform.position, transform.position);
					if (dist < minDist)
					{
						target = p;
						minDist = dist;
					}
			
			}*/

			RaycastHit hit;

			Vector3 p1 = transform.position;
			if(Physics.SphereCast(p1,7,transform.forward,out hit  ,Mathf.Infinity, layerMask)){
				if(hit.transform.tag == "Enemy"){
				target = hit.collider.gameObject;
				}
			}
	}
	
}
