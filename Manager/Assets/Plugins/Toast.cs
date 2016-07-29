using UnityEngine;
using System.Collections;

public class Toast : AndroidBehaviour<Toast>
{
	protected override string javaClassName
	{
		//要呼叫的class所在的Package名稱.要呼叫的java class名稱
		get { return "test.unity.forunity.AndroidToast"; }
	}

	/// <summary>
	/// 使用Toast
	/// </summary>
	/// <param name="message">要顯示的字串</param>
	/// <param name="isLong">true為3.5秒；false為2秒</param>
	public static void makeText(string message, bool isLong)
	{
		#if (UNITY_ANDROID && !UNITY_EDITOR)
		//第一個參數為要呼叫的Method名稱
		instance.CallStatic("showToast", message, isLong);
		#endif
	}
}