using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceControl : MonoBehaviour {
	public AudioSource sound;

	public enum Type{ sound , fx}

	public Type type;
	// Use this for initialization
	void Start () {
		sound  = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if(type == Type.sound){
			sound.volume = SoundControl.sound /100;
		}else if(type == Type.fx){
			sound.volume  = SoundControl.fx / 100;
		}
	}
}
