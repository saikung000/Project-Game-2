using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class PlayerSkill2 : NetworkBehaviour
{

    public UISkill2Manager UISkill2Manager;
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

    public PlayerController playerControl;

    public GameObject bulletPrefab;

    public bool useSkill4 = false;

    public Animator anim;
public CameraControl cameracontrol;
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        playerControl = GetComponent<PlayerController>();
        playerShoot = GetComponent<PlayerShoot>();
        playerHealth = GetComponent<PlayerHealth>();
        playerLevel = GetComponent<PlayerLevel>();
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
        if (UISkill2Manager.UpSkill)
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

        if (Input.GetButtonDown("Skill1") && timerCooldownSkill1 <= 0)
        {
            timerCooldownSkill1 = cooldownSkill1 - ((playerLevel.lvlskill2 - 1) * 4);
            CmdSkill1();
            StartCoroutine(WaitShoot(1));

        }
        if (Input.GetButtonDown("Skill2") && timerCooldownSkill2 <= 0)
        {
            timerCooldownSkill2 = cooldownSkill2 - ((playerLevel.lvlskill3 - 1) * 5);
            //CmdSkill2();
            //StartCoroutine(SpawnSkill2());
            // CmdSkill1();
            // StartCoroutine(WaitShoot(1.5f));

        }
        if (Input.GetButtonDown("Skill3") && timerCooldownSkill3 <= 0)
        {
            timerCooldownSkill3 = cooldownSkill3 - ((playerLevel.lvlskill3 - 1) * 5);
            CmdSkill3();
            StartCoroutine(WaitShoot(1f));
            //StartCoroutine(SpawnSkill2());
            // CmdSkill1();


        }
        if (Input.GetButtonDown("Skill4") && timerCooldownSkill4 <= 0)
        {
            print("uti");
            timerCooldownSkill4 = cooldownSkill4 - ((playerLevel.lvlskill4 - 1) * 5);
            CmdSkill4();
            cameracontrol.zoom();
            StartCoroutine(WaitShoot(2.3f));

        }


    }
    [Command]
    void CmdSkill1()
    {
        var bullet = (GameObject)Instantiate(
          skill1Prefab,
          skillSpawn.position,
          skillSpawn.rotation);
        Ray ray = new Ray();
        ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, layerMask))
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
        bullet.GetComponent<BulletSkill1>().damage = skill1Damage;
        bullet.GetComponent<BulletSkill1>().player = this.gameObject;
        NetworkServer.Spawn(bullet);
        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
    }
    [Command]
    void CmdSkill3()
    {
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            skill3Prefab,
            skillSpawn.position,
            skillSpawn.rotation);
        Ray ray = new Ray();
        ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, layerMask))
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
        NetworkServer.Spawn(bullet);
        // Destroy the bullet after 2 seconds
        Destroy(bullet, 4.0f);


    }




    [Command]
    void CmdSkill4()
    {
        StartCoroutine(Skill4());
    }
    IEnumerator Skill4()
    {
        StartCoroutine(canMove(2f));
        yield return new WaitForSeconds(2f);
        var bullet = (GameObject)Instantiate(
         skill4Prefab,
         transform.position,
         transform.rotation);
        float range = 7;
        var colls = Physics.OverlapSphere(transform.position, range);

        foreach (Collider enemy in colls)
        {
            if (enemy.transform.gameObject.tag == "Enemy")
            {

                var health = enemy.gameObject.GetComponent<EnemyHealth>();
                if (health != null)
                {
                    health.TakeDamage(skill4Damage, this.gameObject);
                }
            }
        }
        Destroy(bullet, 0.5f);



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
        playerShoot.canShoot = false;
        yield return new WaitForSeconds(time);
        playerShoot.canShoot = true;
        cameracontrol.normal();
    }
    IEnumerator canMove(float time)
    {
        playerControl.canMove = false;
        yield return new WaitForSeconds(time);
        playerControl.canMove = true;
    }


}
