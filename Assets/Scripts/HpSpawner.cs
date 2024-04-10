using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HpSpawner : NetworkBehaviour{
	public GameObject hpPack;

	public GameObject hpPackCurrent;
	public float timer;
	public float timeSpawn;
	public Transform spawnPoint;
	public bool waitSpawn = true;
	// Use this for initialization
	void Start () {
		
	}
	public override void OnStartServer()
    {

       timer = timeSpawn;
	   waitSpawn = true; 

    }
	// Update is called once per frame
	void Update () {
		if (!isServer)
        {
        return;           
        }
		if(hpPackCurrent == null && !waitSpawn){
			timer = timeSpawn;
			waitSpawn = true; 
		}
		if(waitSpawn){
			timer -= Time.deltaTime;
			if(timer <=0){
				SpawnHp();
			}
		}
	}
	public void SpawnHp(){
		    var HpPack = (GameObject)Instantiate(hpPack, spawnPoint.position, spawnPoint.rotation);
                NetworkServer.Spawn(HpPack);
				hpPackCurrent = HpPack;
				waitSpawn = false; 
	}
}
