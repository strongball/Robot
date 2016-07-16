using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class IntentEntity
{
    public string intent;
    public List<Entity> entitys;

    public IntentEntity(string intent, List<Entity> entitys)
    {
        this.intent = intent;
        this.entitys = entitys;
    }
}

public class Intent
{
    public static string Order     = "點餐對話";
    public static string Trash     = "垃圾話";
    public static string Ask       = "詢問對話";
    public static string Normal    = "日常對話";
    public static string Photo     = "拍照對話";
    public static string Greeting  = "問候對話";
    public static string Game      = "遊戲對話";
    public static string Service   = "服務對話";
}
public class Entity
{
    public static string Hello = "問候";
    public static string Food = "食物";

    public static string Question = "疑問";
    public static string Question_Age = "疑問::年齡";
    public static string Question_History = "疑問::歷史";
    public static string Question_Weight = "疑問::體重";
    public static string Question_Height = "疑問::身高";
    public static string Question_Who = "疑問::你是誰";
    public static string Question_Ability = "疑問::能力";
    public static string Question_Provide = "疑問::餐點提供與否";
    public static string Question_Situation = "疑問::餐點狀況";

    public static string Number = "數字";
    public static string Number_Food = "數字::餐點份數";
    public static string Number_Game = "數字::遊戲數字";

    public static string Intention = "意圖";
    public static string Intention_Order = "意圖::點餐";
    public static string Intention_Game = "意圖::遊戲";
    public static string Intention_Photo = "意圖::拍照";
    public static string Intention_Help = "意圖::幫助";

    public static string Emotion = "情緒";
    public static string Emotion_Happy = "情緒::喜";
    public static string Emotion_Angry = "情緒::怒";

    public static string Introduce = "介紹";
    public static string Introduce_Food = "介紹::食物";
    public static string Introduce_Game = "介紹::遊戲";
    public static string Introduce_Function = "介紹::功能";

    public static string DailyNoun = "日常名詞";
    public static string DailyNoun_Story = "日常名詞::故事";
    public static string DailyNoun_Joke = "日常名詞::笑話";
    public static string DailyNoun_Weather = "日常名詞::天氣";
    public static string DailyNoun_GhostStory = "日常名詞::鬼故事";
    public static string DailyNoun_Position = "日常名詞::地點";

    public string type;
    public string name;
    public Entity(string type, string name)
    {
        this.type = type;
        this.name = name;
    }
    

}

public abstract class IntentHandler
{
    public string intent;
    public void CheckAndHandle(IntentEntity ie)
    {
        if (ie.intent == intent)
        {
            FitIntent(ie);
        }
    }
    public abstract void FitIntent(IntentEntity ie);
}

public class IntentManager
{
    List<IntentHandler> handlers;
    public IntentManager()
    {
        handlers = new List<IntentHandler>();
        handlers.Add(new OrdermealDialog());
        handlers.Add(new DailyDialog());
    }
    public void HandleIntent(IntentEntity ie)
    {
        Emotion.DetectEmotion(ie);
        ReadResponse.Response(ie);
        foreach (IntentHandler ih in handlers)
        {
            ih.CheckAndHandle(ie);
        }
    }
}