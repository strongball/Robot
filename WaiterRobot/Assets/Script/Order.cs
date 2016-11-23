using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Order : MonoBehaviour {
	public GameObject SpinBoard;
	public GameObject SpinWheel;
	public GameObject CheckPanel;
	public GameObject Title;
	public GameObject Content;

	[Range(0 , 100)]
	public int TryAgain;
	// Use this for initialization
	void Start () {
		IntentManager.addDialogListener(Dialog);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Choice(string meal)
	{
		CheckPanel.SetActive(true);
		Title.GetComponent<Text>().text = meal;
	}

	public void CallService()
	{
		JSONObject data = new JSONObject();
		MyWebSocket.Emit("Service", data);
	}

	public void SendOrder(Text meal)
	{
		JSONObject data = new JSONObject();
		data.AddField("meal", meal.text);
		MyWebSocket.Emit("order", data);
		gameObject.SetActive(false);
	}

	public void ReSpin()
	{
		if(Emotion.Score >= TryAgain)
		{
			TextToSpeech.Say("給你重來一次");
			CheckPanel.SetActive(false);
			Emotion.SetEmotion(false);
			SpinWheel.GetComponent<SpinWheel>().StartGame(false);
		}
		else
		{
			TextToSpeech.Say("不給你重來");
		}
	}

	void OnEnable()
	{
		TextToSpeech.Say("我們這裡是轉到什麼吃什麼");
		SpinBoard.SetActive(true);
	}

	void OnDisable()
	{
		SpinBoard.SetActive(false);
		CheckPanel.SetActive(false);
	}

	public void Dialog(IntentEntity ie)
	{
		if (ie.intent == Intent.Choice)
		{
			foreach (Entity e in ie.entitys)
			{
				if (e.type == Entity.Choice_Confirm)
				{
					if (CheckPanel.activeInHierarchy)
					{
						SendOrder(Title.GetComponent<Text>());
					}
				}
			}
		}
	}
}
