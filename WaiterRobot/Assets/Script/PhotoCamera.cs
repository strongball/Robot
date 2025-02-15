﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

using ZXing;
using ZXing.QrCode;

public class PhotoCamera : MonoBehaviour
{
    public GameObject Display;
	public GameObject Black;
	public GameObject PreView;
	public GameObject PreViewPhoto;
	public GameObject QRCodePanel;
	public GameObject QRCode;
	public bool AutoUpload;
	int width = 1280;
	int height = 720;

	Material displaySite;
	Material preViewSite;
    WebCamTexture back;
	Texture2D photo;

	bool showQR;
	string photoUrl;
	// Use this for initialization
	void Awake()
	{
		displaySite = Display.GetComponent<Image>().material;
		preViewSite = PreViewPhoto.GetComponent<Image>().material;
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

		showQR = false;
		MyWebSocket.On("upload", (jd) => {
			photoUrl = jd.ToString();
			Debug.Log(photoUrl);
			showQR = true;
		});
	}
	// Update is called once per frame
	void Update()
	{
		if (showQR)
		{
			showQR = false;
			MakeQRCode(photoUrl);
		}

	}

	void OnEnable()
    {
		PreView.SetActive(false);
		QRCodePanel.SetActive(false);
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
		StartCoroutine(ShowBlack());
		photo = new Texture2D(back.width, back.height);
        photo.SetPixels(back.GetPixels());
        photo.Apply();
		SetPreview();
	}
	IEnumerator ShowBlack()
	{
		Black.SetActive(true);
		yield return new WaitForSeconds(0.2f);
		Black.SetActive(false);
	}

	public void SetPreview()
	{
		preViewSite.mainTexture = photo;
		PreView.SetActive(true);
	}

	public void SavePhoto()
	{
#if (UNITY_ANDROID && !UNITY_EDITOR)
		//string path = Application.persistentDataPath + "/../../../../WaiterRobot/";
		 string path = "/mnt/sdcard/WaiterRobot/";
#else
		string path = Application.persistentDataPath + "/";
#endif
		string filename = "IMG" + calculateSeconds().ToString() + ".png";

		//Encode to a PNG
		byte[] bytes = photo.EncodeToPNG();
		//Write out the PNG. Of course you have to substitute your_path for something sensible
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
		File.WriteAllBytes(path + filename, bytes);
		Toast.makeText("上傳中"+filename, false);
		TextToSpeech.Say("幫你上傳囉");
		Debug.Log(path + filename);

		if (AutoUpload)
		{
			MyWebSocket.SendBytes(bytes);
		}
		PreView.SetActive(false);
	}

	public void MakeQRCode(string s)
	{
		Texture2D encoded = new Texture2D(256, 256);
		Color32[] color32 = useEncode(s, encoded.width, encoded.height);
		encoded.SetPixels32(color32);//設定要顯示的圖片像素
		encoded.Apply();//申請顯示圖片
		QRCode.GetComponent<Image>().material.mainTexture = encoded;
		QRCodePanel.SetActive(true);
		TextToSpeech.Say("掃描可以下載");
	}

	private Color32[] useEncode(string textForEncoding, int width, int height)
	{
		//開始進行編碼動作
		BarcodeWriter writer = new BarcodeWriter
		{
			Format = BarcodeFormat.QR_CODE,//設定格式為QR Code
			Options = new QrCodeEncodingOptions//設定QR Code圖片寬度和高度
			{
				Height = height,
				Width = width
			}
		};
		return writer.Write(textForEncoding);//將字串寫入，同時回傳轉換後的QR Code
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
		if(ie.CheckIntentEntity(Intent.Photo, Entity.Intention_Photo))
		{
			TakePhoto();
		}

		if (PreView.activeInHierarchy)
		{
			if (ie.CheckIntentEntity(Intent.Choice, Entity.Choice_Confirm))
			{
				SavePhoto();
			}else if(ie.CheckIntentEntity(Intent.Choice, Entity.Choice_Cancel))
			{
				PreView.SetActive(false);
			}
		}

		if (QRCodePanel.activeInHierarchy)
		{
			if (ie.CheckIntentEntity(Intent.Choice, Entity.Choice_Confirm))
			{
				QRCodePanel.SetActive(false);
			}
			else if (ie.CheckIntentEntity(Intent.Choice, Entity.Choice_Cancel))
			{
				QRCodePanel.SetActive(false);
			}
		}
    }
}
