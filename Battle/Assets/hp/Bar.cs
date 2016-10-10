using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Bar : MonoBehaviour {

    
    private float fillAmount;

    [SerializeField]
    private float lerpSpeed; 
    [SerializeField]
    private Image content;


    public float MaxValue { get; set; }

    public float Value
    {
        set
        {
            fillAmount = Map(value, 0, MaxValue, 0, 1);
        }
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        HandleBar();
    }
    private void HandleBar()
    {
        if(fillAmount!=content.fillAmount)
        {
             content.fillAmount = Mathf.Lerp(content.fillAmount,fillAmount,Time.deltaTime*lerpSpeed);
        }
       
       
    }
    private float Map(float value,float hpmin,float hpmax,float outMin,float outMax)
    {
        return (value - hpmin) * (outMax - outMin) / (hpmax - hpmin) + outMin;
    }
}
