using UnityEngine;
using System.Collections;
using System.IO;
using LitJson;

public class ReadResponse : MonoBehaviour {
    private string filePath;
    private static string jsonString;
    private static JsonData jsonData;
    
    void Awake()
    {
        filePath = Application.streamingAssetsPath + "/AI_Content_Unicode.json";
        StartCoroutine(ReadJson());
    }
    IEnumerator ReadJson()
    {
        if (filePath.Contains("://"))
        {
            WWW www = new WWW(filePath);
            yield return www;
            jsonString = www.text;
        }
        else
            jsonString = System.IO.File.ReadAllText(filePath);
        jsonData = JsonMapper.ToObject(jsonString);
    }
    public static void Response(IntentEntity ie)
    {
        foreach(Entity e in ie.entitys)
        {
            Debug.Log(e.type);
            if (jsonData.Keys.Contains(ie.intent) && jsonData[ie.intent].Keys.Contains(e.type) && e.type !=  Entity.Number && e.type != Entity.Emotion_Angry && e.type != Entity.Emotion_Happy)
            {
                JsonData arr = jsonData[ie.intent][e.type];
                float randEmo = Emotion.GetRandomScore();
                int best = 0;
                float mind = 1000;
                for (int i = 0; i < arr.Count; i++)
                {
                    float d = int.Parse(arr[i]["emotion"].ToString()) - randEmo;
                    if (d < mind)
                    {
                        mind = d;
                        best = i;
                    }
                }
                Debug.Log(arr[best]["content"]);
                TextToSpeech.Say(arr[best]["content"].ToString());
            }
        }
    }
}