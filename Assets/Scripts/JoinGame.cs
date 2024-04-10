using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Collections;
using UnityEngine.UI;
public class JoinGame : MonoBehaviour {

	List<GameObject> roomList = new List<GameObject>();

	[SerializeField]
	private Text status , loadingtext;

	public GameObject panalLoading;

	[SerializeField]
	private GameObject roomListItemPrefab;

	[SerializeField]
	private Transform roomListParent;

	private NetworkManager networkManager;

	public UIMenuManager uiMenuManager;
	public bool join = false;
	public Text textRoomName;

	public GameObject  buttonStart;

	public NetworkLobbyManagerCustom network;
	
	// Use this for initialization
	void Start () {
		network = GameObject.Find("NetworkManager").GetComponent<NetworkLobbyManagerCustom>();
		networkManager = NetworkManager.singleton;
		if (networkManager.matchMaker == null)
		{
			networkManager.StartMatchMaker();
		}

		RefreshRoomList();
		join = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(network.joinFinish){
			uiMenuManager.GoLobby();
			panalLoading.SetActive(false);		
			join = true;
		}
	}

	public void RefreshRoomList ()
	{
		ClearRoomList();

		if (networkManager.matchMaker == null)
		{
			networkManager.StartMatchMaker();
		}

		networkManager.matchMaker.ListMatches(0, 20, "", true, 0, 0, OnMatchList);
		status.text = "Loading...";
	}
	void ClearRoomList()
	{
		for (int i = 0; i < roomList.Count; i++)
		{
			Destroy(roomList[i]);
		}

		roomList.Clear();
	}

	public void OnMatchList (bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
	{
		status.text = "";

		if (!success || matchList == null)
		{
			status.text = "Couldn't get room list.";
			return;
		}

		foreach (MatchInfoSnapshot match in matchList)
		{
			GameObject roomListItemGO = Instantiate(roomListItemPrefab);
			roomListItemGO.transform.SetParent(roomListParent);

			RoomListItem roomListItem = roomListItemGO.GetComponent<RoomListItem>();
			if (roomListItem != null)
			{
				roomListItem.Setup(match, JoinRoom);
			}

			
			// as well as setting up a callback function that will join the game.

			roomList.Add(roomListItemGO);
		}

		if (roomList.Count == 0)
		{
			status.text = "No rooms at the moment.";
		}
	}
	public void JoinRoom (MatchInfoSnapshot _match)
	{
		networkManager.StartMatchMaker();
		networkManager = NetworkManager.singleton;
		network.lobbyScene = "menu";
		networkManager.matchMaker.JoinMatch(_match.networkId, "", "", "", 0, 0, OnMatchJoined);
		StartCoroutine(WaitForJoin());
		panalLoading.SetActive(true);
		textRoomName.text = _match.name;
	}
	 
	
	 public  void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        if (success)
        {
            Debug.Log("Match Joined");
            MatchInfo hostInfo = matchInfo;
            NetworkManager.singleton.StartClient(hostInfo);
            OnConnect();
        }
        else
        {
            Debug.Log("ERROR : Match Join Failure");
			uiMenuManager.backServerList();
			join = false;
        }

    }

	 void OnConnect ()
    {
			uiMenuManager.panalLobby.SetActive(true);
        if (Network.isServer)
        {
            Debug.Log("You are Server");
		//	StartCoroutine(WaitToLobby ());
			buttonStart.SetActive(true);
        }
        else if (Network.isClient)
        {
            Debug.Log("You are Client");
		//	StartCoroutine(WaitToLobby ());
			buttonStart.SetActive(true);
			
			buttonStart.GetComponent<Button>().interactable = false;
        }
        else
        {
           Debug.Log("ERROR : MatchMaking Failed, Peer type is neither Client nor Server");
		//	StartCoroutine(WaitToLobby ());
			buttonStart.SetActive(true);
			buttonStart.GetComponent<Button>().interactable = false;
        }
    }
	
	public void Disconected(){
			MatchInfo matchInfo = networkManager.matchInfo;
			if (matchInfo != null)
			{
				networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
				networkManager.StopHost();
			}
			network.joinFinish = false;
			join = false;
	}
    public void OnConnected(NetworkMessage msg)
    {
        Debug.Log("Connected!");
    }
	IEnumerator WaitForJoin ()
	{
		ClearRoomList();

		int countdown = 20;
		while (countdown > 0)
		{
			 loadingtext.text = "JOINING.... ";
			
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
			
			RefreshRoomList();
		}
	}
	IEnumerator WaitToLobby ()
	{
		
		yield return new WaitForSeconds(4);
		uiMenuManager.GoLobby();
		panalLoading.SetActive(false);		
		join = true;
			
	}

}
