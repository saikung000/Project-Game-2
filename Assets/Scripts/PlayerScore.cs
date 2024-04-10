using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerScore : NetworkBehaviour  {
	[SyncVar(hook = "OnScoreChange")]
	public int score;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	 public void GetScore(int amount)
    {
		if (!isServer)
		{
		return;
		}
		score += amount;
       
    }
	 void OnScoreChange(int score){
        //print(bullet);
        this.score =  score;
    }
}
