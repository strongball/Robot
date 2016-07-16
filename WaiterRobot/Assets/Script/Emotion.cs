using UnityEngine;
using System.Collections;

public class Emotion{
    public static int Score = 50;
    public static int Change = 10;

    public static void DetectEmotion(IntentEntity ie)
    {
        foreach (Entity e in ie.entitys)
        {
            if(e.type == Entity.Emotion_Happy)
            {
                Score += Change;
            }
            if(e.type == Entity.Emotion_Angry)
            {
                Score -= Change;
            }
        }
    }
    public static int GetRandomScore()
    {
        float r = Random.Range(-1.5f, 1.5f);
        return Mathf.RoundToInt(Score + r * Change);
    }
}
