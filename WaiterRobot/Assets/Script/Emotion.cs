using UnityEngine;
using System.Collections;

public class Emotion{
    public static float Score = 50;
    public static float Change = 10;
    const float MAX = 100;
    const float MIN = 100;
    public static void DetectEmotion(IntentEntity ie)
    {
        foreach (Entity e in ie.entitys)
        {
            if(e.type == Entity.Emotion_Happy)
            {
                Score += Change;
                if(Score > MAX)
                {
                    Score = MAX;
                }
            }
            if(e.type == Entity.Emotion_Angry)
            {
                Score -= Change;
                if(Score < MIN)
                {
                    Score = MIN;
                }
            }
        }
    }
    public static float GetRandomScore()
    {
        float r = Random.Range(-1.5f, 1.5f);
        return Score + r * Change;
    }
}
