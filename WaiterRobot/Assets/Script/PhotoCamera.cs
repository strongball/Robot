using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class PhotoCamera : MonoBehaviour
{
    public GameObject Display;
	public bool AutoUpload;
	int width = 1280;
	int height = 720;

	Material displaySite;
    WebCamTexture back;
	// Use this for initialization
	void Awake()
	{
		displaySite = Display.GetComponent<Image>().material;
		WebCamDevice[] devices = WebCamTexture.devices;

		foreach (WebCamDevice cam in devices) { 
			if (cam.isFrontFacing)
			{
				{
					back = new WebCamTexture(cam.name, width, height);
				}
			}
		}

		transform.FindChild("Button").GetComponent<Button>().onClick.AddListener(() => {
			TakePhoto();
		});

        IntentManager.addDialogListener(Dialog);
	}

    void OnEnable()
    {
        displaySite.mainTexture = back;
        back.Play();
    }

    void OnDisable()
    {
        displaySite.mainTexture = null;
        back.Stop();
    }

	public void TakePhoto()
	{
		if (gameObject.activeInHierarchy)
		{
            StartCoroutine(MakePhoto());
        }
        else
        {
            GetComponent<UIAdapter>().Open(true);
        }	     
    }

	public IEnumerator MakePhoto()
    {
		yield return new WaitForEndOfFrame();
#if (UNITY_ANDROID && !UNITY_EDITOR)
		//string path = Application.persistentDataPath + "/../../../../WaiterRobot/";
		 string path = "/mnt/sdcard/WaiterRobot/";
#else
		string path = Application.persistentDataPath + "/";
#endif
		string filename = "IMG" + calculateSeconds().ToString() + ".png";

		// it's a rare case where the Unity doco is pretty clear,
		// http://docs.unity3d.com/ScriptReference/WaitForEndOfFrame.html
		// be sure to scroll down to the SECOND long example on that doco page 

		Texture2D photo = new Texture2D(back.width, back.height);
        photo.SetPixels(back.GetPixels());
        photo.Apply();
		
        //Encode to a PNG
        byte[] bytes = photo.EncodeToPNG();
		//Write out the PNG. Of course you have to substitute your_path for something sensible
		if (! Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
        File.WriteAllBytes(path + filename, bytes);
		Toast.makeText(filename, false);
		Debug.Log(path + filename);
		
		if (AutoUpload)
		{
			MyWebSocket.SendBytes(bytes);
			//StartCoroutine(upload(filename, bytes));
		}	
	}
	IEnumerator upload(string name, byte[] bytes)
	{
		yield return new WaitForEndOfFrame();
		
		WWWForm postForm = new WWWForm();
		postForm.AddBinaryData("file", bytes, name, "image/png");

		WWW upload = new WWW(MyWebSocket.IP + "/upload", postForm);
		yield return upload;
		Debug.Log(upload.text);
	}
	// Update is called once per frame
	void Update () {

	}
    public int calculateSeconds()
    {
        DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);//from 1970/1/1 00:00:00 to now
        DateTime dtNow = DateTime.Now;
        TimeSpan result = dtNow.Subtract(dt);
        int seconds = Convert.ToInt32(result.TotalSeconds);
        return seconds;
    }

    public void Dialog(IntentEntity ie)
    {
        if(ie.intent == Intent.Photo)
        {
            foreach(Entity e in ie.entitys)
            {
                if(e.type == Entity.Intention_Photo)
                {
                    TakePhoto();
                }
            }
        }
    }
}
