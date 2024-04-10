using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDataControl : MonoBehaviour {
	public PlayerName playerName;

	public PlayerHealth playerHealth;

	public Text textplayerName;
	public Image playerHpBar;

	public Text playerChar;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(playerName != null && playerHealth != null){
		textplayerName.text = ""+playerName.playerName;
		playerHpBar.fillAmount = (float)playerHealth.currentHealth / (float)playerHealth.Health;
		}else {
			Destroy(this.gameObject);
		}
	}
}
