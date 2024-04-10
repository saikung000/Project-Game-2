using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSkill3 : MonoBehaviour {
		public GameObject SpawnSkill;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter(Collider other)
	{
		Instantiate(SpawnSkill,transform.position,Quaternion.identity);	
        Destroy(this.gameObject);
	}
}
