using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;


public class UIManager : Singleton<UIManager>
{
	[TabGroup("Player Inventory")]
	public Inventory PlayerInventory;
	[TabGroup("Player Inventory")]
	public GameObject PanelInventory;
	[TabGroup("Player Inventory")]
	public SuperUIInventory UIInventoryPlayer;
	[TabGroup("Player Inventory")]
	public UIInventoryProps PlayerInvUIProps;

	[TabGroup("Factory Inventory")]
	public GameObject PanelFactory;
	[TabGroup("Factory Inventory")]
	public SuperUIInventory UIInventoryFactoryInput;
	[TabGroup("Factory Inventory")]
	public SuperUIInventory UIInventoryFactoryOutput;
	[TabGroup("Factory Inventory")]
	public SuperUIDropdown DropdownFactory;
	[TabGroup("Factory Inventory")]
	public Button FactoryButtonCraft;
	[TabGroup("Factory Inventory")]
	public UIInventoryProps FactoryInvInUIProps;
	[TabGroup("Factory Inventory")]
	public UIInventoryProps FactoryInvOutUIProps;

	UICompanion UICompanion = new UICompanion();
	[ReadOnly]
	public Factory _TempFactory;
	public Text TestTime;
	public Image HealBar;

	private void Start()
	{
		InputsManager.I.onOpenInventoryKeyDown.AddListener(OpenCloseInventoryPanel);

		PlayerInvUIProps.OnClickSlot += ActionSlotInvPlayer;
		UIInventoryPlayer.SetupInventoryGrid(PlayerInvUIProps);

		FactoryInvInUIProps.OnClickSlot += ActionSlotInvFacInput;
		UIInventoryFactoryInput.SetupInventoryGrid(FactoryInvInUIProps);

		FactoryInvOutUIProps.OnClickSlot += ActionSlotInvFacOutput;
		UIInventoryFactoryOutput.SetupInventoryGrid(FactoryInvOutUIProps);

		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}
	private void Update()
	{
		TestTime.text = "Current Time: " + TimeManager.I.CurrentTime;
	}

	public void OpenCloseInventoryPanel()
	{
		Debug.Log("=> OpenCloseInventoryPanel");

		UICompanion.OpenClosePanel(PanelInventory);

		if (PanelInventory.activeSelf)
		{
			UIInventoryPlayer.UpdateSlots(PlayerInventory);

			PlayerInventory.GetComponent<PlayerMotor>().CanMove = false;
			PlayerInventory.GetComponent<PlayerMotor>().CanTurn = false;
			PlayerInventory.transform.GetChild(0).GetComponent<CamPlayerMotor>().CanTurn = false;
		}
		else
		{
			UIInventoryPlayer.ClearAllSlots();

			PlayerInventory.GetComponent<PlayerMotor>().CanTurn = true;
			PlayerInventory.GetComponent<PlayerMotor>().CanMove = true;
			PlayerInventory.transform.GetChild(0).GetComponent<CamPlayerMotor>().CanTurn = true;
		}
	}
	public void OpenCloseFactoryPanel(RaycastHit hit)
	{
		Debug.Log("=> OpenCloseFactoryPanel");

		Factory factory = hit.transform.GetComponent<Factory>();

		UICompanion.OpenClosePanel(PanelFactory);
		OpenCloseInventoryPanel();

		if (PanelFactory.activeSelf)
		{
			_TempFactory = factory;

			UIInventoryFactoryInput.UpdateSlots(factory.InputInventory);
			UIInventoryFactoryOutput.UpdateSlots(factory.OutputInventory);

			DropdownFactSetup();
			FactoryButtonCraft.onClick.AddListener(factory.FactoryStartStop);
		}
		else
		{
			_TempFactory = null;

			UIInventoryFactoryInput.ClearAllSlots();
			UIInventoryFactoryOutput.ClearAllSlots();

			DropdownFactory.ClearOptions();
			FactoryButtonCraft.onClick.RemoveAllListeners();
		}
	}

