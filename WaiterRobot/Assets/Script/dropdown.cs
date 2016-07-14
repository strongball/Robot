using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class dropdown : MonoBehaviour
{

	public RectTransform container;
	public bool isOpen;

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
		scale.y = Mathf.Lerp(scale.y, isOpen ? 1 : 0, Time.deltaTime * 12);
		container.localScale = scale;
	}

	public void Open()
	{
		isOpen = !isOpen;
	}
}