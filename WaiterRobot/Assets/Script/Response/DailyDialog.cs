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

            
        }
    }
}
