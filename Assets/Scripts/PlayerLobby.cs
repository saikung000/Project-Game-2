using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerLobby : NetworkBehaviour {
	[SyncVar (hook = "OnNameChanged")] 
	public string playerName;

	[SyncVar (hook = "OnCharacterChanged")] public int playerCharacter;

	[SyncVar (hook = "OnReadyChanged")] public bool ready;

	public PlayerData playerData;

	public NetworkLobbyPlayer networkLobbyPlayer;

	public  GameObject PlayerLobbylist;

	public  PlayerLobbyControl playerLobbyControl;

	public  NetworkLobbyManagerCustom network;
	// Use this for initialization
	void Awake() {

        DontDestroyOnLoad(transform.gameObject);
    }
	void Start () {
		 GameObject  playerlobbylist 	= Instantiate(PlayerLobbylist);
	  	playerLobbyControl = playerlobbylist.GetComponent<PlayerLobbyControl>();
		  playerLobbyControl.playerlobby = GetComponent<PlayerLobby>();
		networkLobbyPlayer = GetComponent<NetworkLobbyPlayer>();
		network = GameObject.Find("NetworkManager").GetComponent<NetworkLobbyManagerCustom>();
		network.joinFinish = true;
	}

	public override void OnStartLocalPlayer(){
		playerData  = GameObject.Find("NetworkManager").GetComponent<PlayerData>();
		playerData.playerlobby = GetComponent<PlayerLobby>();
		networkLobbyPlayer = GetComponent<NetworkLobbyPlayer>();
        SetPlayerName();
		SetPlayerReady();
		//SetPlayerChar();
	 
    }
	
	// Update is called once per frame
	void Update () {
		if(playerData !=null){
			if(isLocalPlayer){
			SetPlayerReady();
			SetPlayerChar();
			}
		 }
		 playerLobbyControl.playerName = this.playerName;
		 playerLobbyControl.ready = this.ready;
		 playerLobbyControl.playerCharacter = this.playerCharacter;
		
	}

	void OnNameChanged(string value){
	playerName = value;
	
	//playerName = gameObject.name;
	}
	void OnCharacterChanged(int value){
		
		playerCharacter = value;
		//print(value+"/"+ playerCharacter);
		
	}
	void OnReadyChanged(bool value){
	//	print(value + playerName);
		ready = value;
		networkLobbyPlayer.readyToBegin = value;
		
	}
	

  ///------------------------------/// 
	[Client]
    void SetPlayerReady(){
        ready = playerData.isReady;
        CmdReadyToServer(ready);
    }

	[Command]
	void CmdReadyToServer(bool readyToSend)
	{
			RpcSetReady(readyToSend);
	}
 
	[ClientRpc]
	void RpcSetReady(bool isReady)
	{
		ready  = isReady;
		
	}
 ///------------------------------/// 
	[Client]
    void SetPlayerName(){
        playerName = playerData.name;
        CmdSendNameToServer(playerName);
    }

	[Command]
	void CmdSendNameToServer(string nameToSend)
	{
			RpcSetPlayerName(nameToSend);
	}
 
	[ClientRpc]
	void RpcSetPlayerName(string name)
	{
		playerName  = name;
		
	}

  ///------------------------------/// 
	[Client]
    void SetPlayerChar(){
         playerCharacter = playerData.playerCharacter;
       CmdSendCharToServer(playerCharacter);
	   
	   network.SetPlayerTypeLobby(networkLobbyPlayer.playerControllerId, playerCharacter);
	  // print(""+ networkLobbyPlayer.playerControllerId+ "type"+playerCharacter);
    }

	[Command]
	void CmdSendCharToServer(int characterToSend)
	{
			RpcSetPlayerChar(characterToSend);
	}
 
	[ClientRpc]
	void RpcSetPlayerChar(int character)
	{
		playerCharacter  = character;
		
	}
}
