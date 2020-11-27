using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
	public int Count = 1;
	public Identity Identity;
	public Instantiation BaseInstantiation;

	[FoldoutGroup("Interactions")]
	public bool IsInteractable;
	[FoldoutGroup("Interactions")]
	public bool IsCollectable;
	[FoldoutGroup("Interactions")]
	public bool IsActivable;
}