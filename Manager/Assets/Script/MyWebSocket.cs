using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using LitJson;
using WebSocketSharp;

public class MyWebSocket : MonoBehaviour {
	public string url = "127.0.0.1:8080";
	public ProtocolType Protocol;
	public bool autoConnect = true;
	public int reconnectDelay = 5;

	public static string IP;
	public static List<SocketListener> eventListeners;
	static MyWebSocket()
	{
		eventListeners = new List<SocketListener>();
	}
	static WebSocket ws;

	private Thread socketThread;
	private volatile bool connected = true;
	// Use this for initialization
	void Awake () {
		ws = new WebSocket("ws://" + url, Protocol.ToString());
		ws.OnOpen += OnOpen;
		ws.OnMessage += OnMessage;
	}
	public void Start()
	{
		if (autoConnect)
		{
			Connect();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void OnDestroy()
	{
		if (socketThread != null) { socketThread.Abort(); }
		ws.Close();
	}
	private void OnOpen(object sender, EventArgs e)
	{
		Debug.Log("OPEN");
		/*JSONObject jo = new JSONObject();
		jo.AddField("type", "waiter");
		jo.AddField("name", "Ball");
		Emit("register", jo);*/
	}

	public void Connect()
	{
		IP = url;
		socketThread = new Thread(RunSocketThread);
		socketThread.Start(ws);
	}
	private void RunSocketThread(object obj)
	{
		WebSocket webSocket = (WebSocket)obj;
		while (connected)
		{
			if (webSocket.IsAlive)
			{
				Thread.Sleep(reconnectDelay);
			}
			else {
				webSocket.Connect();
			}
		}
		webSocket.Close();
	}

	public static void Emit(string ev, JSONObject content)
	{
		JSONObject data = new JSONObject();
		data.AddField("event", ev);
		data.AddField("data", content);
		ws.Send(data.ToString());
	}
	public static void SendBytes(byte[] bytes)
	{
		ws.Send(bytes);
	}

	public static void On(string ev, Action<JsonData> callback)
	{
		eventListeners.Add(new SocketListener(ev, callback));
	}

	public void OnMessage(object sender, MessageEventArgs e)
	{
		JsonData data = JsonMapper.ToObject(e.Data);
		foreach (SocketListener sl in eventListeners)
		{
			sl.Check(data["event"].ToString(), data["data"]);
		}
	}
}

public enum ProtocolType
{
	WaiterRobot,
	manager
}

public class SocketListener
{
	public string EventName;
	public Action<JsonData> callback;
	public SocketListener(string EventName, Action<JsonData> callback)
	{
		this.EventName = EventName;
		this.callback += callback;
	}
	public void Check(string EventName, JsonData data)
	{
		if (this.EventName == EventName)
		{
			callback(data);
		}
	}
}
