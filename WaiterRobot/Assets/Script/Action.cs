using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Action : MonoBehaviour {
	public List<ActionContent> Actions;
	public void StartAction(int num)
	{
		if(Actions.Count > num)
		{
			Actions[num].DoAction();
		}
	}
}
[System.Serializable]
public class ActionContent
{
	public FacialExpression face;
	public BodyMotion body;
	public ActionContent(FacialExpression face, BodyMotion body)
	{
		this.face = face;
		this.body = body;
	}
	public void DoAction()
	{
		Debug.Log(body.ToString());
		Bluetooth.SendToDevice(body.ToString());
	} 
}

public enum FacialExpression
{
	happy,
	angry,
	sad,
	normal,
	heart
}

public enum BodyMotion
{
	none,
	right_hand_up,
	hands_down,
	hands_up,
	move_hands,
	shake_head,
	turnaround_move,
	dance
}