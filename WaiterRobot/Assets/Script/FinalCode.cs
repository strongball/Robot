using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FinalCode : MonoBehaviour
{
    public GameObject leftDisplay;
    public GameObject rightDisplay;
    public GameObject answerDisplay;

    Text minText;
    Text maxText;
    Text answer;

    int min;
    int max;
    int bomb;
    // Use this for initialization
    void Start () {
        answer = answerDisplay.GetComponent<Text>();
        minText = leftDisplay.GetComponent<Text>();
        maxText = rightDisplay.GetComponent<Text>();
        min = 0;
        max = 99;
    }
	
	// Update is called once per frame
	void Update () {
        minText.text = min.ToString();
        maxText.text = max.ToString();
    }

    public void onStart()
    {
        IntentManager.addDialogListener(Dialog);
        initGame();
    }

    public void onClose()
    {
        IntentManager.removeDialogListener(Dialog);
    }

    public void initGame()
    {
        min = 0;
        max = 99;
        bomb = Mathf.FloorToInt(Random.Range(0f, 99f));
    }

    public void guess(int number)
    {
        answer.text = number.ToString();
    }

    public void checkAnswer(bool choice)
    {
        int number;
        if (choice && int.TryParse(answer.text, out number) && number > min && number < max)
        {
            if (number == bomb)
            {
                EndGame();
				return;
            }
            else if (number > bomb)
            {
                max = number;
            }
            else if (number < bomb)
            {
                min = number;
            }
			TextToSpeech.Say(min + "到" + max);
			answer.text = "";
        }
        else
        {
            TextToSpeech.Say("你當我不會判斷嗎");
        }
    }
    public void EndGame()
    {
        TextToSpeech.Say("爆炸，掰掰");
		gameObject.GetComponent<UIAdapter>().Open(false);
    }

    public void Dialog(IntentEntity ie)
    {
        float num;
        if(ie.getNumber(out num)){
            guess(Mathf.FloorToInt(num));
        }

        if(ie.intent == Intent.Choice)
        {
            foreach(Entity e in ie.entitys)
            {
                if(e.type == Entity.Choice_Confirm)
                {
                    checkAnswer(true);
                }else if (e.type == Entity.Choice_Cancel)
                {
                    checkAnswer(false);
                }
            }
        }
    }
}
