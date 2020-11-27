using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


[System.Serializable]
public class Identity
{
	[TabGroup("Identity")]
	public string Name;
	[TabGroup("Identity")]
	public string Description;
	[TabGroup("Identity")][PreviewField(50, ObjectFieldAlignment.Left)]
	public Sprite Icon;
}