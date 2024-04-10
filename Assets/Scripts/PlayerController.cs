using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    public int character;

    public UISkillManager UISkillManager;

    public UISkill2Manager UISkill2Manager;
    public GameObject camera;
    public Camera cam;

    public Rigidbody rgb;

    public GameObject body;
    [SerializeField]
    public float speed, speedMouse, jumpPower;
    public bool grounded;

    public PlayerHealth playerHealth;

    public Animator anim;

    public bool canMove = true;
    // Use this for initialization

    //เพิ่ม
    float vertical, horizontal;

    public Vector3 aimingTargetPosition;
    public Vector3 BoneLookAtOffset;
    public Transform upperBody;
    public GameObject AimingPoint;

    public GameObject  root;

    public bool look = true;
   
    void OnStartLocalPlayer()
    {


    }
    void Start()
    {
        body.transform.position = root.transform.position;
        anim = GetComponent<Animator>();
        if (GameObject.FindGameObjectWithTag("MainCamera") != null)
        {
            GameObject.FindGameObjectWithTag("MainCamera").SetActive(false);
        }
        playerHealth = GetComponent<PlayerHealth>();
        rgb = GetComponent<Rigidbody>();
        if (!isLocalPlayer)
        {
            cam.enabled = false;
            camera.SetActive(false);
        }
        canMove = true;

    }
    void LateUpdate()
    {


        if(look){
        upperBody.LookAt(AimingPoint.transform.position);
        BoneLookAtOffset.z = -90;
        upperBody.Rotate(BoneLookAtOffset);
        }



    }
    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {

            return;
        }


         if (GameManager.isGameOver  || GameManager.isGameClear)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            transform.Translate(Vector3.zero);
            transform.Rotate(Vector3.zero);
            anim.SetFloat("inputH", 0);
            anim.SetFloat("inputV", 0);
            return;
        }
        if (PauseMenu.isOn)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            transform.Translate(Vector3.zero);
            transform.Rotate(Vector3.zero);
            body.transform.Rotate(Vector3.zero);
            anim.SetFloat("inputH", 0);
            anim.SetFloat("inputV", 0);
            return;
        }
        if (character == 0)
        {
            if (UISkillManager.UpSkill)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                transform.Translate(Vector3.zero);
                transform.Rotate(Vector3.zero);
                body.transform.Rotate(Vector3.zero);
                anim.SetFloat("inputH", 0);
                anim.SetFloat("inputV", 0);
                return;
            }
        }
        else if (character == 1)
        {
            if (UISkill2Manager.UpSkill && character == 1)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                transform.Translate(Vector3.zero);
                transform.Rotate(Vector3.zero);
                body.transform.Rotate(Vector3.zero);
                anim.SetFloat("inputH", 0);
                anim.SetFloat("inputV", 0);
                return;
            }
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerHealth.isdead)
        {
            anim.SetFloat("inputH", 0);
            anim.SetFloat("inputV", 0);
            return;
        }

        
        transform.Rotate(new Vector3(0f, Input.GetAxis("Mouse X"), 0f) * speedMouse);
        body.transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y"), 0f, 0f) * speedMouse);
        if (!canMove)
        {
            anim.SetFloat("inputH", 0);
            anim.SetFloat("inputV", 0);
            return;
        }
        /* 
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * speed;
		
        transform.Translate(x,0,z);
        */


        //Jump
        if (Input.GetButtonDown("Jump") && grounded == true)
        {
            //rgb.AddForce(transform.up*jumpPower);
            rgb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
            grounded = false;
        }

        /*
     if (Input.GetKey(KeyCode.Escape))
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
     #else
     Application.Quit();
     #endif
    }
    */


        BoneLookAtOffset.z = -90;
        upperBody.Rotate(BoneLookAtOffset);
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        anim.SetFloat("inputH", horizontal);
        anim.SetFloat("inputV", vertical);

        /*if (Input.GetAxis("Mouse X") > 0)
            body.transform.eulerAngles += new Vector3(0, speedMouse, 0);
        if (Input.GetAxis("Mouse X") < 0)
            body.transform.eulerAngles += new Vector3(0, -speedMouse, 0);
        if (Input.GetAxis("Mouse Y") > 0)
           body.transform.eulerAngles += new Vector3(-speedMouse, 0, 0);
        if (Input.GetAxis("Mouse Y") < 0)
            body.transform.eulerAngles += new Vector3(speedMouse, 0, 0);
			*/


    }
    
    void OnCollisionStay(Collision other)
    {
        grounded = true;
    }
 
    void OnCollisionExit(Collision other)
    {
        grounded = false;
    }


    
}
