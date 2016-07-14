using UnityEngine;
using System.Collections;
using System;

public class OrdermealDialog: IntentHandler
{
    public OrdermealDialog()
    {
        this.intent = Intent.Order;
    }

    public override void FitIntent(IntentEntity ie)
    {
        
    }
}
