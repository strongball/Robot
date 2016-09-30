using UnityEngine;
using System.Collections;
using System.IO;
using LitJson;

public class ReadResponse : MonoBehaviour {
    private string filePath;
    private static string jsonString;
    private static JsonData jsonData;
	private static int wantToDo;

	private Action actionList;
	void Awake()
    {
		wantToDo = -1;
		actionList = GetComponent<Action>();
		filePath = Application.streamingAssetsPath + "/AI_Content_Unicode.json";
        StartCoroutine(ReadJson());
    }
	void Update()
	{
		if(wantToDo > -1)
		{
			actionList.StartAction(wantToDo);
			wantToDo = -1;
		}
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
            if (jsonData.Keys.Contains(ie.intent) && jsonData[ie.intent].Keys.Contains(e.type))
            {
                JsonData arr = jsonData[ie.intent][e.type];
                float randEmo = Emotion.GetRandomScore();
                int best = 0;
                float mind = 1000;
                for (int i = 0; i < arr.Count; i++)
                {
                    float d = Mathf.Abs(int.Parse(arr[i]["emotion"].ToString()) - randEmo);
                    if (d < mind)
                    {
                        mind = d;
                        best = i;
                    }
                }
                TextToSpeech.Say(arr[best]["content"].ToString());
				if (arr[best].Keys.Contains("action")){
					wantToDo = int.Parse(arr[best]["action"].ToString());
				}
            }
        }
    }
}