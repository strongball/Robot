using System;
using System.Collections.Generic;


public delegate void WantToDo();
public interface ListenSpeech
{
	void listen(string s);
}

public class TextAndDo
{
	List<string> textBank = new List<string>();
	public WantToDo wantToDo;
	public string name;
	public TextAndDo(string name, string[] ss)
	{
		this.name = name;
		foreach (string s in ss)
		{
			textBank.Add(s);
		}
	}

	public void addText(string s)
	{
		textBank.Add(s);
	}

	public void check(string target)
	{
		foreach (string s in textBank)
		{
			if (target.Contains(s))
			{
				wantToDo();
				return;
			}
		}
	}
}