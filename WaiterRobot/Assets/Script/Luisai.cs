using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class Luisai : MonoBehaviour
{
	public UnityEvent MyEvent;
	JsonData itemdata;
	Speech speech;
	TextToSpeech textToSpeech;
	void Start()
	{
		if (MyEvent == null)
			MyEvent = new UnityEvent();
		Speech.addListener(callLuis);
	}
		IEnumerator WaitForRequest(WWW www)
    {
        yield return www;
        itemdata = JsonMapper.ToObject(www.text);
		DialogIntents(itemdata);
    }
    public void callLuis(string input)//input is the input string
    {
		string url = "https://api.projectoxford.ai/luis/v1/application?id=929b06ed-deda-479c-82a7-a135b681fe12&subscription-key=c02b958474214f15bbe7ffd005ef041d&q="
        + System.Uri.EscapeDataString(input);
        WWW www = new WWW(url);
        StartCoroutine(WaitForRequest(www));
    }
	public void DialogIntents(JsonData itemdata)
	{
		string s = itemdata["intents"][0]["intent"].ToString();
		/*if ((itemdata["intents"][0]["intent"].ToString()) == "吃飯對話")
		{
			s = "吃飯對話";
		}
		else if ((itemdata["intents"][0]["intent"].ToString()) == "日常對話")
		{
			s = "日常對話";
		}
		else if ((itemdata["intents"][0]["intent"].ToString()) == "問候")
		{
			s = "問候";
		}
		else if ((itemdata["intents"][0]["intent"].ToString()) == "垃圾話")
		{
			s = "垃圾話";
		}
		else
		{
			s = "你腦殘";
		}*/
		Toast.makeText(s, false);
		if(s == "拍照")
		{
			MyEvent.Invoke();
		}
		if (s.Length > 1)
		{
			TextToSpeech.Say(s);
		}
		else
		{
			TextToSpeech.Say("低能兒");
		}
		
	}
}
