using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
using System;

public class Speech : MonoBehaviour
{
	AndroidJavaObject toText;
	public GameObject status;
	public GameObject micPic;

	public Sprite startListening;
	public Sprite stopListening;

	private float waitTime = 0;
	private bool waitRestart = false;
	static Action<string> listeners;
	// Use this for initialization
	void Start()
	{
		//listeners = new List<Action<string>>();
		micPic.GetComponent<Image>().sprite = stopListening;
#if (UNITY_ANDROID && !UNITY_EDITOR)
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject activity = jc.GetStatic<AndroidJavaObject>("currentActivity");
		toText = activity.Call<AndroidJavaObject>("getToText");
		/*Bluetooth.AddMessageListener((s) =>
		{
			if(s == "Start_Speech")
			{
				StartListen();
			}
		});*/
#endif
	}

	void Update()
	{
		if(waitTime > 0)
		{
			if (!waitRestart)
			{
				waitRestart = true;
				//Toast.makeText("停止", false);
				StopListen();

			}
			waitTime -= Time.deltaTime;
		}
		else
		{
			if (waitRestart)
			{
				waitRestart = false;
				StartListen();
				//Toast.makeText("時間", false);
			}
		}
	}

	void speechMessage(string s)
	{
		Toast.makeText(s, false);
		broadcast(s);
	}
	void statusListener(string s)
	{
		status.GetComponent<Text>().text = s;

		if (s == "Ready" || s == "Start")
		{
			micPic.GetComponent<Image>().sprite = startListening;
		}
		else
		{
			micPic.GetComponent<Image>().sprite = stopListening;
		}

		if (s == "Result" && !waitRestart) 
		{
			StartListen();
		}
	}
	// Update is called once per frame

	public void StartListen()
	{
#if (UNITY_ANDROID && !UNITY_EDITOR)
		toText.Call("restartListening");
#endif
	}

	public void StopListen()
	{
#if (UNITY_ANDROID && !UNITY_EDITOR)
		toText.Call("stopListening");
#endif
	}

	public void Restart(int time)
	{
#if (UNITY_ANDROID && !UNITY_EDITOR)
		Toast.makeText("時間", false);
		toText.Call("timeRestart", time);
#endif
	}

	public void TimeRestart(float time)
	{
		waitTime = time;
	}

	public static void addListener(Action<string> callback)
	{
		listeners += callback;
	}

	public void broadcast(string s)
	{
		listeners(s);
	}
}
