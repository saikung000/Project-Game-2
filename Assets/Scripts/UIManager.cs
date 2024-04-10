using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Linq;
public class UIManager : NetworkBehaviour
{

    public GameObject playerLocal;

    public PlayerLevel pl;

    public PlayerHealth ph;

    public PlayerShoot pb;

    public PlayerScore ps;

    public PlayerName pn;
    public EnemyHealth bossHealth;

    public GameObject panalDead, bosshp;

    public Image hpbar, corehpbar, bosshpbar, ImageChar;

    public Sprite Char1, Char2;

    public Text textLevel, textAmmo, textCountSpawn, textScore, textName;
    public GameObject core;

    public CoreHealth ch;

    public GameObject pauseMenu;

    public GameObject panelMap, panelGameOver, panelGameClear, panalPlayer, showupgrade;

    public static bool UpSkill;

    public GameManager gameManager;
    public GameObject gameManagerObj;
    public Text wavetext, timetext;
    public GameObject playerDataPrefab, playerScorePrefab;
    public List<GameObject> playerlist = new List<GameObject>();

    public List<Player> listPlayerScore = new List<Player>();

    public GameObject panalScore;

    public bool isShowScore = false;

    // Use this for initialization
    public override void OnStartServer()
    {
        //wave = 1;
        // StartWave();

    }

    void Start()
    {
        bosshp.SetActive(false);
        UpSkill = false;
        panalDead.SetActive(false);
        panelMap.SetActive(false);
        //panelUpSkill.SetActive(false);
        panelGameClear.SetActive(false);
        panelGameOver.SetActive(false);
        panalScore.SetActive(false);
        showupgrade.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(true);
            PauseMenu.isOn = true;

        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            panelMap.SetActive(true);



        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            panelMap.SetActive(false);

        }



        if (gameManagerObj == null)
        {
            gameManagerObj = GameObject.FindGameObjectWithTag("GameManager");
            if (gameManagerObj != null)
            {
                gameManager = gameManagerObj.GetComponent<GameManager>();
            }
        }
        else
        {
            if (GameManager.isGameOver)
            {
                Debug.Log("Gameover");
                panelGameOver.SetActive(true);
                panalScore.SetActive(true);
                if (!isShowScore)
                {
                    ShowScore();
                    isShowScore = true;
                }

            }
            if (GameManager.isGameClear)
            {
                Debug.Log("Gameclear");
                panelGameClear.SetActive(true);
                panalScore.SetActive(true);
                if (!isShowScore)
                {
                    ShowScore();
                    isShowScore = true;
                }
            }
            if (!gameManager.isStartWave)
            {
                if (gameManager.wave == gameManager.maxWave)
                {
                    wavetext.text = "BOSS WAVE";
                    timetext.text = "TIME RREMAINING : " + gameManager.waitTimer.ToString("00.00");
                }
                else if (gameManager.wave == gameManager.maxWave + 1)
                {
                    wavetext.text = "GAME OVER";
                    timetext.text = "";
                }
                else
                {
                    wavetext.text = "PREPARE FOR NEXT WAVE";
                    timetext.text = "TIME RREMAINING : " + gameManager.waitTimer.ToString("00.00");
                }
                showupgrade.SetActive(true);
            }
            else if (gameManager.wave == gameManager.maxWave + 1)
            {
                wavetext.text = "BOSS WAVE";
                timetext.text = "";
                if (bossHealth == null && gameManager.boss != null)
                {
                    bossHealth = gameManager.boss.GetComponent<EnemyHealth>();
                }
                bosshp.SetActive(true);
                showupgrade.SetActive(false);

            }
            else
            {
                wavetext.text = "WAVE : " + gameManager.wave;
                timetext.text = "";
                showupgrade.SetActive(false);
            }
        }

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in players)
        {
            if (!playerlist.Contains(p))
            {
                if (!p.GetComponent<NetworkIdentity>().isLocalPlayer)
                {
                    playerlist.Add(p);
                    GameObject playerData = Instantiate(playerDataPrefab) as GameObject;
                    PlayerDataControl pdc = playerData.GetComponent<PlayerDataControl>();
                    pdc.playerName = p.GetComponent<PlayerName>();
                    pdc.playerHealth = p.GetComponent<PlayerHealth>();

                    playerData.transform.parent = panalPlayer.transform;
                    playerData.transform.localScale = Vector3.one;
                }
            }
        }
        if (bossHealth != null)
        {
            bosshpbar.fillAmount = (float)bossHealth.currentHealth / (float)bossHealth.maxHealth;
        }
        else
        {
            bosshpbar.fillAmount = 0;
        }
        if (playerLocal == null)
        {
            //GameObject[] players  =  GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject p in players)
            {
                if (p.GetComponent<NetworkIdentity>().isLocalPlayer)
                {
                    print("find");
                    playerLocal = p;
                    pl = playerLocal.GetComponent<PlayerLevel>();
                    ph = playerLocal.GetComponent<PlayerHealth>();
                    pb = playerLocal.GetComponent<PlayerShoot>();
                    ps = playerLocal.GetComponent<PlayerScore>();
                    pn = playerLocal.GetComponent<PlayerName>();



                }
            }
        }
        else
        {
            // textName.text =""+ pn.name;
            if (pn.playerCharacter == 0)
            {
                ImageChar.sprite = Char1;
            }
            else if (pn.playerCharacter == 1)
            {
                ImageChar.sprite = Char2;
            }
            textLevel.text = "Level : " + pl.lvlcurrent;
            textScore.text = "Score : " + ps.score;
            //  textAmmo.text = pb.bulletsLeft + "/" + pb.bulletsPerClip;




            /* 
          if (pb.bulletsLeft == 0)
          {
              textAmmo.color = Color.red;

          }
          else
          {

              textAmmo.color = Color.gray;
          }
          */

            if (ph.isdead)
            {
                panalDead.SetActive(true);
                textCountSpawn.text = "REMAINING : " + ph.timerespawn.ToString("00.00");

            }
            else
            {
                hpbar.fillAmount = (float)ph.currentHealth / (float)ph.Health;
                panalDead.SetActive(false);
            }


        }
    }
    void OnPlayerConnected()
    {

        playerlist.Clear();
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in players)
        {
            playerlist.Add(p);
        }
    }

    void OnPlayerDisconnected(NetworkPlayer player)
    {
        Debug.Log("Clean up after player " + player);
        Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);
        playerlist.Clear();
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in players)
        {
            playerlist.Add(p);
        }
    }

    public void ShowScore()
    {
        listPlayerScore.Clear();

        for (int i = panalScore.transform.childCount - 1; i >= 0; --i)
        {
            var child = panalScore.transform.GetChild(i).gameObject;
            Destroy(child);
        }
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in players)
        {
            Player player = new Player();
            player.player = p;
            player.score = p.GetComponent<PlayerScore>().score;
            listPlayerScore.Add(player);


        }
        listPlayerScore.OrderByDescending(Player => Player.score).ToList();
        int x = 1;
        foreach (Player p in listPlayerScore)
        {

            GameObject playerData = Instantiate(playerScorePrefab) as GameObject;
            PlayerScoreData psd = playerData.GetComponent<PlayerScoreData>();
            psd.playerName = p.player.GetComponent<PlayerName>();
            psd.playerScore = p.player.GetComponent<PlayerScore>();
            psd.num = x;
            playerData.transform.parent = panalScore.transform;
            playerData.transform.localScale = Vector3.one;
            x++;
        }
        panalScore.SetActive(true);
    }



}
[System.Serializable]
public class Player
{
    public GameObject player;
    public float score;
}
