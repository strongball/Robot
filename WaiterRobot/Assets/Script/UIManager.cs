using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour {
    public float closeDelay = 1;
    public List<IntentOpen> intentOpens;
	// Use this for initialization
	void Awake () {
        IntentManager.addDialogListener(openDialog);
	}
	
	// Update is called once per frame
	void Update () {
        if (closeDelay > 1&& Input.GetKeyDown(KeyCode.Escape) && transform.childCount > 0)
        {
            closeDelay = 0;
            Transform t = transform.GetChild(transform.childCount - 1);
            if (t.gameObject.activeInHierarchy)
            {
                t.GetComponent<UIAdapter>().Open(false);
            }
        }
        else
        {
            closeDelay += Time.deltaTime;
        }
    }

    public void openDialog(IntentEntity ie)
    {
        foreach (IntentOpen io in intentOpens)
        {
            if (ie.intent == io.intent && ie.ContianEntity(io.entityType))
            {
                io.UI.GetComponent<UIAdapter>().Open(true);
            }
        }
    }
}
[System.Serializable]
public class IntentOpen
{
    public string intent;
    public string entityType;
    public GameObject UI;
}