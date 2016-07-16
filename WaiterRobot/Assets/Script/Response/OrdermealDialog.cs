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
        foreach (Entity e in ie.entitys)
        {
            if (e.type == Entity.Intention_Order)
            {
                //call   
            }
        }
    }
}
