using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuperUISlot : MonoBehaviour
{
	public ItemData Item;
	public ClickSlot OnClickSlot;

	public void InitSlot()
	{
		Debug.Log("=> InitSlot");

		GetComponent<Button>().onClick.AddListener(() => OnClickSlot(this));

		transform.GetChild(0).GetComponent<Image>().sprite = Item.Identity.Icon;
		transform.GetChild(1).GetComponent<Text>().text = Item.Identity.Name;
	}

	public void ClearSlot()
	{
		Debug.Log("=> ClearSlot");

		GetComponent<Button>().onClick.RemoveAllListeners();

		transform.GetChild(0).GetComponent<Image>().sprite = null;
		transform.GetChild(1).GetComponent<Text>().text = null;
	}
}

public delegate void ClickSlot(SuperUISlot slot);