using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {

	// Use this for initialization
	void Start () {
		IntentManager.addDialogListener(moveDialog);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void moveDialog(IntentEntity ie)
	{
		if (ie.intent == Intent.Move)
		{
			if (ie.ContianEntity(Entity.Move_Forward))
			{
				Bluetooth.SendToDevice("forward");
			}else if (ie.ContianEntity(Entity.Move_Backward))
			{
				Bluetooth.SendToDevice("backward");
			}
			else if (ie.ContianEntity(Entity.Move_Left))
			{
				Bluetooth.SendToDevice("left");
			}
			else if (ie.ContianEntity(Entity.Move_Right))
			{
				Bluetooth.SendToDevice("right");
			}
		}
	}
}
