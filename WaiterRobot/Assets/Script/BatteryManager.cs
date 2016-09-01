using UnityEngine;
using System.Collections;

public class BatteryManager : MonoBehaviour {
	[Range(1f, 60f)]
	public float intervalTime = 60;
	AndroidJavaObject battery;
	// Use this for initialization
	void Start () {
#if (UNITY_ANDROID && !UNITY_EDITOR)
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject activity = jc.GetStatic<AndroidJavaObject>("currentActivity");
		battery = activity.Call<AndroidJavaObject>("getBattery");
#endif
		StartCoroutine(FixGetPower());
	}

	// Update is called once per frame
	void Update () {
	
	}
	public void GetBattery()
	{
#if (UNITY_ANDROID && !UNITY_EDITOR)
		battery.Call("getBatteryInfo");
#endif
	}

	public void SetPower(string s)
	{
		Debug.Log(s);
		MyWebSocket.Emit("Power", new JSONObject(s));
	}

	IEnumerator FixGetPower()
	{
		while (true)
		{
			GetBattery();
			yield return new WaitForSecondsRealtime(intervalTime);
		}
	}
}
