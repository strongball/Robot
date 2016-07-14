using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class BlueList : MonoBehaviour {
	public GameObject canvas;
	public GameObject deviceList;
	public GameObject deviceElement;

	bool isOpen;
	GameObject deviceUI;
	Transform displaySite;
	// Use this for initialization
	void Start () {
		isOpen = false;
		deviceUI = Instantiate(deviceList);
		deviceUI.transform.SetParent(canvas.transform, false);
		displaySite = deviceUI.transform.FindChild("DeviceList/Viewport/Content");

		deviceUI.transform.FindChild("StartScan").GetComponent<Button>().onClick.AddListener(() => {
			Bluetooth.StratScan();
		});
		Bluetooth.addDeviceListener(ReDisplay);
#if UNITY_EDITOR
		List <BluetoothDevice> devices = new List<BluetoothDevice> ();
		devices.Add(new BluetoothDevice("make", "45645645646546"));
		ReDisplay (devices);
#endif
	}
	
	// Update is called once per frame
	void Update () {
		if (isOpen)
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				OpenView();
			}
		}
	}

	public void OpenView()
	{
		isOpen = !isOpen;
		deviceUI.SetActive(isOpen);
	}
	public void ReDisplay(List<BluetoothDevice> devices){
		for (int i = 0; i < displaySite.childCount; i++) {
			GameObject go = displaySite.GetChild (i).gameObject;
			Destroy (go);
		}

		foreach (BluetoothDevice bd in devices) {
			GameObject list = Instantiate (deviceElement);
			list.transform.SetParent (displaySite, false);
			string s = "Name: " + bd.name + "\nAddress: " + bd.address;
			list.transform.FindChild ("Text").GetComponent<Text>().text = s;
			list.transform.FindChild ("Button").GetComponent<Button> ().onClick.AddListener (()=>{
				Bluetooth.Connect(bd.address);
			});
		}
	}
}
