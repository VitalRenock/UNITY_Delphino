using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "New Recipe")]
public class RecipeData: ScriptableObject
{
	public string RecipeName;
	[AssetsOnly]
	public ItemData InputItem;
	[AssetsOnly]
	public ItemData OutputItem;
}