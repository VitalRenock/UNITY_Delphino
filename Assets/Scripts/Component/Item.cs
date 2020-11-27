using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public class Item : MonoBehaviour
{
	public ItemData ItemData;


	private void Awake()
	{
		// Chargement de SO dans Component Entity.
		Entity entity = GetComponent<Entity>();
		entity.Identity = ItemData.Identity;
		entity.Instantiation = ItemData.BaseInstantiation;
	}
}
