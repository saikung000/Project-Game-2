using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class NetworkLobbyManagerCustom :  NetworkLobbyManager  {
		 public int chosenCharacter = 0;
		 public PlayerData playerdata;
		 	public GameObject playerObject;

        public bool  joinFinish = false;

        public  string lobbyscene;

        
     //subclass for sending network messages
     

   
	 Dictionary<int, int> currentPlayers = new Dictionary<int,int>();
	public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
    {
        if(!currentPlayers.ContainsKey(conn.connectionId))
            currentPlayers.Add(conn.connectionId, 0);

        return base.OnLobbyServerCreateLobbyPlayer(conn, playerControllerId);
    }

    public void SetPlayerTypeLobby(short conn, int _type)
    {
        if (currentPlayers.ContainsKey(conn)){
            currentPlayers[conn] = _type;
			//print(""+conn + "type"+_type);
		}

    }
	
    public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
    {
		//SetPlayerTypeLobby(conn,chosenCharacter);
        int index = currentPlayers[conn.connectionId];
        Debug.Log("server add with message "+ chosenCharacter);
        GameObject _temp = (GameObject)GameObject.Instantiate(spawnPrefabs[index],
            startPositions[conn.connectionId].position,
            Quaternion.identity);

       // NetworkServer.AddPlayerForConnection(conn, _temp, playerControllerId);

        return _temp;
    }

     
       public override void OnStopClient()
     {
          currentPlayers.Clear();
            playerdata.isReady = false;
          lobbyScene = ""; // Ensures we don't reload the scene after quitting
     }
     public override void OnStopServer()
     {
         currentPlayers.Clear();
         playerdata.isReady = false;
         lobbyScene = ""; // Ensures we don't reload the scene after quitting
     }

	void Start () {
		playerdata  = GetComponent<PlayerData>();
        lobbyScene =  lobbyscene;
		
	}
        public void Update()
        {
          //  Debug.Log("UpdatePC");
            if (playerdata.playerCharacter == 0)
            {
              // Debug.Log("player1");
                chosenCharacter = 0;
				
				
				//playerObject = playerdata.playerObject;
            }
            else  if (playerdata.playerCharacter == 1)
            {
             //   Debug.Log("player2");
                chosenCharacter = 1;
				//playerObject= playerdata.playerObject;
            }
			 else  if (playerdata.playerCharacter == 2)
            {
                 
                 chosenCharacter = 2;
				 //playerObject = playerdata.playerObject;
            }

        }
    }