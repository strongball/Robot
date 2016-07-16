using UnityEngine;
using System.Collections;

public class Emotion{
    public static int Score = 50;
    public static int Passive = 10;
    public static int Negative = -10;

    public static void DetectEmotion(IntentEntity ie)
    {
        foreach (Entity e in ie.entitys)
        {
            if(e.type == Entity.Emotion_Happy)
            {
                Score += Passive;
            }
            if(e.type == Entity.Emotion_Angry)
            {
                Score += Negative;
            }
        }
    }
}
