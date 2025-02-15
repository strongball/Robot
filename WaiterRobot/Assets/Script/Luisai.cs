﻿using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class Luisai : MonoBehaviour
{

	JsonData itemdata;
	Speech speech;
	TextToSpeech textToSpeech;

    void Start()
	{
		Speech.addListener(callLuis);
        //this.callLuis("我想拍照");
    }


	IEnumerator WaitForRequest(WWW www)
    {
        yield return www;
        itemdata = JsonMapper.ToObject(www.text);
		DialogIntents(itemdata);
    }
    public void callLuis(string input)//input is the input string
    {
		string url = "https://api.projectoxford.ai/luis/v1/application?id=020e4d3e-8694-45b0-b07d-11f2a3cc6a6f&subscription-key=c02b958474214f15bbe7ffd005ef041d&q="
        + System.Uri.EscapeDataString(input);
        Debug.Log(input);
        WWW www = new WWW(url);
        StartCoroutine(WaitForRequest(www));
    }
	public void DialogIntents(JsonData itemdata)
	{
		string s = itemdata["intents"][0]["intent"].ToString();
        List<Entity> es = new List<Entity>();

        for(int i = 0; i < itemdata["entities"].Count; i++)
        {
            var e = itemdata["entities"][i];
            es.Add(new Entity(e["type"].ToString(), e["entity"].ToString()));
        }
        if(itemdata["entities"].Count == 0)
		{
			es.Add(new Entity(Entity.None, Entity.None));
		}

        IntentManager.HandleIntent(new IntentEntity(s, es));
	}
    
    public void EditorSpeech(Text t)
    {
		callLuis(t.text);
    }
}
