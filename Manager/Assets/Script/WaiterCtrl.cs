using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WaiterCtrl : MonoBehaviour {
	public GameObject Table;
	public GameObject Power;
	public GameObject Emotion;
	public GameObject Bell;

	public Sprite RedBell;
	public Sprite GreenBell;
	string id;
	bool service;

	void Update()
	{
		if (service)
		{
			Bell.GetComponent<Image>().sprite = RedBell;
		}
		else
		{
			Bell.GetComponent<Image>().sprite = GreenBell;
		}
	}

	public void SetData(WaiterAction wa)
	{
		Table.GetComponent<Text>().text = wa.table;
		Emotion.GetComponent<Text>().text = wa.mood.ToString();
		Power.GetComponent<Text>().text = wa.power + "%";
		id = wa.id;
		service = wa.service;
	}
	public void DoneService()
	{
		if (service)
		{
			JSONObject data = new JSONObject();
			data.AddField("id", id);
			MyWebSocket.Emit("ReceiveService", data);
		}
	}
}
