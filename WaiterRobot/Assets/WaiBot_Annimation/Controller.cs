using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
	public Animator Anim;
	public AnimatorStateInfo BS;
	static int Normal = Animator.StringToHash ("Base Layer.Normal");
	static int Speak = Animator.StringToHash ("Base Layer.Speak");
	static int NormaltoHappy = Animator.StringToHash ("Base Layer.Normal_to_Happy");
	static int NormaltoAngry = Animator.StringToHash ("Base Layer.Normal_to_Angry");
	static int NormaltoSad = Animator.StringToHash ("Base Layer.Normal_to_Sad");

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		/*if (Input.GetKey (KeyCode.H)) 
		{
			happy ();
		}
		if (Input.GetKey (KeyCode.N)) 
		{
			normal ();
		}

		if (Input.GetKey (KeyCode.A)) 
		{
			angry ();
		}
			
		if (Input.GetKey (KeyCode.S)) 
		{
			sad ();
		}

		if (Input.GetKey (KeyCode.T)) 
		{
			speak ();
		}*/
	}

	public void angry(){
		Anim.SetTrigger ("toAngry");
	}

	public void speak(float time){
		StartCoroutine(speakThread(time));
	}

	IEnumerator speakThread(float time)
	{
		Anim.SetTrigger("toSpeak");
		yield return new WaitForSecondsRealtime(time);
		Anim.SetTrigger("toNormal");
	}

	public void happy(){
		Anim.SetTrigger ("toHappy");
	}

	public void sad(){
		Anim.SetTrigger ("toSad");
	}

	public void normal(){
		Anim.SetTrigger ("toNormal");
	}
		
}
