using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;


public class SpotItem : MonoBehaviour
{
	[TabGroup("Item")]
	public ItemData ItemOnResource;
	[TabGroup("Item")]
	public GameObject PrefabItem;

	[TabGroup("Events")]
	public UnityEvent OnSpawnResource;


	public void SpawnItem()
	{
		Debugger.I.DebugMessage("=> SpawnResource");

		Vector3 dropedPosition = transform.position + Vector3.down;

		GameObject ItemBase;
		ItemBase = Instantiate(PrefabItem, dropedPosition, Quaternion.identity);
		ItemBase.name = ItemOnResource.Identity.Name;

		Item tempItem = ItemBase.GetComponent<Item>();
		tempItem.ItemData = ItemOnResource;
		tempItem.ItemData.BaseInstantiation.SetInstantiate(ItemBase);
	}
}