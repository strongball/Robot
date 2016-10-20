using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class RobotControler : MonoBehaviour {
    public GameObject display;

    public float power;
    public float intervalTime;

    int currectSpeed;
    int leftSpeed;
    int rightSpeed;

    int front;
    int turn;
	// Use this for initialization
	void Start () {
        currectSpeed = 0;
        StartCoroutine(FixSendCommand());
    }
	
	// Update is called once per frame
	void Update () {
        if (Bluetooth.IsConnect)
        {
            front = Mathf.RoundToInt(Input.acceleration.y * power - currectSpeed);
            turn = Mathf.RoundToInt(Input.acceleration.x * 100);

            currectSpeed += front;

            leftSpeed = currectSpeed + turn;
            rightSpeed = currectSpeed - turn;
            /*Bluetooth.SendCommand("turn", Mathf.RoundToInt(Input.acceleration.x).ToString());
            Bluetooth.SendCommand("front", Mathf.RoundToInt(Input.acceleration.y).ToString());*/
            display.GetComponent<Text>().text = currectSpeed.ToString();
        }
    }

    public void OnDisable()
    {
        leftSpeed = 0;
        rightSpeed = 0;
    }

    public void SpeedUp(float speedUpRatio, float skillTime)
    {
        power *= speedUpRatio;
        StartCoroutine(EndSpeedUp(speedUpRatio, skillTime));
    }

    IEnumerator FixSendCommand()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(intervalTime);
            Bluetooth.SendCommand("wheel", leftSpeed.ToString() + " " + rightSpeed.ToString());
        }
        
    }

    IEnumerator EndSpeedUp(float speedUpRatio, float skillTime)
    {
        yield return new WaitForSecondsRealtime(skillTime);
        power /= speedUpRatio;
    }
}
