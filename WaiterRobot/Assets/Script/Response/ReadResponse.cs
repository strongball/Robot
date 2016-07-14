using UnityEngine;
using System.Collections;
using System.IO;
using LitJson;

public class ReadResponse : MonoBehaviour {
    private string filePath;
    private string jsonString;
    private JsonData jsonData;
    
    void Start()
    {
        filePath = Application.streamingAssetsPath + "/sample.json";
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
        Debug.Log(jsonData["name"]);//(3)
        Debug.Log(jsonData["age"]);
        // Toast.makeText(jsonData["name"].ToString(), false);
        Toast.makeText(jsonData["name"].ToString(), false);
    }
}