using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class IntentEntity
{
    public string intent;
    public List<Entity> entitys;
    public float numValue;

    public IntentEntity(string intent, List<Entity> entitys)
    {
        this.intent = intent;
        this.entitys = entitys;
    }

    public bool ContianEntity(string type)
    {
        foreach(Entity e in entitys)
        {
            if(e.type == type)
            {
                return true;
            }
        }
        return false;
    }
    public bool getNumber(out float value)
    {
        foreach(Entity e in entitys)
        {
            if (e.getNumber(out value))
            {
                return true;
            }
        }
        value = 0;
        return false;
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
    public static string Choice    = "確認回應";
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

    public static string Choice_Confirm = "確認回應::肯定";
    public static string Choice_Cancel = "確認回應::否定";
    Dictionary<string, float> Chinese_Number =
    new Dictionary<string, float>()
    { { "一", 1 },
      { "二", 2 },
      { "三", 3 },
      { "四", 4 },
      { "五", 5 },
      { "六", 6 },
      { "七", 7 },
      { "八", 8 },
      { "九", 9 },
      { "十", 10 },};

    public string type;
    public string name;
    public Entity(string type, string name)
    {
        this.type = type;
        this.name = name;
    }

    public bool getNumber(out float value)
    {
        if (type == Entity.Number)
        {
            if (!float.TryParse(name, out value))
            {
                value = Chinese_Number[name];
            }
            return true;
        }
        value = 0;
        return false;
    }
}

public class IntentManager
{
    static Action<IntentEntity> dialogListener;

    public IntentManager()
    {

    }
    public static void HandleIntent(IntentEntity ie)
    {
        float number;
        Emotion.DetectEmotion(ie);
        ReadResponse.Response(ie);
        dialogListener(ie);
        /*if(ie.getNumber(out number))
        {
            numberListener(Mathf.FloorToInt(number));
        }*/
    }
    public static void addDialogListener(Action<IntentEntity> l)
    {
        dialogListener += l;
    }
    public static void removeDialogListener(Action<IntentEntity> l)
    {
        dialogListener -= l;
    }
}