	void ActionSlotInvPlayer(SuperUISlot slot)
	{
		Debug.Log("=> ActionSlotInvPlayer");

		if (PanelFactory.activeSelf == true)
			if (slot.Item != null)
			{
				PlayerInventory.RemoveItem(slot.Item);
				_TempFactory.InputInventory.AddItem(slot.Item);
				slot.ClearSlot();

				UIInventoryPlayer.UpdateSlots(PlayerInventory);
				UIInventoryFactoryInput.UpdateSlots(_TempFactory.InputInventory);
			}
			else
			{
				Debug.Log("Remove");
				Debug.Log(slot.Item.name);
				PlayerInventory.RemoveItem(slot.Item);
				slot.ClearSlot();
				UIInventoryPlayer.UpdateSlots(PlayerInventory);
			}
		else
		{
			PlayerInventory.DropItem(slot.Item);
			slot.ClearSlot();
			UIInventoryPlayer.UpdateSlots(PlayerInventory);
		}
	}
	void ActionSlotInvFacInput(SuperUISlot slot)
	{
		Debug.Log("=> ActionSlotInvFacInput");

		if (slot.Item != null)
			if (PanelInventory.activeSelf == true)
			{
				_TempFactory.InputInventory.RemoveItem(slot.Item);
				PlayerInventory.AddItem(slot.Item);
				slot.ClearSlot();

				UIInventoryFactoryInput.UpdateSlots(_TempFactory.InputInventory);
				UIInventoryPlayer.UpdateSlots(PlayerInventory);
			}
	}
	void ActionSlotInvFacOutput(SuperUISlot slot)
	{
		Debug.Log("=> ActionSlotInvFacOutput");

		if (slot.Item != null)
			if (PanelInventory.activeSelf == true)
			{
				_TempFactory.OutputInventory.RemoveItem(slot.Item);
				PlayerInventory.AddItem(slot.Item);
				slot.ClearSlot();

				UIInventoryFactoryOutput.UpdateSlots(_TempFactory.OutputInventory);
				UIInventoryPlayer.UpdateSlots(PlayerInventory);
			}
	}

	void DropdownFactSetup()
	{
		Debug.Log("=> DropdownFactSetup");

		List<string> dropdownOptionTexts = new List<string>();
		foreach (RecipeData recipe in _TempFactory.RecipesList)
			dropdownOptionTexts.Add(recipe.RecipeName);

		DropdownFactory.ClearOptions();
		DropdownFactory.SetupDropdown(dropdownOptionTexts, false);
		DropdownFactory.OnValueChanged += ActionDropdownFactChanged;

		_TempFactory.RecipeSelected = _TempFactory.RecipesList[0];
	}
	void ActionDropdownFactChanged()
	{
		Debug.Log("=> ActionDropdownFactChanged");

		for (int i = 0; i < _TempFactory.RecipesList.Count; i++)
			if (_TempFactory.RecipesList[i].RecipeName == DropdownFactory._Dropdown.captionText.text)
			{
				int index = i;
				_TempFactory.RecipeSelected = _TempFactory.RecipesList[index];
				return;
			}
	}


	Coroutine HealBarLerp;
	public void SetHealBar(float value)
	{
		Debug.Log("=> SetHealBar");

		if (HealBarLerp == null)
			HealBarLerp = StartCoroutine(HealBarLerping(value * 0.01f));
		else
		{
			StopCoroutine(HealBarLerp);
			HealBarLerp = null;
			HealBarLerp = StartCoroutine(HealBarLerping(value * 0.01f));
		}
	}
	IEnumerator HealBarLerping(float value)
	{
		Debug.Log("=> Start HealBarLerping");

		if (value == 0)
		{
			HealBar.fillAmount = value;
			yield break;
		}


		float max = HealBar.fillAmount;
		float time = 0;

		while (HealBar.fillAmount > value)
		{
			HealBar.fillAmount = Mathf.Lerp(max, value, time);
			time += Time.deltaTime * 2;

			yield return null;
		}
	}
}