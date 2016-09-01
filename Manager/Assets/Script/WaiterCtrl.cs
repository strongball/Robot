using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WaiterCtrl : MonoBehaviour {
	public GameObject Table;
	public GameObject Power;
	public GameObject Emotion;
	

	public void SetTable(string s)
	{
		Table.GetComponent<Text>().text = s;
	}

	public void SetEmotion(int s)
	{
		Emotion.GetComponent<Text>().text = s.ToString();
	}

	public void SetPower(string s)
	{
		Power.GetComponent<Text>().text = s;
	}
}
