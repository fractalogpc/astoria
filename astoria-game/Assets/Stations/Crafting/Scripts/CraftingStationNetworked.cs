using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Mirror;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/guides/networkbehaviour
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/

/// <summary>
/// This class is used to create a crafting station that can be interacted with by a player.
/// This and supporting classes were all made in under 30 minutes! This is definitely a little rough around the edges.
/// </summary>
[RequireComponent(typeof(Interactable))]
// public class CraftingStationNetworked : NetworkBehaviour
public class CraftingStationNetworked : MonoBehaviour
{
	public RecipeLibrary Recipes;
	public CraftingItemInfoUI ItemInfoUI;
	public CraftingRecipeGridUI RecipeGridUI;
	public CraftingCountUI CraftCountUI;
	[SerializeField, ReadOnly] private InventoryComponent _playerInventory;
	[SerializeField] private Interactable _interactable;
	[SerializeField] private Button _craftButton;

	[ReadOnly] public RecipeData SelectedRecipe { get; private set; }
	[ReadOnly] public int SelectedCraftCount { get; private set; }

	// NOTE: Do not put objects in DontDestroyOnLoad (DDOL) in Awake.  You can do that in Start instead.

	private void Start() {
		_interactable.OnInteract.AddListener(OnInteract);
		_craftButton.onClick.AddListener(OnCraftButtonClicked);
	}

	private void OnDisable() {
		_interactable.OnInteract.RemoveListener(OnInteract);
		_craftButton.onClick.RemoveListener(OnCraftButtonClicked);
		if (_playerInventory != null) _playerInventory.OnInventoryChange.RemoveListener(OnPlayerInventoryChanged);
	}

	/// <summary>
	/// Should be called by a Interactor event.
	/// </summary>
	public void OnInteract() {
		// Because the player inventory won't exist at start, but definitely does exist if the player can interact with the station
		bool firstTime = _playerInventory == null;
		_playerInventory = PlayerInstance.Instance.gameObject.GetComponentInChildren<InventoryComponent>();
		if (firstTime) _playerInventory.OnInventoryChange.AddListener(OnPlayerInventoryChanged);
		ItemInfoUI.Initialize(this);
		RecipeGridUI.Initialize(this);
		CraftCountUI.Initialize(this);
		// Just in case
		SelectedRecipe = null;
		ItemInfoUI.ResetInfo();
		RecipeGridUI.UpdateRecipeGrid();
	}
	
	public void OnPlayerInventoryChanged(List<ItemInstance> items) {
		RecipeGridUI.UpdateRecipeGrid();
		if (SelectedRecipe == null) return;
		if (!CanCraftRecipe(SelectedRecipe, 1)) {
			SelectedRecipe = null;
			ItemInfoUI.ResetInfo();
		}
	}

	public void OnCraftButtonClicked() {
		if (SelectedRecipe == null) return;
		Craft(SelectedRecipe, SelectedCraftCount);
		CraftCountUI.UpdateUI();
	}

	public void SetCraftCount(int count) {
		if (SelectedRecipe == null) return;
		SelectedCraftCount = count;
		if (SelectedCraftCount < 1) SelectedCraftCount = 1;
	}
	
	public void SelectRecipe(RecipeData recipe) {
		SelectedRecipe = recipe;
		SelectedCraftCount = 1;
		ItemInfoUI.SetRecipe(recipe);
		CraftCountUI.UpdateUI();
	}
	
	public bool Craft(RecipeData recipe, int craftCount) {
		if (!CanCraftRecipe(recipe, craftCount)) return false;
		foreach (ItemSet ingredientSet in recipe._ingredientSetList.ItemSets) {
			int itemsNeeded = ingredientSet.ItemCount * craftCount;
			for (int i = 0; i < itemsNeeded; i++) {
				_playerInventory.RemoveItemByData(ingredientSet.ItemData);
			}
		}
		foreach (ItemSet resultSet in recipe._resultSetList.ItemSets) {
			int outputItems = resultSet.ItemCount * craftCount;
			for (int j = 0; j < outputItems; j++) {
				_playerInventory.AddItemByData(resultSet.ItemData);
			}
		}
		return true;
	}
	
	public bool CanCraftRecipe(RecipeData recipe, int craftCount) {
		return recipe._ingredientSetList.ContainedWithin(_playerInventory.GetItems(), craftCount);
	}
}