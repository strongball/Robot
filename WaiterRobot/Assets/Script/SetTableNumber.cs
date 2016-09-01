using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SetTableNumber : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void SendTable(Text text)
	{
		MyWebSocket.Emit("setTable", new JSONObject(text.text.ToString()));
	}
}
