using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class SpinWheel : MonoBehaviour
{
    public List<int> prize;
    public List<AnimationCurve> animationCurves;
    private bool spinning;
    private float anglePerItem;
    private int randomTime;
    private int itemNumber;

    void Start()
    {
        /*spinning = false;
        anglePerItem = 360 / prize.Count;*/
    }

    public void Update()
    {
        /*if (!spinning)
        {

            randomTime = Random.Range(1, 4);
            itemNumber = Random.Range(0, prize.Count);
            float maxAngle = 360 * randomTime + (itemNumber * anglePerItem);

            StartCoroutine(SpinTheWheel(5 * randomTime, maxAngle));
        }*/
    }
    public void StartGame()
    {
        spinning = false;
        anglePerItem = 360 / prize.Count;

        
        randomTime = Random.Range(5,8);
        itemNumber = Random.Range(0, prize.Count);
        float maxAngle = 360 * randomTime + (itemNumber * anglePerItem);

        StartCoroutine(SpinTheWheel(randomTime, maxAngle));
      
    }

    IEnumerator SpinTheWheel(float time, float maxAngle)
    {
        spinning = true;

        float timer = 0.0f;
        float startAngle = transform.eulerAngles.z;
        maxAngle = maxAngle - startAngle;

        int animationCurveNumber = Random.Range(0, animationCurves.Count);
        Debug.Log("Animation Curve No. : " + animationCurveNumber);
        Debug.Log("Time : " + time);

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
    }
}

