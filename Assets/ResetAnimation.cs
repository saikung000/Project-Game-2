using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimation : MonoBehaviour {

	public Animator[] anim;
	// Use this for initialization
	void Start () {
		anim =  GetComponentsInChildren<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	
	void OnEnable()
	{
		
		resetanim();
	}
	public void resetanim(){
		foreach (Animator ani  in anim)
		{
			ani.Play("",0,0);
		}
	}
}
