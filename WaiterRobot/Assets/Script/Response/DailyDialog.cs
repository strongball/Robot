using UnityEngine;
using System.Collections;
using System;

public class DailyDialog : IntentHandler
{
    public DailyDialog()
    {
        this.intent = Intent.Normal;
    }

    public override void FitIntent(IntentEntity ie)
    {
        foreach (Entity e in ie.entitys)
        {
            if(e.type == Entity.DailyNoun_Story)
            {
                TextToSpeech.Say("我要說故事了喔喔喔喔");
            }
            else if(e.type ==Entity.DailyNoun_Weather)
            {
                TextToSpeech.Say("今天天氣不錯");
            }
            else if(e.type ==Entity.DailyNoun_Joke)
            {
                TextToSpeech.Say("哈哈哈哈哈哈哈哈哈哈哈");
            }
        }
    }
}
