using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
public class UISkill2Manager :MonoBehaviour
{

    public GameObject playerLocal;
    public PlayerLevel pl;
    public PlayerShoot ps;

    public PlayerSkill2 psk;
    public bool UpSkill;
    public GameObject panelUpSkill;

    public Text textAmmo;
    public Image skill1Icon, skill2Icon, skill3Icon, skill4Icon;

    public Text textLevel2, textSkillPoint, textlevelSkill1, textlevelSkill2, textlevelSkill3, textlevelSkill4, textlevelSkill5, textlevelSkill6;

    public Button btnUpskill1, btnUpskill2, btnUpskill3, btnUpskill4, btnUpskill5, btnUpskill6;

    public GameManager gameManager;
    public GameObject gameManagerObj;
    void Start()
    {
        playerLocal = transform.parent.gameObject;
        pl = playerLocal.GetComponent<PlayerLevel>();
        psk = playerLocal.GetComponent<PlayerSkill2>();
        ps = playerLocal.GetComponent<PlayerShoot>();
        panelUpSkill.SetActive(false);
        btnUpskill1.onClick.AddListener(delegate { pl.upskill(1); });
        btnUpskill2.onClick.AddListener(delegate { pl.upskill(2); });
        btnUpskill3.onClick.AddListener(delegate { pl.upskill(3); });
        btnUpskill4.onClick.AddListener(delegate { pl.upskill(4); });
        btnUpskill5.onClick.AddListener(delegate { pl.upskill(5); });
        btnUpskill6.onClick.AddListener(delegate { pl.upskill(6); });

    }

    // Update is called once per frame
    void Update()
    {
         
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
            if (Input.GetKeyDown(KeyCode.U) && !gameManager.isStartWave)
            {
                if (UpSkill == false)
                {
                    UpSkill = true;
                    panelUpSkill.SetActive(true);
                }
                else
                {
                    UpSkill = false;
                    panelUpSkill.SetActive(false);
                }
            }
            if (gameManager.isStartWave)
            {
                UpSkill = false;
                panelUpSkill.SetActive(false);
            }

             if(UpSkill == false){
                panelUpSkill.SetActive(false);
            }
        }

        skill1Icon.fillAmount = (1 - (psk.cooldownSkill1 - psk.timerCooldownSkill1) / psk.cooldownSkill1);
        skill2Icon.fillAmount = (1 - (psk.cooldownSkill2 - psk.timerCooldownSkill2) / psk.cooldownSkill2);
        skill3Icon.fillAmount = (1 - (psk.cooldownSkill3 - psk.timerCooldownSkill3) / psk.cooldownSkill3);
        skill4Icon.fillAmount = (1 - (psk.cooldownSkill4 - psk.timerCooldownSkill4) / psk.cooldownSkill4);

        /* 
        if (skill1Icon.fillAmount == 1)
        {
            skill1Icon.color = Color.yellow;
        }
        else
        {
            skill1Icon.color = Color.white;
        }

        if (skill2Icon.fillAmount == 1)
        {
            skill2Icon.color = Color.yellow;
        }
        else
        {
            skill2Icon.color = Color.white;
        }

        if (skill3Icon.fillAmount == 1)
        {
            skill3Icon.color = Color.yellow;
        }
        else
        {
            skill3Icon.color = Color.white;
        }

        if (skill4Icon.fillAmount == 1)
        {
            skill4Icon.color = Color.yellow;
        }
        else
        {
            skill4Icon.color = Color.white;
        }
         */
        textSkillPoint.text = "Skill Point : " + pl.skillpoint;
        textlevelSkill1.text = pl.lvlskill1 + "/15";
        textlevelSkill2.text = pl.lvlskill2 + "/15";
        textlevelSkill3.text = pl.lvlskill3 + "/4";
        textlevelSkill4.text = pl.lvlskill4 + "/4";
        textlevelSkill5.text = pl.lvlskill5 + "/4";
        textlevelSkill6.text = pl.lvlskill6 + "/4";

        textLevel2.text = "Level : " + pl.lvlcurrent;

        textAmmo.text = ps.bulletsLeft + "/" + ps.bulletsPerClip;

    }
    public void ExitUpSkill()
    {
        UpSkill = false;
        panelUpSkill.SetActive(false);
    }
}
