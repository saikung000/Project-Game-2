using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class PlayerName : NetworkBehaviour
{
    [SyncVar(hook = "OnNameChanged")]
    public string playerName;

    public SpriteRenderer mapIcon;
    public Sprite spriteYou;
    [SyncVar(hook = "OnCharacterChanged")] public int playerCharacter;

    public PlayerData playerData;

    public Text textName;
    public GameObject CanvasSkill;
    // Use this for initialization
    void Start()
    {
        //transform.localScale = Vector3.one;
        if (isLocalPlayer)
        {
            mapIcon.sprite = spriteYou;
        }
        else
        {
            CanvasSkill.SetActive(false);
        }
    }

    public override void OnStartLocalPlayer()
    {
        playerData = GameObject.Find("NetworkManager").GetComponent<PlayerData>();
        SetPlayerName();
    }
    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            //	if(playerData == null){
            playerData = GameObject.Find("NetworkManager").GetComponent<PlayerData>();

            //}
            //else {

            //playerName = playerData.name ;
            //OnNameChanged(playerData.name);
            OnCharacterChanged(playerData.playerCharacter);
            //playerCharacter = playerData.playerCharacter;	

            //}

        }
        else
        {
            gameObject.name = playerName;

        }
        textName.text = "" + playerName;

    }
    void OnNameChanged(string value)
    {
        playerName = value;
        gameObject.name = value;
        //playerName = gameObject.name;
    }

    void OnCharacterChanged(int value)
    {
        playerCharacter = value;
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
        gameObject.name = name;

    }
}
