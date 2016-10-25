using UnityEngine;
using System.Collections;
using System;
using LitJson;

public class Battlemanager : MonoBehaviour {
    public GameObject GamePad;
    public GameObject PlayerCtrl1;
    public GameObject PlayerCtrl2;

    public int MaxHP = 100;
    public int MaxMP = 100;
    public int Attack = 10;

    [SerializeField]
    public SkillSetting PowerUp;
    [SerializeField]
    public SkillSetting SpeedUp;
    Player p1Ctrl;
    Player p2Ctrl;

    Player ownPlayer;
    bool isEnd;
    string playerName;
    // Use this for initialization
    void Awake ()
    {
        isEnd = false;
        p1Ctrl = PlayerCtrl1.GetComponent<Player>();
        p2Ctrl = PlayerCtrl2.GetComponent<Player>();
        MyWebSocket.On("UpdataHPMP", HPMPHandler);
        MyWebSocket.On("EndGame", EndGame);

        Bluetooth.AddMessageListener(blueDialog);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (isEnd)
        {
            isEnd = false;
            GamePad.SetActive(false);
        }
	}

    public void blueDialog(string s)
    {
        if (s == "onhit")
        {
            GetDamage();
        }
    }

    public void StartGame(string name)
    {
        playerName = name;
		Toast.makeText("You are " + playerName, false);
		if (playerName == "p1")
        {
            ownPlayer = p1Ctrl;
        }
        else
        {
            ownPlayer = p2Ctrl;
        }

        GamePad.SetActive(true);

        JSONObject data = new JSONObject();
        data.AddField("maxHP", MaxHP);
        data.AddField("maxMP", MaxMP);
        data.AddField("attack", Attack);
        MyWebSocket.Emit("Setting", data);
    }

    public void GetDamage()
    {
        MyWebSocket.Emit("GetDamage");  
    }
    
    public void UsePowerUp()
    {
        if (ownPlayer.IsEnergyEnough(PowerUp.Cost))
        {
            JSONObject data = new JSONObject();
            data.AddField("name", PowerUp.Name);
            data.AddField("MP", PowerUp.Cost);
            data.AddField("skillTime", PowerUp.SkillTime*1000);
            data.AddField("powerValue", PowerUp.PowerValue);
            MyWebSocket.Emit("UseSkill", data);
        }
    }

    public void UseSpeedUp()
    {
        if (ownPlayer.IsEnergyEnough(SpeedUp.Cost))
        {
            JSONObject data = new JSONObject();
            data.AddField("name", SpeedUp.Name);
            data.AddField("MP", SpeedUp.Cost);
            data.AddField("skillTime", SpeedUp.SkillTime);
            data.AddField("powerValue", SpeedUp.PowerValue);
            MyWebSocket.Emit("UseSkill", data);
            //Bluetooth.SendToDevice
            GamePad.GetComponent<RobotControler>().SpeedUp(SpeedUp.PowerValue, SpeedUp.SkillTime);
        }
    }

    public void EndGame(JsonData jd)
    {
        Debug.Log("winner is:" + jd.ToString());
		Toast.makeText("Winner is:" + jd.ToString(), false);
		isEnd = true;
    }

    public void HPMPHandler(JsonData jd)
    {
        p1Ctrl.SetCurrentHP(float.Parse(jd["p1"]["HP"].ToString()));
        p1Ctrl.SetCurrentMP(float.Parse(jd["p1"]["MP"].ToString()));
        p2Ctrl.SetCurrentHP(float.Parse(jd["p2"]["HP"].ToString()));
        p2Ctrl.SetCurrentMP(float.Parse(jd["p2"]["MP"].ToString()));
    }
}
[Serializable]
public class SkillSetting
{
    public string Name;
    public float Cost;
    public float SkillTime;
    public float PowerValue;
}