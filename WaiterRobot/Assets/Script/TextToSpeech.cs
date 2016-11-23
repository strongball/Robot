using UnityEngine;
using System.Collections;
using System;

public class TextToSpeech : MonoBehaviour {
	public GameObject face;
	public GameObject speech;
	AndroidJavaObject activity;

	static AndroidJavaObject toSpeech;
	static float speakTime = 0;
	// Use this for initialization
	void Start () {
#if (UNITY_ANDROID && !UNITY_EDITOR)
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		activity = jc.GetStatic<AndroidJavaObject>("currentActivity");
		toSpeech = activity.Call<AndroidJavaObject>("getSpeech");
#endif
	}

	// Update is called once per frame
	void Update () {
		if(speakTime > 0)
		{
			face.GetComponent<Controller>().speak(speakTime);
			speech.GetComponent<Speech>().TimeRestart(speakTime);
			speakTime = 0;
		}
	}
	public static void Say(string s){
#if (UNITY_ANDROID && !UNITY_EDITOR)
		if(toSpeech != null){
			toSpeech.Call ("say", s);
		}
#endif
		speakTime = s.Length / 4;
		Debug.Log("Say: " + s);
	}
}
