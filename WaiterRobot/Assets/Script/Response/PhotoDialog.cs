using UnityEngine;
using System.Collections;

public class PhotoDialog :IntentHandler
{
    public PhotoDialog()
    {
        this.intent = Intent.Photo;
    }

    public override void FitIntent(IntentEntity ie)
    {
        foreach (Entity e in ie.entitys)
        {
            if (e.type == Entity.Intention_Photo)
            {
                GameObject.Find("PhotoCamera").GetComponent<PhotoCamera>().TakePhoto();
            }
        }
    }

}
