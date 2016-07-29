using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WaiterCtrl : MonoBehaviour {
	public GameObject Emotion;

	public void SetEmotion(float s)
	{
		Emotion.GetComponent<Text>().text = s.ToString();
	}
}
