using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking.Match;
public class RoomListItem : MonoBehaviour {
	public delegate void JoinRoomDelegate(MatchInfoSnapshot _match);
	private JoinRoomDelegate joinRoomCallback;

	private MatchInfoSnapshot match;
	public Button joinbtn;
	public Text  roomNameText;
	// Use this for initialization
	void Start () {
			this.transform.localScale = Vector3.one;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void Setup (MatchInfoSnapshot _match, JoinRoomDelegate _joinRoomCallback)
	{
		match = _match;
		joinRoomCallback = _joinRoomCallback;

		roomNameText.text = match.name + " (" + match.currentSize + "/" + match.maxSize + ")";
    }
	public void JoinRoom ()
	{
		joinRoomCallback.Invoke(match);
	}
}
