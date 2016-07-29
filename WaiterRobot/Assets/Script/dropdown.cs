using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public enum VerticalHorizonal{
	Vertical,
	Horizonal
}
public class dropdown : MonoBehaviour
{
	public RectTransform container;
	public VerticalHorizonal openWay;
	public bool isOpen;
	public UnityEvent onClose;

	// Use this for initialization
	void Start()
	{
		container = transform.FindChild("container").GetComponent<RectTransform>();
		isOpen = false;
	}

	// Update is called once per frame
	void Update()
	{
		Vector3 scale = container.localScale;
		if(openWay == VerticalHorizonal.Horizonal)
		{
			scale.x = Mathf.Lerp(scale.x, isOpen ? 1 : 0, Time.deltaTime * 12);
		}else if (openWay == VerticalHorizonal.Vertical)
		{
			scale.y = Mathf.Lerp(scale.y, isOpen ? 1 : 0, Time.deltaTime * 12);
		}
		
		container.localScale = scale;
	}

	public void Open()
	{
		isOpen = !isOpen;
		if (!isOpen)
		{
			onClose.Invoke();
		}
	}
	public void Close()
	{
		isOpen = false;
	}
}