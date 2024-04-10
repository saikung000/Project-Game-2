using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerLobbyControl : NetworkBehaviour
{
    [SyncVar(hook = "OnNameChanged")]
    public string playerName;

    [SyncVar(hook = "OnCharacterChanged")] public int playerCharacter;

    [SyncVar(hook = "OnReadyChanged")] public bool ready;

    public PlayerData playerData;

    public NetworkLobbyPlayer networkLobbyPlayer;

    public Text textName;

    public Image imageReady, imageChar;
    public Sprite spriteReady, spriteNotready, spriteChar1, spriteChar2;

    public PlayerLobby playerlobby;
    // Use this for initialization

    public override void OnStartLocalPlayer()
    {

    }


    void Start()
    {
        	
        /*	
            playerData  = GameObject.Find("NetworkManager").GetComponent<PlayerData>();
            if(playerData != null){
                SetPlayerName();
                textName.text = ""+playerName;
            }
           */

    }

    // Update is called once per frame
    void Update()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "menu" && transform.parent != GameObject.Find("PanelPlayerLobby").transform)
        {
            transform.parent = GameObject.Find("PanelPlayerLobby").transform;
            transform.localScale = Vector3.one;
            
           
        }
        /*	
			if(playerData != null){
			ready = playerData.isReady;
			
			}
		*/
        if (playerlobby == null)
        {

            Destroy(this.gameObject);
        }
        textName.text = "" + playerName;
        if (ready)
        {
            imageReady.sprite = spriteReady;
            //textReady.text = "Ready";
        }
        else
        {
            imageReady.sprite = spriteNotready;
            //textReady.text = "Not Ready";
        }
        switch (playerCharacter)
        {
            case 0:
                {
                    imageChar.sprite = spriteChar1;
                    //textPlayerChar.text  = "Char 1";
                }
                break;
            case 1:
                {
                    imageChar.sprite = spriteChar2;
                    //textPlayerChar.text  = "Char 2";
                }
                break;
            case 2:
                {
                    //textPlayerChar.text  = "Char 3";
                }
                break;
        }
    }
    void OnNameChanged(string value)
    {
        playerName = value;
        textName.text = "" + playerName;
        //playerName = gameObject.name;
    }

    void OnCharacterChanged(int value)
    {
        playerCharacter = value;
        switch (value)
        {
            case 0:
                {
                    imageChar.sprite = spriteChar1;
                    //textPlayerChar.text  = "Char 1";
                }
                break;
            case 1:
                {
                    imageChar.sprite = spriteChar2;
                    //textPlayerChar.text  = "Char 2";
                }
                break;
            case 2:
                {
                    //textPlayerChar.text  = "Char 3";
                }
                break;
        }
    }
    void OnReadyChanged(bool value)
    {
        ready = value;

        if (value)
        {
            imageReady.sprite = spriteReady;
            //textReady.text = "Ready";
        }
        else
        {
            imageReady.sprite = spriteNotready;
            //textReady.text = "Not Ready";
        }
    }


    [Client]
    void SetPlayerName()
    {
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
        playerName = name;
        textName.text = "" + playerName;
    }
}
