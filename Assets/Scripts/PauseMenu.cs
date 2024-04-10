using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{

    public static bool isOn = false;

    private NetworkManager NetworkManager;

    public PlayerData playerData;

    public NetworkLobbyManagerCustom network;
    // Use this for initialization
    void Start()
    {
        isOn = false;
        NetworkManager = NetworkManager.singleton;
        playerData = GameObject.Find("NetworkManager").GetComponent<PlayerData>();
        network = GameObject.Find("NetworkManager").GetComponent<NetworkLobbyManagerCustom>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LeaveRoom()
    {
        try
        {
            MatchInfo matchInfo = NetworkManager.matchInfo;
            NetworkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, NetworkManager.OnDropConnection);

        }
        catch
        {

        }

        try
        {
            playerData.isReady = false;
            network.joinFinish = false;
        }
        catch
        {

        }
        NetworkManager.StopHost();
        playerData.isReady  = false;
        
        SceneManager.LoadScene("menu");
    }
    public void Continue()
    {

        isOn = false;
    }
}
