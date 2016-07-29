using UnityEngine;
using System.Collections;

public class StatusUpdata : MonoBehaviour {
	public static int table = 1;

	public float interval = 1;

	float counter;
	// Use this for initialization
	void Start () {
		counter = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(counter > interval)
		{
			JSONObject data = new JSONObject();
			data.AddField("table", table);
			data.AddField("power", 50);
			data.AddField("mood", Emotion.Score);
			MyWebSocket.Emit("status", data);
			counter = 0;
		}
		counter += Time.deltaTime;
	}
}
