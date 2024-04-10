using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour
{
    public int character;

    public UISkillManager UISkillManager;

    public UISkill2Manager UISkill2Manager;
    public Camera camera;
    public Vector3 crosshairPosition;
    public float allowDistance;
    public LayerMask layerMask;
    public GameObject bulletPrefab;

    public float bulletspeed;
    public GameObject look;
    public Transform bulletSpawnL, bulletSpawnR;

    public int damage = 10;
    public int bulletMax = 10;
    public int bulletsPerClip = 10;
    public float reloadTime = 0.5f;
    // [SyncVar(hook = "OnBulletChange")]
    public int bulletsLeft = 0;

    public bool Reloading = false;

    public float fireRate = 1.0f;
    private float nextFire = 0.0F;

    public bool isLeft = false;


    public PlayerHealth playerHealth;
    public PlayerLevel playerLevel;

    public bool canShoot = true;

    public GameObject shootpar;
    // Use this for initialization


    public Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        canShoot = true;
        bulletsPerClip = bulletMax;
        bulletsLeft = bulletsPerClip;
        playerLevel = GetComponent<PlayerLevel>();
        playerHealth = GetComponent<PlayerHealth>();
        crosshairPosition = GameObject.Find("crosshair").transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        bulletsPerClip = bulletMax;
        if (GameManager.isGameOver || GameManager.isGameClear)
        {
            return;
        }
        if (PauseMenu.isOn)
        {
            return;
        }
        if (character == 0)
        {
            if (UISkillManager.UpSkill)
            {
                return;
            }
        }
        else if (character == 1)
        {
            if (UISkill2Manager.UpSkill)
            {
                return;
            }
        }
        if (!isLocalPlayer)
        {
            return;
        }

        if (playerHealth.isdead)
        {

            return;
        }

        /*if (Input.GetButton("Fire1") && !Reloading  && Time.time > nextFire && canShoot  )
        {
            /*if(bulletsLeft == 0 ){
                CmdReload();
            }else {
           nextFire = Time.time + 1f/fireRate; */
        /*  if(isLeft){
              isLeft = false;
          CmdFireL();
          }else {
              isLeft = true;
          CmdFireR();
          }
          //}

      }
      */
        if (Input.GetButtonUp("Reload") && !Reloading)
        {
            anim.SetBool("Attack", false);
            CmdReload();
        }




        Ray ray = new Ray();
        
        //Debug.DrawRay(bulletSpawn.position,ray.direction*100,Color.black);
        RaycastHit hit;
        if (Physics.Raycast(look.transform.position,look.transform.forward, out hit, 100))
        {
            Debug.DrawLine(bulletSpawnL.position, hit.point, Color.red);
            Debug.DrawLine(bulletSpawnR.position, hit.point, Color.green);
        }



        if (Input.GetButton("Fire1") && !Reloading && canShoot)
        {
            anim.SetBool("Attack", true);
        }
        else
        {
            anim.SetBool("Attack", false);
        }
    }

    void OnBulletChange(int bullet)
    {
        //print(bullet);
        bulletsLeft = bullet;
    }
    public void FireL()
    {
        if (isLocalPlayer || !playerHealth.isdead)
        {
            CmdFireL();
        }
    }
    public void FireR()
    {
        if (isLocalPlayer || !playerHealth.isdead)
        {
            CmdFireR();
        }
    }


    [Command]
    void CmdFireL()
    {
        /* 
        if (!isLocalPlayer)
        {
            return;
        }*/

        /* if(Reloading  && Time.time < nextFire && !canShoot ){
             return;
         }*/
        if (Reloading && !canShoot)
        {
            anim.SetBool("Attack", false);
            return;
        }
        if (bulletsLeft == 0)
        {
            anim.SetBool("Attack", false);
            CmdReload();
            return;
        }
        nextFire = Time.time + 1f / fireRate;
        bulletsLeft--;

        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawnL.position,
            bulletSpawnL.rotation);
        Ray ray = new Ray();
        ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

        RaycastHit hit;
        if (Physics.Raycast(look.transform.position,look.transform.forward, out hit, 1000, layerMask))
        {
            if (hit.distance > allowDistance)
            {
                //Debug.DrawLine(bulletSpawnR.position, hit.point, Color.red);

                // Add velocity to the bullet
                Vector3 direction = (hit.point - bullet.transform.position).normalized;
                bullet.GetComponent<Rigidbody>().velocity = direction * bulletspeed;
            }
            else
            {

                bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletspeed;

            }
        }
        else
        {

            // Add velocity to the bullet
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletspeed;
        }
        if (bullet.GetComponent<Bullet>())
        {
            bullet.GetComponent<Bullet>().damage = damage+ 4 * (playerLevel.lvlskill2-1);
            bullet.GetComponent<Bullet>().player = this.gameObject;
        }
        else
        {
            bullet.GetComponent<BulletSkill2>().damage = damage + 4 * (playerLevel.lvlskill2-1);
            bullet.GetComponent<BulletSkill2>().player = this.gameObject;
        }
        NetworkServer.Spawn(bullet);
        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);

        var par = (GameObject)Instantiate(
            shootpar,
            bulletSpawnL.position,
            bulletSpawnL.rotation);
          Destroy(par, 2.0f);



    }
    
    [Command]
    void CmdFireR()
    {
        /* 
       if (!isLocalPlayer)
       {
           return;
       }*/

        /* if(Reloading  && Time.time < nextFire && !canShoot ){
             return;
         }*/
        if (Reloading && !canShoot)
        {
            anim.SetBool("Attack", false);
            return;
        }
        if (bulletsLeft == 0)
        {
            CmdReload();
            anim.SetBool("Attack", false);
            return;
        }
        nextFire = Time.time + 1f / fireRate;
        bulletsLeft--;

        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawnR.position,
            bulletSpawnR.rotation);
        Ray ray = new Ray();
        ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

        RaycastHit hit;
        if (Physics.Raycast(look.transform.position,look.transform.forward, out hit, 1000, layerMask))
        {
            if (hit.distance > allowDistance)
            {
               // Debug.DrawLine(ray.origin, hit.point, Color.red);

                // Add velocity to the bullet
                Vector3 direction = (hit.point - bullet.transform.position).normalized;
                bullet.GetComponent<Rigidbody>().velocity = direction * bulletspeed;
            }
            else
            {

                bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletspeed;

            }
        }
        else
        {

            // Add velocity to the bullet
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletspeed;
        }
        if (bullet.GetComponent<Bullet>())
        {
            bullet.GetComponent<Bullet>().damage = damage+ 4 * (playerLevel.lvlskill2-1);
            bullet.GetComponent<Bullet>().player = this.gameObject;
        }
        else
        {
            bullet.GetComponent<BulletSkill2>().damage = damage + 4 * (playerLevel.lvlskill2-1);
            bullet.GetComponent<BulletSkill2>().player = this.gameObject;
        }
        NetworkServer.Spawn(bullet);
        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
            var par = (GameObject)Instantiate(
            shootpar,
            bulletSpawnR.position,
            bulletSpawnR.rotation);
          Destroy(par, 2.0f);


    }
    // [Command]
    void CmdReload()
    {
        nextFire = Time.time + reloadTime + 1f / fireRate;
        StartCoroutine(Reload());
    }

    IEnumerator Reload()
    {
        Reloading = true;
        yield return new WaitForSeconds(reloadTime);
        bulletsLeft = bulletsPerClip;
        Reloading = false;
    }
}
