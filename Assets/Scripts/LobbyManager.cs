using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour {
	private NetworkLobbyManagerCustom networkLobbyManager;


	public PlayerData playerData;

	public Text textButtonReady;

	

	
	// Use this for initialization
	void Start () {
			//networkManager = NetworkManager.singleton;
			networkLobbyManager = GameObject.Find("NetworkManager").GetComponent<NetworkLobbyManagerCustom>();
	}
	
	// Update is called once per frame
	void Update () {
		if(playerData == null){
			playerData  = GameObject.Find("NetworkManager").GetComponent<PlayerData>();
			
		}else{
			if(playerData.isReady){
				textButtonReady.text = "NOT READY";
			}else {
				textButtonReady.text = "READY";
			}
		}
	}

	public void Ready(){
		if(playerData.isReady){
				playerData.isReady = false;
			}else {
				playerData.isReady = true;
			}
	}
	public void SelectChar(int  Char){
		playerData.playerCharacter =  Char;
	}

	public void StartGame(){
		networkLobbyManager.CheckReadyToBegin();
	}

	
}
