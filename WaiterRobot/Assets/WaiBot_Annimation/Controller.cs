using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
	public Animator Anim;
	public AnimatorStateInfo BS;
	static int Speak = Animator.StringToHash ("Base Layer.Speak");
	static int NormaltoHappy = Animator.StringToHash ("Base Layer.Normal_to_Happy");
	static int NormaltoAngry = Animator.StringToHash ("Base Layer.Normal_to_Angry");
	static int NormaltoSad = Animator.StringToHash ("Base Layer.Normal_to_Sad");

	static int HappytoNormal = Animator.StringToHash ("Base Layer.Happy_to_Normal");
	static int AngrytoNormal = Animator.StringToHash ("Base Layer.Angry_to_Normal");
	static int SadtoNormal = Animator.StringToHash ("Base Layer.Sad_to_Normal");
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Anim.SetBool ("NormaltoAngry", false);
		Anim.SetBool ("NormaltoHappy", false);
		Anim.SetBool ("NormaltoSad", false);
		Anim.SetBool ("HappytoNormal", false);
		Anim.SetBool ("AngrytoNormal", false);
		Anim.SetBool ("SadtoNormal", false);
		Anim.SetBool ("NormaltoSpeak", false);
		if (Input.GetKey (KeyCode.H)) 
		{
			happy ();
		}
		if (Input.GetKey (KeyCode.N)) 
		{
			emotion_to_normal ();
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
		}
			

	}

	void angry(){
		Anim.SetBool ("NormaltoAngry", true);
	}

	void speak(){
		Anim.SetBool ("NormaltoSpeak", true);
	}

	void happy(){
		Anim.SetBool ("NormaltoHappy", true);
	}

	void sad(){
		Anim.SetBool ("NormaltoSad", true);
	}

	void emotion_to_normal(){
		Anim.SetBool ("HappytoNormal", true);
		Anim.SetBool ("AngrytoNormal", true);
		Anim.SetBool ("SadtoNormal", true);
	}
}
