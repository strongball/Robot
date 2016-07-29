using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class ReveiveData : MonoBehaviour {
	public GameObject WaiterDisplay;
	public GameObject WaiterPanel;

	private List<WaiterStatus> waiters;
	private bool change;
	// Use this for initialization
	void Start () {
		waiters = new List<WaiterStatus>();
		change = false;
		MyWebSocket.On("updata", Status);
	}
	void Update()
	{
		if (change)
		{
			ReList();
		}
	}
	void Status(JsonData jd)
	{
		waiters.Clear();
		
		for (int i = 0; i < jd.Count; i++)
		{
			waiters.Add(new WaiterStatus(jd[i]["name"].ToString(), jd[i]["power"].ToString(), jd[i]["mood"].ToString()));
		}
		change = true;
	}

	void ReList()
	{
		change = false;
		Transform waiterSite = WaiterPanel.transform;
		for (int i = 0; i < waiterSite.childCount; i++)
		{
			Destroy(waiterSite.GetChild(i).gameObject);
		}
		foreach(WaiterStatus ws in waiters)
		{
			GameObject list = Instantiate(WaiterDisplay);
			list.transform.SetParent(waiterSite, false);
			list.GetComponent<WaiterCtrl>().SetEmotion(ws.emotion);
		}
	}
}

public class WaiterStatus
{
	public string name;
	public float power;
	public float emotion;
	public WaiterStatus(string name, string power, string emotion)
	{
		this.name = name;
		this.power = float.Parse(power);
		this.emotion = float.Parse(emotion);
	}
}
