﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

public class BluetoothDevice{
	public string name;
	public string address;
	public BluetoothDevice(string name, string address){
		this.name = name;
		this.address = address;
	}
}

public class Bluetooth : MonoBehaviour
{
	static AndroidJavaObject bluetooth;
	static Action<List<BluetoothDevice>> deviceListeners;
    static public bool IsConnect = false;
    static Action<string> messageListener;

    public UnityEvent onConnect;
	List<BluetoothDevice> devices;
	BlueList blist;
   
	// Use this for initialization
	void Start () {
#if (UNITY_ANDROID && !UNITY_EDITOR)
		devices = new List<BluetoothDevice>();
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject activity = jc.GetStatic<AndroidJavaObject>("currentActivity");
		bluetooth = activity.Call<AndroidJavaObject>("getBluetooth");
#endif
#if (UNITY_EDITOR)
        OnConnect("EDED");
#endif
    }
    public static void StratScan(){
		#if (UNITY_ANDROID && !UNITY_EDITOR)
		bluetooth.Call ("startDiscovery");
		#endif
	}

	public static void Connect(string address){
		#if (UNITY_ANDROID && !UNITY_EDITOR)
		bluetooth.Call("connectBluetooth", address);
		#endif
	}
	public static void OpenBluetooth()
	{

#if (UNITY_ANDROID && !UNITY_EDITOR)
		if(bluetooth != null){
			bluetooth.Call("openBluetooth");
		}
#endif
	}

	public static void SendToDevice(string message){
#if (UNITY_ANDROID && !UNITY_EDITOR)
		if(bluetooth != null && IsConnect){
			bluetooth.Call("sendToDevice", message);
		}
#endif
    }
    public static void SendCommand(string cmd, string value)
    {
        SendToDevice(cmd + " " + value + ";");
    }
    // Update is called once per frame
    void Update () {
		
	}
	void receiver(string s)
	{
		//Toast.makeText(s, false);
        messageListener(s);
    }
	void OnConnect(string s)
	{
		Toast.makeText("connect: "+s, false);
        IsConnect = true;
        onConnect.Invoke();
    }
	void NotOpen(string s)
	{
		Toast.makeText("Bluetooth not open", false);
		OpenBluetooth();
	}

	public static void addDeviceListener(Action<List<BluetoothDevice>> listener)
	{
		deviceListeners += listener;
	}

	void findDevice(string s)
	{
#if (UNITY_ANDROID && !UNITY_EDITOR)
		AndroidJavaObject deviceArray = bluetooth.Call<AndroidJavaObject>("getDevices");
		int size = deviceArray.Call<int>("size");
		devices.Clear();
		for (int i = 0; i < size; i++)
		{
			AndroidJavaObject device = deviceArray.Call<AndroidJavaObject>("get", i);
			string name = device.Call<string>("getName");
			string address = device.Call<string>("getAddress");
			devices.Add(new BluetoothDevice(name, address));
		}
#endif
		deviceListeners(devices);
	}

    public static void AddMessageListener(Action<string> listener)
    {
#if (UNITY_ANDROID && !UNITY_EDITOR)
		messageListener+=listener;
#endif
    }
    public static void RemoveMessageListener(Action<string> listener)
    {
#if (UNITY_ANDROID && !UNITY_EDITOR)
		messageListener-=listener;
#endif
    }
}