using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class CheckOrder : MonoBehaviour {
	public GameObject Display;
	public GameObject OrderElement;

	List<OrderStatus> orders;
	// Use this for initialization
	void Start () {
		orders = new List<OrderStatus>();
		MyWebSocket.On("OrderList", GetOrderList);
	}
	
	// Update is called once per frame
	void Update () {
		ShowOrder();
	}

	void OnEnable()
	{
		Clear();
		RequsetOrderList();
	}

	public void RequsetOrderList()
	{
		MyWebSocket.Emit("CheckOrder", new JSONObject());
	}

	public void Clear()
	{
		for (int i = 0; i < Display.transform.childCount; i++)
		{
			GameObject go = Display.transform.GetChild(i).gameObject;
			Destroy(go);
		}
	}

	public void ShowOrder()
	{
		foreach(OrderStatus os in orders)
		{
			Transform ele = Instantiate(OrderElement).transform;
			ele.SetParent(Display.transform, false);
			ele.FindChild("Name").GetComponent<Text>().text = os.name;
			ele.FindChild("Status").GetComponent<Text>().text = os.status;
		}
		orders.Clear();
	}

	public void GetOrderList(JsonData jd)
	{
		for (int i = 0; i < jd.Count; i++)
		{
			orders.Add(new OrderStatus(jd[i]["name"].ToString(), jd[i]["status"].ToString()));
		}
	}
}

public class OrderStatus{
	public string name;
	public string status;
	public OrderStatus(string name, string status)
	{
		this.name = name;
		this.status = status;
	}
}
