using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TableOrder : MonoBehaviour {
	public GameObject TableNumber;
	public GameObject TableList;

	public GameObject OrderElement;
	public GameObject EditStatus;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Refresh(WaiterAction wact)
	{
		for (int i = 0; i < TableList.transform.childCount; i++)
		{
			GameObject go = TableList.transform.GetChild(i).gameObject;
			Destroy(go);
		}
		TableNumber.GetComponent<Text>().text = "第" + wact.table + "桌";

		foreach (Meal meall in wact.meals)
		{
			Meal meal = meall;
			Transform mealView = Instantiate(OrderElement).transform;
			mealView.name = meal.id + meal.name;
			mealView.SetParent(TableList.transform, false);
			mealView.FindChild("MealName").GetComponent<Text>().text = meal.name;
			mealView.FindChild("Status").GetComponent<Text>().text = meal.status;
			mealView.GetComponent<Button>().onClick.AddListener(()=>
			{
				Transform edit = Instantiate(EditStatus).transform;
				edit.SetParent(this.transform, false);
				Debug.Log(meal.name);
				edit.GetComponent<Button>().onClick.AddListener(() =>
				{
					Destroy(edit.gameObject);
				});
				edit.FindChild("status/CloseButton").GetComponent<Button>().onClick.AddListener(() =>
				{
					Destroy(edit.gameObject);
				});

				edit.FindChild("status/finish").GetComponent<Button>().onClick.AddListener(() =>
				{
					JSONObject data = new JSONObject();
					data.AddField("waiterId", wact.id);
					data.AddField("mealId", meal.id);
					data.AddField("status", "finish");
					MyWebSocket.Emit("OrderChange", data);
					Destroy(edit.gameObject);
				});
				edit.FindChild("status/delete").GetComponent<Button>().onClick.AddListener(() =>
				{
					JSONObject data = new JSONObject();
					data.AddField("waiterId", wact.id);
					data.AddField("mealId", meal.id);
					data.AddField("status", "delete");
					MyWebSocket.Emit("OrderChange", data);
					Destroy(edit.gameObject);
				});
			});
		}
	}
}
