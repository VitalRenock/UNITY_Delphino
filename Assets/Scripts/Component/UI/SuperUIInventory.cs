using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperUIInventory : MonoBehaviour
{
	public UIInventoryProps UIInventoryProps;
	public List<GameObject> SlotsList;

	public void SetupInventoryGrid(UIInventoryProps uIInventoryProps)
	{
		UIInventoryProps = uIInventoryProps;
	}

	public void UpdateSlots(Inventory inventory)
	{
		Debug.Log("=> UpdateSlots");

		ClearAllSlots();
		GenerateSlots();

		if (inventory == null)
		{
			Debug.LogError("Inventory null!");
			return;
		}
		if (inventory.ItemsList.Count > SlotsList.Count)
		{
			Debug.LogError("Inventory trop grand pour SlotsList");
			return;
		}

		for (int i = 0; i < inventory.ItemsList.Count; i++)
		{
			SuperUISlot currentSlot = SlotsList[i].GetComponent<SuperUISlot>();
			currentSlot.Item = inventory.ItemsList[i];
			currentSlot.OnClickSlot = UIInventoryProps.OnClickSlot;
			currentSlot.InitSlot();
		}
	}

	void GenerateSlots()
	{
		Debug.Log("=> GenerateSlot");

		for (int i = 0; i < UIInventoryProps.Size; i++)
			SlotsList.Add(Instantiate(UIInventoryProps.PrefabForSlot, transform));
	}

	public void ClearAllSlots()
	{
		Debug.Log("=> ClearAllSlots");

		for (int i = 0; i < SlotsList.Count; i++)
		{
			Destroy(SlotsList[i]);
		}
		SlotsList.Clear();
	}
}

[System.Serializable]
public struct UIInventoryProps
{
	public int Size;
	public GameObject PrefabForSlot;
	public ClickSlot OnClickSlot;
}