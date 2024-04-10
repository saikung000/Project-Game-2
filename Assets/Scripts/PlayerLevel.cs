using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerLevel : NetworkBehaviour
{
    [SyncVar(hook = "OnlvlcurrentChange")]
    public int lvlcurrent = 1;
    [SyncVar(hook = "OnExpChange")]
    public int exp;
    [SyncVar(hook = "Onlvlup")]
    public int lvlup = 0;
    public int exptolvlup = 1000;
    [SyncVar(hook = "OnGetSkillPoint")]
    public int skillpoint;

    public PlayerHealth ph;

    [SyncVar(hook = "Onuplvlskill1")]
    public int lvlskill1 = 1;
    [SyncVar(hook = "Onuplvlskill2")]
    public int lvlskill2 = 1;
    [SyncVar(hook = "Onuplvlskill3")]
    public int lvlskill3 = 1;
    [SyncVar(hook = "Onuplvlskill4")]
    public int lvlskill4 = 1;
    [SyncVar(hook = "Onuplvlskill5")]
    public int lvlskill5 = 1;
    [SyncVar(hook = "Onuplvlskill6")]
    public int lvlskill6 = 1;
    // Use this for initialization
    void Start()
    {
        ph = GetComponent<PlayerHealth>();
        lvlcurrent = lvlskill1 = lvlskill2 = lvlskill3 = lvlskill4 = lvlskill5 = lvlskill6 = 1;
        lvlup = exp = 0;
    }

    // Update is called once per frame
    void Update()
    {

      //  exptolvlup = (lvlcurrent + lvlup) * 1000;
        if (isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                CmdGetExpNow(1000);
				CmdLvlupNow();
                //OnlvlcurrentChange(lvlcurrent++ );
                //OnGetSkillPoint(skillpoint++);
                //RpcLvlup();
            }
        }


    }

    void Onuplvlskill1(int Lvl)
    {
        lvlskill1 = Lvl;
    }
    void Onuplvlskill2(int Lvl)
    {
        lvlskill2 = Lvl;
    }
    void Onuplvlskill3(int Lvl)
    {
        lvlskill3 = Lvl;
    }
    void Onuplvlskill4(int Lvl)
    {
        lvlskill4 = Lvl;
    }
    void Onuplvlskill5(int Lvl)
    {
        lvlskill5 = Lvl;
    }
    void Onuplvlskill6(int Lvl)
    {
        lvlskill6 = Lvl;
    }
    void OnlvlcurrentChange(int Lvl)
    {
        lvlcurrent = Lvl;
    }
    void OnExpChange(int Exp)
    {
        exp = Exp;
    }
    void OnGetSkillPoint(int point){
        skillpoint  = point;
    }
    void Onlvlup(int lvl)
    {
        lvlup = lvl;
    }
    public void GetExp(int amount)
    {
        
        exp += amount;
        if (exp >= exptolvlup)
        {
            exp -= exptolvlup;
            lvlup++;

        }

    }
    public void upskill(int skill)
    {
        if (skillpoint <= 0)
        {
            return;
        }
        switch (skill)
        {
            case 1:
                if (lvlskill1 < 15)
                {
                    lvlskill1++;
                    skillpoint--;
                    ph.GetHealth(1000);
                }
                break;
            case 2:
                if (lvlskill2 < 15)
                {
                    lvlskill2++;
                    skillpoint--;
                }
                break;
            case 3:
                if (lvlskill3 < 4)
                {
                    lvlskill3++;
                    skillpoint--;
                }
                break;
            case 4:
                if (lvlskill4 < 4)
                {
                    lvlskill4++;
                    skillpoint--;
                }
                break;
            case 5:
                if (lvlskill5 < 4)
                {
                    lvlskill5++;
                    skillpoint--;
                }
                break;
            case 6:
                if (lvlskill6 < 4 && lvlcurrent > 5)
                {
                    lvlskill6++;
                    skillpoint--;
                }
                break;
        }

    }
    public void Lvlup()
    {
        Debug.Log("Lvlup");
        lvlcurrent += lvlup;
        skillpoint += lvlup;
        lvlup = 0;
    }
    [Command]
    public void CmdGetExpNow(int amount)
    {
        
        exp += amount;
        if (exp >= exptolvlup)
        {
            exp -= exptolvlup;
            lvlup++;

        }

    }
    [Command]
    public void CmdLvlupNow()
    {
        Debug.Log("Lvlup");
        lvlcurrent += lvlup;
        skillpoint += lvlup;
        lvlup = 0;
    }

    
}
