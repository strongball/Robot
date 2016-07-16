using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SocketIO;

public class ConnectServer : MonoBehaviour {
	SocketIOComponent socket;
	public RobertType type;
	public string uniqName;
	public float interval;
	float timeCounter;
	// Use this for initialization
	void Start () {
		socket = GetComponent<SocketIOComponent>();
		socket.On("shakehand", (SocketIOEvent e) => {
			register();
		});
		timeCounter = 0;
	}
	
	// Update is called once per frame
	void Update () {
		timeCounter += Time.deltaTime;
		if(timeCounter > interval)
		{
			sendStatus();
			timeCounter = 0;
		}
	}

	void sendStatus()
	{
		Dictionary<string, string> data = new Dictionary<string, string>();
		data["mood"] = Emotion.Score.ToString();
		data["power"] = "100";
		socket.Emit("update", new JSONObject(data));
	}

	void register()
	{
		Dictionary<string, string> data = new Dictionary<string, string>();
		switch (type)
		{
			case RobertType.waiter:
				data["type"] = "waiter";
				break;
			case RobertType.sender:
				data["type"] = "sender";
				break;
		}
		data["name"] = uniqName;

		socket.Emit("register", new JSONObject(data));
	}
}
public enum RobertType
{
	waiter,
	sender
}
