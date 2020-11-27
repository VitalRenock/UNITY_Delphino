using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;


public class Inventory : MonoBehaviour
{
	[TabGroup("Inventory")]
	public List<ItemData> ItemsList;
	[TabGroup("Inventory")]
	public int MaxInventorySize;
	[TabGroup("Inventory")]
	public GameObject ItemBasePrefab;

	[TabGroup("Events")]
	public UnityEvent OnPickupItem = new UnityEvent();
	[TabGroup("Events")]
	public UnityEvent OnDropedItem = new UnityEvent();

	public bool AddItem(ItemData item)
	{
		Debug.Log("=> AddItem");

		bool isCollected = false;

		if (ItemsList.Count < MaxInventorySize)
		{
			ItemsList.Add(item);
			isCollected = true;
		}

		return isCollected;
	}
	public bool RemoveItem(ItemData item)
	{
		Debug.Log("=> RemoveItem");

		bool isRemoved = false;

		if (ItemsList.Contains(item))
		{
			ItemsList.Remove(item);
			isRemoved = true;
		}

		return isRemoved;
	}

	public void PickupItem(RaycastHit hit)
	{
		Debug.Log("=> PickupItem");

		Item item = hit.transform.GetComponent<Item>();

		bool isPickUp = AddItem(item.ItemData);
		if (isPickUp)
		{
			OnPickupItem.Invoke();
			hit.transform.GetComponent<Entity>().DestroyEntity();
		}
	}
	public void DropItem(ItemData item)
	{
		Debug.Log("=> DropItem");

		// Retrait de l'item dans l'inventaire.
		bool isDroped = RemoveItem(item);

		// Instantiation de l'item.
		if (isDroped)
		{
			OnDropedItem.Invoke();

			Vector3 dropedPosition = transform.position + (transform.forward * 3);

			GameObject ItemBase;
			ItemBase = Instantiate(ItemBasePrefab, dropedPosition, Quaternion.identity);
			ItemBase.name = item.Identity.Name;

			Item tempItem = ItemBase.GetComponent<Item>();
			tempItem.ItemData = item;
			tempItem.ItemData.BaseInstantiation.SetInstantiate(ItemBase);
		}
	}
}