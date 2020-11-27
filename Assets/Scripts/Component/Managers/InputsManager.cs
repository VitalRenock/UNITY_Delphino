using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class InputsManager : Singleton<InputsManager>
{
	[TabGroup("Keycode")]
	public KeyCode InteractKey;
	[TabGroup("Keycode")]
	public KeyCode DropedKey;
	[TabGroup("Keycode")]
	public KeyCode OpenInventoryKey;

	[TabGroup("Events Keyboard")]
	public UnityEvent onInteractKeyDown = new UnityEvent();
	[TabGroup("Events Keyboard")]
	public UnityEvent onDropedKeyDown = new UnityEvent();
	[TabGroup("Events Keyboard")]
	public UnityEvent onOpenInventoryKeyDown = new UnityEvent();

	[TabGroup("Events Mouse")]
	public UnityEvent onInteractMouse0Down = new UnityEvent();
	[TabGroup("Events Mouse")]
	public UnityEvent onInteractMouse1Down = new UnityEvent();

	private void Update()
	{
		if (Input.GetKeyDown(InteractKey))
			onInteractKeyDown.Invoke();

		if (Input.GetMouseButtonDown(0))
			onInteractMouse0Down.Invoke();

		if (Input.GetMouseButtonDown(1))
			onInteractMouse1Down.Invoke();

		if (Input.GetKeyDown(DropedKey))
			onDropedKeyDown.Invoke();

		if (Input.GetKeyDown(OpenInventoryKey))
			onOpenInventoryKeyDown.Invoke();
	}
}
