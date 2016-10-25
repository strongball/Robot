using UnityEngine;
using System.Collections;
using LitJson;

public class Lobby : MonoBehaviour {
    public GameObject bluetooth;
    public GameObject BattleManager;

    bool isPair;
    string playerNumber;
	// Use this for initialization
	void Start () {
        isPair = false;
        MyWebSocket.On("OnPair", OnPair);
    }
	
	// Update is called once per frame
	void Update () {
        if (Bluetooth.IsConnect)
        {

        }
        if (isPair)
        {
            isPair = false;
            BattleManager.GetComponent<Battlemanager>().StartGame(playerNumber);
        }
	}

    public void Ready()
    {
        MyWebSocket.Emit("Ready");
        
    }

    public void OnPair(JsonData jd)
    {       
        Debug.Log(jd.ToString());
        playerNumber = jd.ToString();
        isPair = true;
    }
}
