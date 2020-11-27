using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Dropdown))]
public class SuperUIDropdown : MonoBehaviour
{
	public Dropdown _Dropdown;
	public DropdownOnValueChanged OnValueChanged;

	private void Awake()
	{
		_Dropdown = GetComponent<Dropdown>();
	}

	public void SetupDropdown(List<string> optionsList, bool addOptions)
	{
		Debug.Log("=> SetupDropdown");

		if (!addOptions)
			ClearOptions();

		_Dropdown.AddOptions(optionsList);
		_Dropdown.onValueChanged.AddListener(delegate { OnValueChanged.Invoke(); });
	}

	public void ClearOptions()
	{
		Debug.Log("=> DropdownClear");

		_Dropdown.ClearOptions();
	}
}

public delegate void DropdownOnValueChanged();