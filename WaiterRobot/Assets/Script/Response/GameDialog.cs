using UnityEngine;
using System.Collections;
using System;

public class GameDialog : IntentHandler
{
    public GameDialog()
    {
        this.intent = Intent.Game;
    }

    public override void FitIntent(IntentEntity ie)
    {
        foreach (Entity e in ie.entitys)
        {
            if (e.type == Entity.Intention_Game)
            {
                //call   game
            }
        }
    }
}
