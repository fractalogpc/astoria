using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using System.Linq;
using TMPro;
using UnityEngine.UI;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/guides/networkbehaviour
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/

[Serializable]
public class ItemSet
{
	public ItemData _item;
	public int _itemCount;

	public bool Equals(ItemSet otherSet) {
		if (_item != otherSet._item) return false;
		if (_itemCount != otherSet._itemCount) return false;
		return true;
	}
}

/// <summary>
/// This class is used to create a crafting station that can be interacted with by a player.
/// This and supporting classes were all made in under 30 minutes! This is definitely a little rough around the edges.
/// </summary>
[RequireComponent(typeof(Interactable))]
public class CraftingStationNetworked : NetworkBehaviour
{
	public List<RecipeData> Recipes;
	public CraftingItemInfoUI ItemInfoUI;
	public CraftingRecipeGridUI RecipeGridUI;
	public CraftingCountUI CraftCountUI;
	[SerializeField] [ReadOnly] private InventoryComponent _playerInventory;
	[SerializeField] private Interactable _interactable;
	[SerializeField] private Button _craftButton;

	[SerializeField][ReadOnly] private RecipeData _selectedRecipe;
	[SerializeField][ReadOnly] private int _craftCount = 1;
	
	/// <summary>
	/// Add your validation code here after the base.OnValidate(); call.
	/// </summary>
	protected override void OnValidate() {
		base.OnValidate();
	}

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
		_playerInventory = NetworkClient.localPlayer.gameObject.GetComponentInChildren<InventoryComponent>();
		if (firstTime) _playerInventory.OnInventoryChange.AddListener(OnPlayerInventoryChanged);
		ItemInfoUI.Initialize(this);
		RecipeGridUI.Initialize(this);
		CraftCountUI.Initialize(this);
		// Just in case
		_selectedRecipe = null;
		ItemInfoUI.ResetInfo();
		RecipeGridUI.UpdateRecipeGrid();
	}
	
	public void OnPlayerInventoryChanged(List<ItemInstance> items) {
		RecipeGridUI.UpdateRecipeGrid();
		if (_selectedRecipe == null) return;
		if (!CanCraftRecipe(_selectedRecipe, 1)) {
			_selectedRecipe = null;
			ItemInfoUI.ResetInfo();
		}
	}

	public void OnCraftButtonClicked() {
		if (_selectedRecipe == null) return;
		Craft(_selectedRecipe, _craftCount);
	}

	public void SetCraftCount(int count) {
		if (_selectedRecipe == null) return;
		_craftCount = count;
		if (_craftCount < 1) _craftCount = 1;
	}
	
	public void SelectRecipe(RecipeData recipe) {
		_selectedRecipe = recipe;
		_craftCount = 1;
		ItemInfoUI.SetRecipe(recipe);
	}
	
	public bool Craft(RecipeData recipe, int recipeCount) {
		if (!CanCraftRecipe(recipe, recipeCount)) return false;
		for (int i = 0; i < recipe._ingredientSets.Count; i++) {
			ItemSet set = recipe._ingredientSets[i];
			int itemsNeeded = set._itemCount * recipeCount;
			for (int j = 0; j < itemsNeeded; j++) {
				_playerInventory.TryRemoveItemByData(set._item);
			}
		}
		for (int i = 0; i < recipe._resultSets.Count; i++) {
			ItemSet set = recipe._resultSets[i];
			int outputItems = set._itemCount * recipeCount;
			for (int j = 0; j < outputItems; j++) {
				_playerInventory.TryAddItemByData(set._item);
			}
		}
		return true;
	}
	
	public bool CanCraftSelectedRecipe() {
		
		if (_selectedRecipe == null) return false;
		return CanCraftRecipe(_selectedRecipe, _craftCount);
	}
	
	public bool CanCraftRecipe(RecipeData recipe, int recipeCount) {
		List<ItemInstance> ingredients = new(_playerInventory.GetItems());
		
		foreach (ItemSet ingredient in recipe._ingredientSets) {
			int ingredientCountLeft = ingredient._itemCount * recipeCount;
			for (int j = ingredients.Count - 1; j >= 0; j--) {
				if (ingredient._item != ingredients[j].ItemData) continue;
				ingredients.RemoveAt(j);
				ingredientCountLeft--;
				if (ingredientCountLeft == 0) break;
			}
			if (ingredientCountLeft < 0) {
				print($"can't craft {recipe._resultSets[0]._item.ItemName}");
				return false;
			}
		}
		print($"can craft {recipe._resultSets[0]._item.ItemName}");
		return true;
	}
	
	private bool SetListsAreEqual(List<ItemSet> list1, List<ItemSet> list2) {
		if (list1.Count != list2.Count) return false;
		for (int i = 0; i < list1.Count; i++) {
			if (!list1[i].Equals(list2[i])) return false;
		}

		return true;
	}

	private List<ItemSet> ItemsListToSetList(List<ItemInstance> items) {
		List<ItemSet> ingredientSets = new();
		foreach (ItemInstance item in items) {
			bool found = false;
			foreach (ItemSet ingredientSet in ingredientSets.Where(ingredientSet => item.ItemData == ingredientSet._item)) {
				ingredientSet._itemCount += 1;
				found = true;
				break;
			}

			if (!found) ingredientSets.Add(new ItemSet { _item = item.ItemData, _itemCount = 1 });
		}

		return ingredientSets;
	}

	private List<ItemInstance> SetListToItemsList(List<ItemSet> sets) {
		List<ItemInstance> items = new();
		foreach (ItemSet set in sets) {
			for (int i = 0; i < set._itemCount; i++) {
				items.Add(new ItemInstance(set._item));
			}
		}

		return items;
	}

	private List<ItemData> ItemsListToDatasList(List<ItemInstance> items) {
		List<ItemData> ingredientDatas = new();
		foreach (ItemInstance item in items) {
			ingredientDatas.Add(item.ItemData);
		}

		return ingredientDatas;
	}
}