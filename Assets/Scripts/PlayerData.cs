using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
public class PlayerData : MonoBehaviour {
	
	//[SyncVar (hook = "OnNameChanged")]
	 public string name;

//	[SyncVar (hook = "OnCharacterChanged")] 
	public int playerCharacter;

//	[SyncVar (hook = "OnReadyChanged")] 
	public bool isReady = false;

	public GameObject playerObject;

	public GameObject  playerPrefab1,playerPrefab2,playerPrefab3;

	public NetworkLobbyManagerCustom networkLobbyManager;

	public PlayerLobby playerlobby;
	public bool  backMenu;
	

	//public InputField inputName;
	// Use this for initialization
	void Start () {
		//print("playerdata");
		 
		networkLobbyManager =  GetComponent<NetworkLobbyManagerCustom>();
		
		
		
	}
	
	// Update is called once per frame
	void Update () {
			 Scene scene = SceneManager.GetActiveScene();
			if(scene.name == "test" ){
				
			}
			if(playerlobby != null){
				playerlobby.ready = isReady;
				playerlobby.playerCharacter = playerCharacter;		
			}
			switch(playerCharacter){
			case 0 : {
				
				playerObject  = playerPrefab1;
			} 
			break;
			case 1 : {
				playerObject  = playerPrefab2;
			} 
			break;
			case 2 : {
				playerObject  = playerPrefab3;
			} 
			break;
		}
			
	}
	void OnNameChanged(string value){
	name = value;
	}

	void OnCharacterChanged(int value){
		playerCharacter = value;
	}
	void OnReadyChanged(bool value){
		isReady = value;
	}
		// called when a client connects 
	
	
	
}
