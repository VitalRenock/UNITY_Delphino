using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICompanion
{
	#region Panel

	public void OpenClosePanel(GameObject panel)
	{
		Debug.Log("=> OpenClosePanel");

		if (panel == null)
		{
			Debug.LogError("Panel est null!");
			return;
		}

		if (panel.activeSelf == false)
		{
			panel.SetActive(true);

			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}
		else
		{
			panel.SetActive(false);

			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}
	}

	#endregion
}