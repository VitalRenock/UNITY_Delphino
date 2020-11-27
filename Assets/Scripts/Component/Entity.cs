using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public class Entity : MonoBehaviour
{
	[TabGroup("Identity")]
	public Identity Identity;
	[TabGroup("Instantiation")]
	public Instantiation Instantiation;

	private void Start()
	{
		if (Instantiation.InstantiateOnStart)
			Instantiation.SetInstantiate(gameObject);
	}

	public void DestroyEntity()
	{
		Debugger.I.DebugMessage("=> DestroyEntity");

		Destroy(gameObject);
	}
}