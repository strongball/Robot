using UnityEngine;
using System.Collections;

public class TextToSpeech : MonoBehaviour {
	AndroidJavaObject activity;
	static AndroidJavaObject toSpeech;
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
	
	}
	public static void Say(string s){
#if (UNITY_ANDROID && !UNITY_EDITOR)
		if(toSpeech != null){
			toSpeech.Call ("say", s);
		}
#endif
	}
}
