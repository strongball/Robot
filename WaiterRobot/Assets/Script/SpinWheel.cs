using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;


[System.Serializable]
public class IntEvent : UnityEvent<string>
{
}
public class SpinWheel : MonoBehaviour
{
	public List<string> prize;
    public List<AnimationCurve> animationCurves;
	public IntEvent OnSpinEnd;
	[Range(0, 100)]
	public int Force;

    private bool spinning;
    private float anglePerItem;
    private int randomTime;
    private int itemNumber;

    void Start()
    {
        spinning = false;
        /*anglePerItem = 360 / prize.Count;*/
    }

    public void Update()
    {
		if(Input.touchCount > 0)
		{
			if(!spinning && Input.GetTouch(0).deltaPosition.y > Force)
			{
				StartGame(true);
			}
			else if(!spinning && Input.GetTouch(0).deltaPosition.y < - Force)
			{
				StartGame(false);
			}
		}
#if UNITY_EDITOR
		if(Input.GetKey(KeyCode.Q) && !spinning)
		{
			StartGame(true);
		}
#endif
	}
	public void StartGame(bool clock)
    {
        spinning = true;
        anglePerItem = 360 / prize.Count;

        
        randomTime = Random.Range(5,8);
        itemNumber = Random.Range(0, prize.Count);
        float maxAngle = 360 * randomTime + (itemNumber * anglePerItem);

        StartCoroutine(SpinTheWheel(randomTime, maxAngle, clock));
      
    }

    IEnumerator SpinTheWheel(float time, float maxAngle, bool clock)
    {
        float timer = 0.0f;
        float startAngle = transform.eulerAngles.z;

        maxAngle = maxAngle - startAngle;

		if (!clock)
		{
			maxAngle  = - maxAngle + 2 * (maxAngle % 360);
		}

		int animationCurveNumber = Random.Range(0, animationCurves.Count);

        while (timer < time)
        {
            //to calculate rotation
            float angle = maxAngle * animationCurves[animationCurveNumber].Evaluate(timer / time);
            transform.eulerAngles = new Vector3(0.0f, 0.0f, angle + startAngle);
            timer += Time.deltaTime;
            yield return 0;
        }

        transform.eulerAngles = new Vector3(0.0f, 0.0f, maxAngle + startAngle);
        spinning = false;  

        //prize[itemNumber] = GetComponent<Text>;
        Debug.Log("Prize: " + prize[itemNumber]);//use prize[itemNumnber] as per requirement
		OnSpinEnd.Invoke(prize[itemNumber]);
	}
}

