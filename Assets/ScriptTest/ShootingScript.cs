using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingScript : MonoBehaviour {
    public GameObject BlackArrow;
    public float Related = 0.4f;
    public float Relate = 0f;
    public Animator anim;
	// Use this for initialization
	void Start () {
		
	}
	void Shooting()
    {
        print("Shooted");
        Vector3 shootHere = transform.position;
        shootHere.z += 2;
        GameObject Shooting = Instantiate(BlackArrow, transform.position, Quaternion.identity);
        
        Shooting.GetComponent<Rigidbody>().velocity = transform.forward *60;
        Shooting.transform.forward = transform.forward;
        Shooting.transform.Rotate(0, 90, 0);

        Destroy(Shooting.gameObject, 3);
    }
	// Update is called once per frame
	void Update () {
		if(Input.GetButton("Fire1"))
        {
            if(Time.time > Relate)
            {
                Relate = Time.time + Related;
                Shooting();
            }
            
        }
        anim.SetFloat("inputH", Input.GetAxis("Horizontal"));
        anim.SetFloat("inputV", Input.GetAxis("Vertical"));
	}
}
