using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class HostGame : MonoBehaviour {
	[SerializeField]
	private uint roomSize = 3;
	[SerializeField]
	private Text  loadingtext;
	private string roomName;

	private NetworkManager networkManager;

	public Text textRoomName;

	public UIMenuManager uiMenuManager;

	public NetworkLobbyManagerCustom network;

	public GameObject panalLoading;
	public bool join ;
	// Use this for initialization
	void Start () {
		network = GameObject.Find("NetworkManager").GetComponent<NetworkLobbyManagerCustom>();
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		networkManager = NetworkManager.singleton;
		if(networkManager.matchMaker == null){
			networkManager.StartMatchMaker();

		}
	}
	
	// Update is called once per frame
	void Update () {
		if(network.joinFinish){
			
			uiMenuManager.GoLobby();
			panalLoading.SetActive(false);
			join = true;		
		}
	}

	public void SetRoomName(string name){

		roomName = name;
	}

	public void CreateRoom(){
		if(roomName != "" && roomName != null){
			networkManager.StartMatchMaker();
			networkManager = NetworkManager.singleton;
			network.lobbyScene = "menu";
			//Create room
			networkManager.matchMaker.CreateMatch(roomName,roomSize,true,"","","",0,0,networkManager.OnMatchCreate);
			textRoomName.text = roomName;
			//uiMenuManager.GoLobby();
			StartCoroutine(WaitForJoin());
			panalLoading.SetActive(true);
		}

	}

	IEnumerator WaitForJoin ()
	{
		int countdown = 20;
		while (countdown > 0)
		{
			 loadingtext.text = "JOINING... ";
			
			yield return new WaitForSeconds(1);
		
			countdown--;
		}
		
		if(join != true){
			// Failed to connect
			loadingtext.text = "Failed to connect.";
			yield return new WaitForSeconds(1);
			uiMenuManager.backServerList();
			panalLoading.SetActive(false);
			network.joinFinish = false;
			join = false;
			MatchInfo matchInfo = networkManager.matchInfo;
			if (matchInfo != null)
			{
				networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
				networkManager.StopHost();
			}
			
			
		}
	}
}
