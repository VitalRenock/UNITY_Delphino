using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : Singleton<Debugger>
{
	public bool EnableDebugger = false;

	public void DebugMessage(string sentence)
	{
		if (EnableDebugger)
			Debug.Log(sentence);
	}
	public void DebugWarning(string sentence)
	{
		if (EnableDebugger)
			Debug.LogWarning(sentence);
	}
	public void DebugError(string sentence)
	{
		if (EnableDebugger)
			Debug.LogError(sentence);
	}
}
