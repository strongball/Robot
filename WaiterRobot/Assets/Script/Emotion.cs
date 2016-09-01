using UnityEngine;
using System.Collections;

public class Emotion {
	public static float Score = 50;
    public static float Change = 10;
    const float MAX = 100;
    const float MIN = 0;

	public static void SetEmotion(bool positive)
	{
		if (positive)
		{
			Score += Change;
		}
		else
		{
			Score -= Change;
		}

		SendEmotion();
	}

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
		SendEmotion();
	}
    public static float GetRandomScore()
    {
        float r = Random.Range(-1.5f, 1.5f);
		Debug.Log(Score + r * Change);
        return Score + r * Change;
    }

	public static void SendEmotion()
	{
		JSONObject data = new JSONObject();
		data.AddField("mood", Score);
		data.AddField("power", 87);
		MyWebSocket.Emit("status", data);
	}
}
