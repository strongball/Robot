using UnityEngine;
using System.Collections;

public class ServiceDialog : IntentHandler
{
    public ServiceDialog()
    {
        this.intent = Intent.Service;
    }

    public override void FitIntent(IntentEntity ie)
    {
        foreach (Entity e in ie.entitys)
        {
            if (e.type == Entity.Intention_Help)
            {
                //call  help
            }
        }
    }

}
