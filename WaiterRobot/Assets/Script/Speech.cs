using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;

public class Speech : MonoBehaviour
{
	AndroidJavaObject toText;
	public GameObject status;

	static Action<string> listeners;
	// Use this for initialization
	void Start()
	{
		//listeners = new List<Action<string>>();
#if (UNITY_ANDROID && !UNITY_EDITOR)
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject activity = jc.GetStatic<AndroidJavaObject>("currentActivity");
		toText = activity.Call<AndroidJavaObject>("getToText");
#endif

	}
	void messgae(string s)
	{
		Toast.makeText(s, false);
		broadcast(s);
	}
	void statusListener(string s)
	{
		status.GetComponent<Text>().text = s;
		if (s == "Result")
		{
			StartListen();
		}
	}
	// Update is called once per frame
	void Update()
	{

	}
	public void StartListen()
	{
#if (UNITY_ANDROID && !UNITY_EDITOR)
		toText.Call("restartListening");
#endif
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
