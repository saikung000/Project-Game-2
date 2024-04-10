using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MovementScript : MonoBehaviour {
    float vertical, horizontal;
    //public CharacterController character;
    
    public Rigidbody rb;
    public float charSpeed;
    public float runSpeed;
    public float jumpSpeed;
    public float speed;
    public float speedMouse;
    public float blinkDistance;
    public float mousex, mousey;

    public Vector3 aimingTargetPosition;
    public Vector3 BoneLookAtOffset;
    public Transform upperBody;
    public GameObject CameraObj;
    public GameObject blinkPar;
    public GameObject firePoint;
    public GameObject gun;
    public GameObject AimingPoint;
    public Animator anim;
    public GameObject GunProject;
    public bool run, blink, jump,flashBombActive,look,isGround;
    
    Vector3 flashBombTarget;
    bool moved = false;
	// Use this for initialization
	void Start () {
        look = true;
        rb = GetComponent<Rigidbody>();
        run = false;
        blink = false;
        jump = false;
        moved = true;
        mousex = 0;
        mousey = 0;
        flashBombActive = false;
        anim = GetComponent<Animator>();
        
        //character = GetComponent<CharacterController>();
	}
	
    public void move()
    {
        
        Vector3 moveDir = transform.TransformDirection(horizontal, 0, vertical);
        if ((vertical != 0 || horizontal != 0) && moved)
        {

            //if (run && this.CompareTag("testChar"))
            //{
            //    transform.localPosition += moveDir * runSpeed * Time.deltaTime;
                
            //}
            //else if (blink&&this.CompareTag("blinkChar"))
            //{
            //    Vector3 blinkTrans = new Vector3(transform.position.x + horizontal * -20f, transform.position.y, transform.position.z + vertical * -20f);
            //    transform.localPosition = Vector3.Lerp(transform.position, blinkTrans, 0.5f);
            //    GameObject bPar =  Instantiate(blinkPar, transform.position, Quaternion.identity) as GameObject;
            //    Destroy(bPar.gameObject, 1);
            //    blink = false;
            //}
            //else
            //{
            //    transform.localPosition += moveDir * charSpeed * Time.deltaTime;

            //}
        }
        

    }
    void cameraSystem()
    {
        
        Vector3 cameraPositionMove = new Vector3(transform.position.x, transform.position.y ,transform.position.z);
        CameraObj.transform.localPosition = cameraPositionMove;
    }
    public void Jump()
    {
        anim.SetTrigger("Jump");
        jump = false;
    }
    public void Run()
    {

    }
    void flashBomb()
    {
        if(flashBombActive)
        {
            transform.localPosition = Vector3.Lerp(transform.position, flashBombTarget, 1f);
            anim.Play("flashSkill", -1);           
            flashBombActive = false;
        }
        
    }
    void setLook()
    {
        if (look)
        {
            look = false;
        }
        else
        {
            look = true;
        }
       
    }
    void setWeight()
    {
        anim.SetLayerWeight(1, 1);
    }
    void spinKick()
    {
        int way = 0;
        anim.SetLayerWeight(1, 0);
        look = false;
        if(vertical>0||(vertical==0&&horizontal==0))
        {
            anim.Play("Dash", 0);
            anim.Play("Dash", 1);
        }
        if (vertical < 0)
        {
            anim.Play("DashBack", 0);
            anim.Play("DashBack", 1);
        }
        if (horizontal > 0)
        {
            anim.Play("DashRight", 0);
            anim.Play("DashRight", 1);
        }
        if (horizontal < 0)
        {
            anim.Play("DashLeft", 0);
            anim.Play("DashLeft", 1);
        }
        if (vertical > 0&&horizontal>0)
        {
            anim.Play("DashFrontRight", 0);
            anim.Play("DashFrontRight", 1);
        }
        if (vertical > 0 &&horizontal<0)
        {
            anim.Play("DashFrontLeft", 0);
            anim.Play("DashFrontLeft", 1);
        }
        if (vertical < 0 &&horizontal>0)
        {
            anim.Play("DashBackRight", 0);
            anim.Play("DashBackRight", 1);
        }
        if (vertical < 0 &&horizontal<0)
        {
            anim.Play("DashBackLeft", 0);
            anim.Play("DashBackLeft", 1);
        }




        print("");
    }
    void Shooting()
    {
        if (Input.GetButton("Fire1"))
        {
            anim.SetBool("Attack", true);
        }
        else
        {
            anim.SetBool("Attack", false);
        }
    }
    void CmdFireL()
    {
        print("Left");
    }
    void CmdFireR()
    {
        print("Right");
    }

    // Update is called once per frame
    void FixedUpdate () {
        move();
        //Jump();
        //cameraSystem();
        flashBomb();
        
	}
    void LateUpdate()
    {


        if(look)
        {
            upperBody.LookAt(AimingPoint.transform.position);
            BoneLookAtOffset.z = -90;
            upperBody.Rotate(BoneLookAtOffset);
        }
        



    }
    void Update()
    {
        if(look)
        {
            BoneLookAtOffset.z = -90;
            upperBody.Rotate(BoneLookAtOffset);
            
        }
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        anim.SetFloat("inputH", horizontal);
        anim.SetFloat("inputV", vertical);
        //mousex += Input.GetAxis("Mouse X") * Time.deltaTime * speedMouse;
        //mousey += Input.GetAxis("Mouse Y") * Time.deltaTime * speedMouse;


        Shooting();
        //if (Input.GetKey(KeyCode.LeftShift) && this.CompareTag("testChar"))
        //{
        //    run = true;

        //}
        //if (Input.GetKeyDown(KeyCode.LeftShift) && this.CompareTag("blinkChar"))
        //{
        //    blink = true;
        //}
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
            Jump();
        }
        //if(Input.GetKeyDown(KeyCode.LeftShift)&&this.CompareTag("flashBombChar"))
        //{
        //    //flashBombTarget = new Vector3(transform.position.x, transform.position.y, transform.position.z + 5);
        //    //rb.AddForce(Vector3.up * 5f, ForceMode.VelocityChange);
        //    //flashBombActive = true;
        //    //moved = false;
        //}
        if (Input.GetKeyDown(KeyCode.LeftShift) && this.CompareTag("SpinKick"))
        {
            spinKick();
            moved = false;
        }
        //cameraSystem();
        //saikung coding
        //float x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        //float z = Input.GetAxis("Vertical") * Time.deltaTime * speed;
        if (Input.GetAxis("Mouse X") > 0)

            CameraObj.transform.eulerAngles += new Vector3(0, speedMouse, 0);
        if (Input.GetAxis("Mouse X") < 0)
            CameraObj.transform.eulerAngles += new Vector3(0, -speedMouse, 0);
        if (Input.GetAxis("Mouse Y") > 0)
            CameraObj.transform.eulerAngles += new Vector3(-speedMouse, 0, 0);
        if (Input.GetAxis("Mouse Y") < 0)
            CameraObj.transform.eulerAngles += new Vector3(speedMouse, 0, 0);
        //isGroundCheck();
        //CharacterisGround();
    }

    //void isGroundCheck()
    //{
    //    if (Physics.Raycast(transform.position, Vector3.down, 0.1f))
    //    {
    //        isGround = true;
    //        Debug.Log("HIT");

    //    }
    //    else
    //    {
    //        isGround = false;


    //    }
    //}    
    //void CharacterisGround()
    //{
    //    if (isGround == false)
    //    {
    //        rb.velocity = Vector3.down * Time.deltaTime*1000;
    //    }
    //}
}

