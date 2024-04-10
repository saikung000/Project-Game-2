using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class PlayerSkill : NetworkBehaviour
{


    public UISkillManager UISkillManager;


    public Camera camera;
    public Vector3 crosshairPosition;
    public float allowDistance;
    public LayerMask layerMask;

    public GameObject skill1Prefab, skill3Prefab, skill4Prefab;

    public float cooldownSkill1, cooldownSkill2, cooldownSkill3, cooldownSkill4;

    public float timerCooldownSkill1, timerCooldownSkill2, timerCooldownSkill3, timerCooldownSkill4;

    public int skill1Damage, skill3Damage, skill4Damage;
    public Transform skillSpawn;
    public PlayerHealth playerHealth;
    public PlayerLevel playerLevel;

    public PlayerShoot playerShoot;

    public GameObject bulletPrefab;

    public PlayerController playerController;

    public float range = 4;
    public bool useSkill4 = false;

    public Animator anim;

    float vertical, horizontal;
    public GameObject skillDashFX, BoomFX, UltimateFX, ShotSkill;

    public CameraControl cameracontrol;
    public bool CanMove = false;
    public GameObject body, look;

    public bool isSkill = false;
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        playerShoot = GetComponent<PlayerShoot>();
        playerHealth = GetComponent<PlayerHealth>();
        playerLevel = GetComponent<PlayerLevel>();
        playerController = GetComponent<PlayerController>();
        useSkill4 = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerCooldownSkill1 > 0)
        {
            timerCooldownSkill1 -= Time.deltaTime;
        }
        else
        {
            timerCooldownSkill1 = 0;
        }

        if (timerCooldownSkill2 > 0)
        {
            timerCooldownSkill2 -= Time.deltaTime;
        }
        else
        {
            timerCooldownSkill2 = 0;
        }


        if (timerCooldownSkill3 > 0)
        {
            timerCooldownSkill3 -= Time.deltaTime;
        }
        else
        {
            timerCooldownSkill3 = 0;
        }
        if (timerCooldownSkill4 > 0)
        {
            timerCooldownSkill4 -= Time.deltaTime;
        }
        else
        {
            timerCooldownSkill4 = 0;
        }
        if (GameManager.isGameOver)
        {
            return;
        }
        if (PauseMenu.isOn)
        {
            return;
        }
        if (UISkillManager.UpSkill)
        {
            return;
        }
        if (!isLocalPlayer)
        {
            return;
        }

        if (playerHealth.isdead)
        {


            return;
        }
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Skill1") && timerCooldownSkill1 <= 0 && !isSkill)
        {
            timerCooldownSkill1 = cooldownSkill1 - ((playerLevel.lvlskill3 - 1) * 1);
            playerController.look = false;
            anim.SetLayerWeight(1, 0);
            anim.Play("ShootingSkill1");
            //CmdSkill1();


            StartCoroutine(WaitShoot(1.5f));

        }
        if (Input.GetButtonDown("Skill2") && timerCooldownSkill2 <= 0 && !isSkill)
        {
            timerCooldownSkill2 = cooldownSkill2 - ((playerLevel.lvlskill4 - 1) * 1);
            CmdSkill2();
            // StartCoroutine(WaitShoot(1.5f));
            spinKick();

            //RpcUseSkill2();
        }
        if (Input.GetButtonDown("Skill3") && timerCooldownSkill3 <= 0 && !isSkill)
        {
            timerCooldownSkill3 = cooldownSkill3 - ((playerLevel.lvlskill5 - 1) * 4);

            CmdSkill3();


        }
        if (Input.GetButtonDown("Skill4") && timerCooldownSkill4 <= 0 && !isSkill)
        {

            print("uti");
            timerCooldownSkill4 = cooldownSkill4 - ((playerLevel.lvlskill6 - 1) * 20);
            cameracontrol.zoom();
            UltimateSkill();
            CmdSkill4();
            // StartCoroutine(WaitCanMove(5));
            //StartCoroutine(WaitShoot(5));
            //RpcUseSkill4();

        }
        if (CanMove)
        {
            float x = Input.GetAxis("Horizontal") * Time.deltaTime * 5;
            float z = Input.GetAxis("Vertical") * Time.deltaTime * 5;

            transform.Translate(x, 0, z);
        }

    }
    public void Skill1()
    {
        if (isLocalPlayer)
        {
            CmdSkill1();
        }
    }
    void setLook()
    {
        if (playerController.look)
        {
            playerController.look = false;
        }
        else
        {
            playerController.look = true;
        }

    }
    void setWeight()
    {

        anim.SetLayerWeight(1, 1);


    }

    void setWeightAnim()
    {
        anim.SetLayerWeight(1, 1);
    }
    [Command]
    void CmdSkill1()
    {

        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            skill1Prefab,
            skillSpawn.position,
            skillSpawn.rotation);
        Ray ray = new Ray();
        ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

        RaycastHit hit;
        if (Physics.Raycast(look.transform.position, look.transform.forward, out hit, 1000, layerMask))
        {
            if (hit.distance > allowDistance)
            {
                Debug.DrawLine(ray.origin, hit.point, Color.red);

                // Add velocity to the bullet
                Vector3 direction = (hit.point - bullet.transform.position).normalized;
                bullet.GetComponent<Rigidbody>().velocity = direction * 10;
            }
            else
            {

                bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 20;

            }
        }
        else
        {

            // Add velocity to the bullet
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 10;
        }
        bullet.GetComponent<BulletSkill1>().damage = skill1Damage + (playerLevel.lvlskill3 - 1) * 20;
        bullet.GetComponent<BulletSkill1>().player = this.gameObject;
        NetworkServer.Spawn(bullet);
        // Destroy the bullet after 2 seconds
        Destroy(bullet, 10f);


    }

    /* [Command]
     void  CmdSkill2()
     {
         // Create the Bullet from the Bullet Prefab
         var bullet = (GameObject)Instantiate(
             skill2Prefab,
             skillSpawn.position,
             skillSpawn.rotation);
         Ray ray = new Ray();
         ray =  camera.ScreenPointToRay (new Vector3(Screen.width/2,Screen.height/2));

         RaycastHit hit;
         if (Physics.Raycast (ray,out hit,1000,layerMask)) 
         {
             if(hit.distance >allowDistance){
             Debug.DrawLine (ray.origin, hit.point,Color.red);

         // Add velocity to the bullet
             Vector3 direction = (hit.point - bullet.transform.position).normalized;
             bullet.GetComponent<Rigidbody>().velocity = direction * 10;
             }else {

              bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 40;

             }    
         }else {

         // Add velocity to the bullet
         bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 10;
         }
          bullet.GetComponent<BulletSkill2>().damage = skill1Damage;
          bullet.GetComponent<BulletSkill2>().player = this.gameObject;
         NetworkServer.Spawn(bullet);
          // Destroy the bullet after 2 seconds
         Destroy(bullet, 5.0f);            
     }*/
    [Command]
    void CmdSkill3()
    {

        RpcUseSkill3();
    }
    [Command]
    void CmdSkill2()
    {
        /* anim.SetLayerWeight(1, 0);
         playerController.look = false;
          StartCoroutine(WaitShoot(1.5f));
       */
        Vector3 playerPos = transform.position;
        playerPos.y -= 1;
        GameObject dash = (GameObject)Instantiate(skillDashFX, playerPos, skillDashFX.transform.rotation);
        dash.transform.parent = transform;
        NetworkServer.Spawn(dash);
        RpcSyncGameOjectOnce(dash.transform.localPosition, dash.transform.localRotation, dash, dash.transform.parent.gameObject);
        Destroy(dash.gameObject, 1);

        RpcUseSkill2();
    }

    [Command]
    void CmdSkill4()
    {
        /* anim.SetLayerWeight(1, 0);
         playerController.look = false;
         StartCoroutine(WaitCanMove(5));
         StartCoroutine(WaitShoot(5));
         */
        Vector3 playerPos = transform.position;
        playerPos.y += 0.4f;
        GameObject ultimateFx = (GameObject)Instantiate(UltimateFX, playerPos, skillDashFX.transform.rotation);
        ultimateFx.transform.parent = transform;
        NetworkServer.Spawn(ultimateFx);
        RpcSyncGameOjectOnce(ultimateFx.transform.localPosition, ultimateFx.transform.localRotation, ultimateFx, ultimateFx.transform.parent.gameObject);
        Destroy(ultimateFx.gameObject, 5);


        RpcUseSkill4();
        StartCoroutine(Skill4());
    }
    IEnumerator Skill4()
    {

        for (int i = 0; i < 40 + (playerLevel.lvlskill6 - 1) * 5; i++)
        {
            var colls = Physics.OverlapSphere(transform.position, range + (playerLevel.lvlskill6 - 1) * 2f);
            // print(i);
            foreach (Collider enemy in colls)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, (enemy.transform.position - transform.position), out hit, range + (playerLevel.lvlskill6 - 1) * 2f, layerMask))
                {

                    Transform T = hit.transform;
                    while (T.parent != null) T = T.parent;
                    if (T.transform.gameObject.tag == "Enemy")
                    {
                        Debug.DrawLine(transform.position, hit.point, Color.green);
                        var health = T.gameObject.GetComponent<EnemyHealth>();
                        if (health != null)
                        {
                            health.TakeDamage(skill4Damage + (playerLevel.lvlskill6 - 1) * 2, this.gameObject);
                        }
                    }
                }

            }
            yield return new WaitForSeconds(0.1f);

        }
    }

    IEnumerator SpawnSkill3()
    {
        for (int i = 0; i < 5; i++)
        {
            CmdSkill3();
            yield return new WaitForSeconds(0.3f);
        }
    }
    IEnumerator Skill3(float time)
    {
        playerShoot.bulletPrefab = skill3Prefab;
        yield return new WaitForSeconds(time);
        playerShoot.bulletPrefab = bulletPrefab;
    }
    IEnumerator WaitShoot(float time)
    {
        isSkill = true;
        playerShoot.canShoot = false;
        yield return new WaitForSeconds(time);
        playerShoot.canShoot = true;
        cameracontrol.normal();
        playerController.look = true;
        anim.SetLayerWeight(1, 1);
        isSkill = false;
    }
    IEnumerator WaitCanMove(float time)
    {

        CanMove = true;
        yield return new WaitForSeconds(time);
        CanMove = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, range + (playerLevel.lvlskill6 - 1) * 0.5f);
    }

    void UltimateSkill()
    {

        int way = 0;

        // look = false;

        if (vertical > 0 || (vertical == 0 && horizontal == 0))
        {
            anim.Play("UltimateSkill_Front");
        }
        if (vertical < 0)
        {
            anim.Play("UltimateSkill_B");

        }
        if (horizontal > 0)
        {
            anim.Play("UltimateSkill_R");
        }
        if (horizontal < 0)
        {
            anim.Play("UltimateSkill_L");
        }
        if (vertical > 0 && horizontal > 0)
        {
            anim.Play("UltimateSkill_FR");
        }
        if (vertical > 0 && horizontal < 0)
        {
            anim.Play("UltimateSkill_FL");
        }
        if (vertical < 0 && horizontal > 0)
        {
            anim.Play("UltimateSkill_BR");
        }
        if (vertical < 0 && horizontal < 0)
        {
            anim.Play("UltimateSkill_BL");
        }
    }
    void spinKick()
    {
        int way = 0;
        anim.SetLayerWeight(1, 0);
        // playerController.look = false;
        //  look = false;


        if (vertical > 0 || (vertical == 0 && horizontal == 0))
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
        if (vertical > 0 && horizontal > 0)
        {
            anim.Play("DashFrontRight", 0);
            anim.Play("DashFrontRight", 1);
        }
        if (vertical > 0 && horizontal < 0)
        {
            anim.Play("DashFrontLeft", 0);
            anim.Play("DashFrontLeft", 1);
        }
        if (vertical < 0 && horizontal > 0)
        {
            anim.Play("DashBackRight", 0);
            anim.Play("DashBackRight", 1);
        }
        if (vertical < 0 && horizontal < 0)
        {
            anim.Play("DashBackLeft", 0);
            anim.Play("DashBackLeft", 1);
        }


    }
    [ClientRpc]
    public void RpcSyncGameOjectOnce(Vector3 localPos, Quaternion localRot, GameObject gameobject, GameObject parent)
    {
        gameobject.transform.parent = parent.transform;
        gameobject.transform.localPosition = localPos;
        gameobject.transform.localRotation = localRot;
    }

    [ClientRpc]
    public void RpcUseSkill3()
    {
        StartCoroutine(Skill3((4 + (playerLevel.lvlskill5 - 1) * 1)));
    }

    [ClientRpc]
    public void RpcUseSkill4()
    {
        anim.SetLayerWeight(1, 0);
        playerController.look = false;
        StartCoroutine(WaitCanMove(4 + (playerLevel.lvlskill6 - 1) * 0.5f));
        StartCoroutine(WaitShoot(4 + (playerLevel.lvlskill6 - 1) * 0.5f));
    }
    [ClientRpc]
    public void RpcUseSkill2()
    {
        anim.SetLayerWeight(1, 0);
        playerController.look = false;
        StartCoroutine(WaitShoot(1.5f));
    }




}
