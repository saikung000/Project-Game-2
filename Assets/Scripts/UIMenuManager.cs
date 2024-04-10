using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuManager : MonoBehaviour {
	public  enum State {
        Menu,
        Lobby,
        ServerList,
    }

	public State state;
	public GameObject panalMenu,panalSetName,panalLobby,panalServerList;

	public Text textname;
	public PlayerData playerData;
	public static bool setName = false; 
	public InputField inputName;

	public NetworkLobbyManagerCustom network;

	public GameObject panalOption;
	public Slider  sliderSound,sliderFx;

	// Use this for initialization
	void Start () {
		//setName = false; 
		state = State.Menu;
		network = GameObject.Find("NetworkManager").GetComponent<NetworkLobbyManagerCustom>();
		sliderSound.value = SoundControl.sound /100f;
		sliderFx.value = SoundControl.fx / 100f;
		panalOption.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if(setName){
			panalSetName.SetActive(false);
		}else {
			panalSetName.SetActive(true);
		}
		if(playerData == null){
			playerData  = GameObject.Find("NetworkManager").GetComponent<PlayerData>();
			inputName.text = playerData.name;
		}else{
			playerData.name = inputName.text;
			textname.text = "" +playerData.name ; 
		}
		SoundControl.sound = sliderSound.value*100f;
		SoundControl.fx = sliderFx.value *100f;
		switch(state){
			case State.Menu :{
				panalMenu.SetActive(true);
				panalLobby.SetActive(false);
				panalServerList.SetActive(false);
			}break;
			case State.ServerList :{
				panalMenu.SetActive(false);
				panalLobby.SetActive(false);
				panalServerList.SetActive(true);
				playerData.isReady = false;
			}break;
			case State.Lobby :{
				panalMenu.SetActive(false);
				panalLobby.SetActive(true);
				panalServerList.SetActive(false);
			}break;


		}
	}
	
	public void GoServerList (){
		state = State.ServerList;
	}

	public void backMenu(){
		state = State.Menu;
	}
	public void backServerList(){

		state = State.ServerList;

		network.joinFinish = false;
		
	}
	public void GoLobby(){
		state = State.Lobby;
	}
	public void ExitGame(){
		print("Quit");
		 Application.Quit();
	}

	public void SetName( bool t){
		setName = t;
		 

	}

	public void Option(){
		panalOption.SetActive(true);
	}
}
