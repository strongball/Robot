using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using LitJson;

public class ReceiveWaiters : MonoBehaviour {
	public static string WaiterAddName = "Waiter_";

	public GameObject Display;
	public GameObject WaiterElement;

	public static string OrderAddName = "Order_";

	public GameObject OrderDisplay;
	public GameObject OrderTable;

	List<WaiterAction> WaitToDo;

	void Awake()
	{
		InitWaiter();
		WaitToDo = new List<WaiterAction>();
		MyWebSocket.On("AllWaiters", ReceiveAll);
		MyWebSocket.On("Updata", ReceiveOne);
		MyWebSocket.On("DelWaiter", DeleteWaiter);
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		foreach(WaiterAction wa in WaitToDo)
		{
			EditInformation(wa);
		}
		WaitToDo.Clear();
	}

	void DeleteWaiter(JsonData jd)
	{
		WaitToDo.Add(new WaiterAction(jd, false));
	}

	void ReceiveOne(JsonData jd)
	{
		WaitToDo.Add(new WaiterAction(jd));
	}

	void ReceiveAll(JsonData jd)
	{
		
		for (int i = 0; i < jd.Count; i++)
		{
			WaitToDo.Add(new WaiterAction(jd[i]));
		}
	}

	void InitWaiter()
	{
		for (int i = 0; i < Display.transform.childCount; i++)
		{
			GameObject go = Display.transform.GetChild(i).gameObject;
			Destroy(go);
		}
	}

	public void EditInformation(WaiterAction wa)
	{
		if (wa.keep)
		{
			
			//waiterdisplay
			Transform edit = Display.transform.FindChild(WaiterAddName + wa.id);
			if (edit == null)
			{
				edit = Instantiate(WaiterElement).transform;
				edit.SetParent(Display.transform, false);
				edit.gameObject.name = WaiterAddName + wa.id;
			}
			edit.GetComponent<WaiterCtrl>().SetEmotion(wa.mood);
			edit.GetComponent<WaiterCtrl>().SetTable(wa.table);

			//setorder
			Transform order = OrderDisplay.transform.FindChild(OrderAddName + wa.id);
			if (order == null)
			{
				order = Instantiate(OrderTable).transform;
				order.SetParent(OrderDisplay.transform, false);
				order.gameObject.name = OrderAddName + wa.id;
			}
			order.GetComponent<TableOrder>().Refresh(wa);
		}
		else
		{
			try
			{
				Transform edit = Display.transform.FindChild(WaiterAddName + wa.id);
				if (edit != null)
				{
					Destroy(edit.gameObject);
				}

				Transform order = OrderDisplay.transform.FindChild(OrderAddName + wa.id);
				if (order != null)
				{
					Destroy(order.gameObject);
				}
			}
			catch (System.Exception)
			{

				throw;
			}
		}
	}
}

public class WaiterAction
{
	public bool keep;
	public string id;
	public string table;
	public int mood;
	public int power;
	public List<Meal> meals;

	public WaiterAction(JsonData jd)
	{
		this.keep = true;
		this.id = jd["id"].ToString();
		this.table = jd["table"].ToString();
		this.mood = int.Parse(jd["mood"].ToString());
		this.power = int.Parse(jd["power"].ToString());


		this.meals = new List<Meal>();
		for(int i = 0; i < jd["orderList"].Count; i++)
		{
			this.meals.Add(new Meal(jd["orderList"][i]));
		}
	}

	public WaiterAction(JsonData jd, bool keep)
	{
		this.keep = keep;
		this.id = jd["id"].ToString();
		this.table = jd["table"].ToString();
		this.mood = int.Parse(jd["mood"].ToString());
		this.power = int.Parse(jd["power"].ToString());

		this.meals = new List<Meal>();
		for(int i = 0; i < jd["orderList"].Count; i++)
		{
			this.meals.Add(new Meal(jd["orderList"][i]));
		}
	}
}

public class Meal
{
	public string id;
	public string name;
	public string status;
	public Meal(JsonData jd)
	{
		this.id = jd["id"].ToString();
		this.name = jd["name"].ToString();
		this.status = jd["status"].ToString();
	}
}
public enum MealStatus
{
	prepare,
	cooking,
	deliver
}
 