using UnityEngine;
using UnityEngine.UI;
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
		if(bluetooth != null){
			bluetooth.Call("sendToDevice", message);
		}
#endif
	}
	// Update is called once per frame
	void Update () {
		
	}
	void receiver(string s)
	{
		Toast.makeText(s, false);
	}
	void OnConnect(string s)
	{
		Toast.makeText("connect: "+s, false);
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
}