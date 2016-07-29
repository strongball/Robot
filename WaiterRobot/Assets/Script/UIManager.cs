using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour {
	[Range(0.0F, 1.0F)]
	public float closeDelay = 1;
	public List<IntentOpen> intentOpens;

	private float delayCounter;
	// Use this for initialization
	void Awake () {
        IntentManager.addDialogListener(openDialog);
		delayCounter = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (delayCounter > closeDelay && Input.GetKeyDown(KeyCode.Escape) && transform.childCount > 0)
        {
			delayCounter = 0;
            Transform t = transform.GetChild(transform.childCount - 1);
            if (t.gameObject.activeInHierarchy)
            {
                t.GetComponent<UIAdapter>().Open(false);
            }
        }
        else
        {
			delayCounter += Time.deltaTime;
        }
    }

    public void openDialog(IntentEntity ie)
    {
        foreach (IntentOpen io in intentOpens)
        {
            if (ie.intent == io.intent.ToString() && ie.ContianEntity(io.entityType))
            {
                io.UI.GetComponent<UIAdapter>().Open(true);
            }
        }
    }
}
[System.Serializable]
public class IntentOpen
{
    public IntentUnit intent;
    public string entityType;
    public GameObject UI;
}
