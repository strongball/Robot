using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WaiterCtrl : MonoBehaviour {
	public GameObject Emotion;
	public GameObject Table;

	public void SetTable(string s)
	{
		Table.GetComponent<Text>().text = s;
	}

	public void SetEmotion(int s)
	{
		Emotion.GetComponent<Text>().text = s.ToString();
	}
}
