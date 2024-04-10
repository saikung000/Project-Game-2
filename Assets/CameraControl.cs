using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
	public GameObject  normalposition,zoomoutposition;
    public GameObject DestinationObject;
    public float Speed = 10;
    // Use this for initialization
    void Start()
    {
DestinationObject = normalposition;
    }

    void Update()
    {
		if(Input.GetKeyDown(KeyCode.Z)){
			zoom();
		}
        if (DestinationObject)
        {
            // Interpolate from current position to the destination object position and orientation
            Vector3 newposition = Vector3.Lerp(transform.position, DestinationObject.transform.position, Time.deltaTime * Speed);
            Quaternion newrotation = Quaternion.Slerp(transform.rotation, DestinationObject.transform.rotation, Time.deltaTime * Speed);
            transform.position = newposition;
            transform.rotation = newrotation;
        }
    }
	public void zoom(){
		if(DestinationObject == normalposition){
			DestinationObject = zoomoutposition;
		}else {
			DestinationObject = normalposition;
		}
	}
    public void normal (){
        DestinationObject = normalposition;
    }
}
