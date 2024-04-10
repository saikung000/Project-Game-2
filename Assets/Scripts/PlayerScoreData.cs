using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreData : MonoBehaviour {
	public PlayerName playerName;

	public PlayerScore playerScore;

	public int num;

	public Text textplayerName,textplayerScore,textnum;


	public Image playerChar;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		textplayerName.text = ""+playerName.playerName;
		textnum.text = ""+num;
		textplayerScore.text = "Score : " + playerScore.score;

	}
}
