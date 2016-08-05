using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class UIAdapter : MonoBehaviour {
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Open(bool open)
    {
        if (open)
        {
            if (!gameObject.activeInHierarchy)
            {
                gameObject.SetActive(true);
            }
            gameObject.transform.SetAsLastSibling();
        }
        else
        {
            gameObject.SetActive(false);
            gameObject.transform.SetAsFirstSibling();
        }
    }

    public void toggleOpen()
    {
        if(gameObject.activeInHierarchy &&transform.GetSiblingIndex() == transform.parent.childCount - 1)
        {
            Open(false);
        }
        else
        {
            Open(true);
        }
    }
}
