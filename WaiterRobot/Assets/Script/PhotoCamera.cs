using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System;

public class PhotoCamera : MonoBehaviour
{
    public GameObject Display;
	int width = 1280;
	int height = 720;

	Material displaySite;
    WebCamTexture back;
	bool isOpen;
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
		isOpen = false;

		transform.FindChild("Button").GetComponent<Button>().onClick.AddListener(() => {
			TakePhoto();
		});

        IntentManager.addDialogListener(Dialog);
	}

    public void onStart()
    {
        displaySite.mainTexture = null;
        displaySite.mainTexture = back;
        back.Play();
    }

    public void onClose()
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
		Debug.Log(filename);
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
