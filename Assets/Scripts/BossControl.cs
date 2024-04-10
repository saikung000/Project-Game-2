using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class BossControl : NetworkBehaviour
{
    public float areaXMin, areaZMin, areaXMax, areaZMax;
    public WaveData[] listWave;
    public float bossHP = 100;
    public GameObject bulletprefab, bulletFollow, bomb, bulletFast;

    public GameObject bulletFastSpawner, missileSpanwer, missileSpanwer2, missileSpanwer3, missileSpanwer4;
    public EnemyHealth enHealth;
    public Transform[] bulletSpawner;
    public GameObject enemybomb;
    public int rn;

    public List<GameObject> spawnWavePoint = new List<GameObject>();
    public GameObject[] spawnRandomMissile;
    public GameObject RandomMissileBegin, RandomMissileBoom;

    public GameObject[] spawnRandomPoint;
    public int TotalRnd = 0;
    private int rndCount = 0;
    public float waitspawnBulletFollow, followTime, waitspawnBullet;
    public bool spawnFinish;
    public bool triggerCheck75 = true;
    public bool triggerCheck50 = true;
    public bool triggerCheck25 = true;

    Animator anim;
    public GameObject Target;
    public GameObject Gun;
    public float RotationSpeed, waitShoot;
    private Quaternion _lookRotation;
    private Vector3 _direction;

    public bool gunFollowPlayer;

    public GameObject[] players;
    public int BulletFastSpeed, BulletSpeed;

    public LayerMask layerMask;
    // Use this for initialization
    void Start()
    {
        // spawnWavePoint = GameObject.FindGameObjectsWithTag("SpawnEnemyWave");
        spawnRandomPoint = GameObject.FindGameObjectsWithTag("SpawnEnemyRandom");
        anim = GetComponent<Animator>();


        //
        enHealth = GetComponent<EnemyHealth>();
        gunFollowPlayer = false;
        randomAtt();

        //StartCoroutine(WaitrandomAtt());
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.isGameClear || GameManager.isGameOver)
        {
            Target = null;
            return;
        }
        bossHP = enHealth.getCurrentHealth();
        Debug.Log("boss HP :" + bossHP + "");
        if (!isServer)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            SpawnBulletFollow(3);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            SpawnBomb(10);
        }

        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            SpawnEnemywave(1, 1);
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            SpawnEnemyBomb(5);
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            GunFollowPlayer();
        }

        if (Input.GetKeyDown(KeyCode.Keypad6))
        {

            anim.Play("BigBomg_Begin");
        }

        if (gunFollowPlayer && !GameManager.isGameOver)
        {

            Debug.DrawLine(bulletFastSpawner.transform.position, Target.transform.position, Color.green);
            Debug.Log("Zoom");
            _direction = (Target.transform.position - bulletFastSpawner.transform.position).normalized;

            _lookRotation = Quaternion.LookRotation(_direction);

            Gun.transform.rotation = Quaternion.Slerp(Gun.transform.rotation, _lookRotation, Time.deltaTime * RotationSpeed);


            Vector3 fwd = bulletFastSpawner.transform.TransformDirection(Vector3.forward);
            RaycastHit hit;

            if (Physics.Raycast(bulletFastSpawner.transform.position, fwd, out hit, 10000, layerMask))
            {
                Debug.DrawLine(bulletFastSpawner.transform.position, hit.point, Color.red);
            }

        }


        if (Input.GetKeyDown(KeyCode.J))
        {
            Cmdtrigger75();

        }
        if (triggerCheck75 && bossHP <= 75.0f)
        {
            Cmdtrigger75();

        }
        else if (triggerCheck50 && bossHP <= 50.0f)
        {
            Cmdtrigger50();

        }
        else if (triggerCheck25 && bossHP <= 25.0f)
        {
            Cmdtrigger25();

        }

    }
    [Command]
    public void Cmdtrigger75()
    {
        Rpctrigger75();
    }
    [Command]
    public void Cmdtrigger50()
    {
        Rpctrigger50();
    }
    [Command]
    public void Cmdtrigger25()
    {
        Rpctrigger25();
    }

    [ClientRpc]
    public void Rpctrigger75()
    {
        Debug.Log("trigger 75");
        triggerCheck75 = false;
        StopAllCoroutines();
        anim.SetTrigger("bigBomb");
        StartCoroutine(afterBigbomb());
    }
    [ClientRpc]
    public void Rpctrigger50()
    {
        Debug.Log("trigger 50");
        triggerCheck50 = false;
        StopAllCoroutines();
        anim.SetTrigger("rage");
        StartCoroutine(afterBigbomb());
    }
    [ClientRpc]
    public void Rpctrigger25()
    {
        Debug.Log("trigger 25");
        triggerCheck25 = false;
        StopAllCoroutines();
        anim.SetTrigger("rage");
        StartCoroutine(afterBigbomb());
    }
    public void GunFollowPlayer()
    {
        findnearplayer();
        gunFollowPlayer = true;
        //CmdShootBulletFast();
    }
    public void SpawnEnemywave(int wave, int position)
    {
        if (!isServer)
        {
            return;
        }
        StartCoroutine(SpawnEnemy(wave, position));
    }


    //public void SpawnBullet(int num ,int position)
    //{

    //    StartCoroutine(ShootBullet(num,position));
    //}
    void SpawnBullet(string nums)
    {
        if (!isServer)
        {
            return;
        }
        Debug.Log("5555");
        print("5555");
        string[] num = nums.Split('/');
        int numShot = int.Parse(num[0]);
        int position = int.Parse(num[1]);
        Debug.Log(num[0] + " " + num[1] + " " + numShot + " " + position);
        StartCoroutine(ShootBullet(numShot, position));
    }

    public void showDe()
    {
        print("5555test");
    }
    public void SpawnBulletFollow(int num)
    {
        if (!isServer)
        {
            return;
        }
        CmdSpawnBulletFollow(num);
    }
    public void SpawnBomb(int num)
    {
        if (!isServer)
        {
            return;
        }
        CmdSpawnBomb(num);
    }

    public void SpawnEnemyBomb(int num)
    {
        if (!isServer)
        {
            return;
        }
        StartCoroutine(SpawnEnemyBombRandom(num));

    }
    public void RandomMissileLeft()
    {
        var randomBoomPar = (GameObject)Instantiate(RandomMissileBegin, spawnRandomMissile[0].transform.position, spawnRandomMissile[0].transform.rotation);
        var randomBoomPar2 = (GameObject)Instantiate(RandomMissileBegin, spawnRandomMissile[1].transform.position, spawnRandomMissile[1].transform.rotation);
        Destroy(randomBoomPar, 3);
        Destroy(randomBoomPar2, 3);

    }
    public void RandomMissileRight()
    {
        var randomBoomPar3 = (GameObject)Instantiate(RandomMissileBegin, spawnRandomMissile[2].transform.position, spawnRandomMissile[2].transform.rotation);
        var randomBoomPar4 = (GameObject)Instantiate(RandomMissileBegin, spawnRandomMissile[3].transform.position, spawnRandomMissile[3].transform.rotation);
        Destroy(randomBoomPar3, 3);
        Destroy(randomBoomPar4, 3);
    }
    public void ShootBulletFast()
    {
        if (!isServer)
        {
            return;
        }
        CmdShootBulletFast();
    }
    [Command]
    void CmdShootBulletFast()
    {
        /*
         if (isServer)
         {
             findnearplayer();
         }
        */
        //GunFollowPlayer();
        StartCoroutine(ShootingBulletFast());
    }
    [Command]
    void CmdSpawnBullet(int slot)
    {
        var bullet = (GameObject)Instantiate(
         bulletprefab,
         bulletSpawner[slot].position,
         bulletSpawner[slot].rotation);

        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * BulletSpeed;

        NetworkServer.Spawn(bullet);

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 5.0f);
    }

    [Command]
    void CmdSpawnBulletFollow(int num)
    {
        StartCoroutine(SpawnBulletFollowWait(num));
    }
    IEnumerator afterBigbomb()
    {
        yield return new WaitForSeconds(30.0f);
        randomAtt();
        StopCoroutine(afterBigbomb());
    }
    IEnumerator SpawnBulletFollowWait(int num)
    {
        for (int i = 0; i < num; i++)
        {
            var bullet = (GameObject)Instantiate(bulletFollow, missileSpanwer.transform.position, missileSpanwer.transform.rotation);
            NetworkServer.Spawn(bullet);
            var bullet2 = (GameObject)Instantiate(bulletFollow, missileSpanwer2.transform.position, missileSpanwer2.transform.rotation);
            NetworkServer.Spawn(bullet2);
            var bullet3 = (GameObject)Instantiate(bulletFollow, missileSpanwer3.transform.position, missileSpanwer3.transform.rotation);
            NetworkServer.Spawn(bullet3);
            var bullet4 = (GameObject)Instantiate(bulletFollow, missileSpanwer4.transform.position, missileSpanwer4.transform.rotation);
            NetworkServer.Spawn(bullet4);
            yield return new WaitForSeconds(waitspawnBulletFollow);
        }
    }
    [Command]
    public void CmdwaitForAttack()
    {
        RpcwaitForAttack();
    }
    [ClientRpc]
    public void RpcwaitForAttack()
    {
        StartCoroutine(waitForAttack());
    }
    IEnumerator waitForAttack()
    {
        yield return new WaitForSeconds(5.0f);
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(60.0f);
        Debug.Log("Random att");
        randomAtt();
        StopCoroutine(waitForAttack());
    }
    public void randomAtt()
    {
        if (!isServer)
        {

            return;
        }

        rn = Mathf.RoundToInt(Random.Range(1.0f, 3.0f));
        CmdrandomAttCommand();
    }
    [Command]
    public void CmdrandomAttCommand()
    {

        StartCoroutine(WaitrandomAtt());
        //RpcWaitrandomAtt();
    }

    //  [ClientRpc]
    public void RpcWaitrandomAtt()
    {
        StartCoroutine(WaitrandomAtt());
    }
    IEnumerator WaitrandomAtt()
    {

        yield return new WaitForSeconds(5.0f);
        rndCount++;
        //  Debug.Log("555212312");

        Debug.Log(rn);
        if (rn == 1)
        {
            /* 
            anim.SetTrigger("n1");
            Debug.Log("Trigger1");
            */
            RpcN1();
        }
        else if (rn == 2)
        {   /* 
            anim.SetTrigger("n2");
            Debug.Log("Trigger2");
            */
            RpcN2();
        }
        else
        {
            /* 
            anim.SetTrigger("n3");
            Debug.Log("Trigger3");
            */
            RpcN3();
        }
        yield return new WaitForSeconds(10.0f);
        if (rndCount <= TotalRnd)
        {
            /* 
            StopCoroutine(WaitrandomAtt());
            StartCoroutine(WaitrandomAtt());
            */
            Rpcstop1();
        }
        else
        {
            /* 
            StopCoroutine(WaitrandomAtt());
            StartCoroutine(waitForAttack());
            */
            Rpcstop2();
        }


    }
    [ClientRpc]
    public void RpcN1()
    {
        anim.SetTrigger("n1");
        Debug.Log("Trigger1");
    }
    [ClientRpc]
    public void RpcN2()
    {
        anim.SetTrigger("n2");
        Debug.Log("Trigger2");
    }
    [ClientRpc]
    public void RpcN3()
    {
        anim.SetTrigger("n3");
        Debug.Log("Trigger3");
    }
    [ClientRpc]
    public void Rpcstop1()
    {
        StopCoroutine(WaitrandomAtt());

        CmdrandomAttCommand();
    }
    [ClientRpc]
    public void Rpcstop2()
    {
        StopCoroutine(WaitrandomAtt());

        CmdwaitForAttack();
    }

    [Command]
    void CmdSpawnBomb(int num)
    {
        for (int i = 0; i < num; i++)
        {
            Vector3 positionRandom = new Vector3(Random.Range(areaXMin, areaXMax), 0, Random.Range(areaZMin, areaZMax));
            var bullet = (GameObject)Instantiate(bomb, positionRandom, Quaternion.identity);
            NetworkServer.Spawn(bullet);

        }
    }

    IEnumerator ShootingBulletFast()
    {

        yield return new WaitForSeconds(waitShoot);
        //Gun.transform.LookAt(Target.transform.position);
        var bullet = (GameObject)Instantiate(bulletFast, bulletFastSpawner.transform.position, Gun.transform.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * BulletFastSpeed;
        //gunFollowPlayer = false;
        NetworkServer.Spawn(bullet);

    }

    IEnumerator ShootBullet(int num, int position)
    {

        for (int i = 0; i < num; i++)
        {

            CmdSpawnBullet(position);
            yield return new WaitForSeconds(waitspawnBullet);
        }

    }

    IEnumerator ShootBullet2(int num, int position)
    {

        for (int i = 0; i < num; i++)
        {
            CmdSpawnBullet(position);
            yield return new WaitForSeconds(waitspawnBullet);
        }

    }

    IEnumerator SpawnEnemy(int wave, int position)
    {


        spawnFinish = false;

        for (int i = 0; i < listWave[wave].listEnemy.Count; i++)
        {

            for (int x = 0; x < listWave[wave].listEnemy[i].Count; x++)
            {
                // print(i + "" + "" + x);
                if (!GameManager.isGameOver)
                {
                    if (listWave[wave].listEnemy[i].enemyPrefab != null)
                    {
                        var enemy = (GameObject)Instantiate(listWave[wave].listEnemy[i].enemyPrefab, spawnWavePoint[position].transform.position, spawnWavePoint[position].transform.rotation);
                        NetworkServer.Spawn(enemy);
                    }
                    yield return new WaitForSeconds(listWave[wave].listEnemy[i].wait);
                }
            }

        }
        spawnFinish = true;

    }

    IEnumerator SpawnEnemyBombRandom(int num)
    {


        spawnFinish = false;

        for (int i = 0; i < num; i++)
        {

            int random = Random.Range(0, spawnRandomPoint.Length);
            if (!GameManager.isGameOver)
            {

                var enemy = (GameObject)Instantiate(enemybomb, spawnRandomPoint[random].transform.position, spawnRandomPoint[random].transform.rotation);
                NetworkServer.Spawn(enemy);

                yield return new WaitForSeconds(waitspawnBulletFollow);
            }
        }
        spawnFinish = true;

    }

    public void findnearplayer()
    {

        players = GameObject.FindGameObjectsWithTag("Player");
        int randomPlayer = Random.Range(0, players.Length);
        Target = players[randomPlayer];


    }

}
