using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;


public class Factory : MonoBehaviour
{
	[FoldoutGroup("Energy")]
	public Energy Energy;
	[FoldoutGroup("Inventory")]
	public Inventory InputInventory;
	[FoldoutGroup("Inventory")]
	public Inventory OutputInventory;
	[FoldoutGroup("Recipes")]
	public List<RecipeData> RecipesList;
	[FoldoutGroup("Recipes")][ReadOnly]
	public RecipeData RecipeSelected;


	public void FactoryStartStop()
	{
		Debugger.I.DebugMessage("=> FactoryStartStop");

		if (Energy.IsOn == false)
			StartFactory();
		else
			StopFactory();
	}
	void StartFactory()
	{
		Debugger.I.DebugMessage("=> StartFactory");

		TimeManager.I.onMasterTick.AddListener(Energy.Cycle);
		Energy.onCycle.AddListener(delegate { Manufacturing(); });
		Energy.onOverConsumption.AddListener(StopFactory);
		Energy.IsOn = true;

		UIManager.I.FactoryButtonCraft.GetComponent<Image>().color = Color.green;
	}
	void StopFactory()
	{
		Debugger.I.DebugMessage("=> StopFactory");

		TimeManager.I.onMasterTick.RemoveAllListeners();
		Energy.onCycle.RemoveAllListeners();
		Energy.onOverConsumption.RemoveAllListeners();
		Energy.IsOn = false;

		UIManager.I.FactoryButtonCraft.GetComponent<Image>().color = Color.red;
	}

	public void Manufacturing()
	{
		Debugger.I.DebugMessage("=> Manufacturing");

		if (!Energy.IsOn)
			StopFactory();

		for (int i = 0; i < InputInventory.ItemsList.Count; i++)
			if (InputInventory.ItemsList[i] == RecipeSelected.InputItem)
			{
				Debugger.I.DebugMessage("Manufacturing");

				if (OutputInventory.AddItem(RecipeSelected.OutputItem))
					InputInventory.RemoveItem(RecipeSelected.InputItem);

				UIManager.I.UIInventoryFactoryInput.UpdateSlots(InputInventory);
				UIManager.I.UIInventoryFactoryOutput.UpdateSlots(OutputInventory);
				return;
			}
			else
				Debugger.I.DebugMessage("No item to manufacturing!");
	}
}

public enum FactInventoryType
{
	Input,
	Ouput
}