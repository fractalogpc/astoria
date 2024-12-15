using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using System.Linq;
using NUnit.Framework;
using UnityEngine.UI;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/guides/networkbehaviour
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/

[Serializable]
public class ItemSet
{
	public ItemData _item;
	public int _count;

	public bool Equals(ItemSet otherSet) {
		if (_item != otherSet._item) return false;
		if (_count != otherSet._count) return false;
		return true;
	}
}

/// <summary>
/// This class is used to create a crafting station that can be interacted with by a player.
/// There are way too many for loops in this code. This should be refactored.
/// </summary>
[RequireComponent(typeof(Interactable))]
public class CraftingStationNetworked : NetworkBehaviour
{
	public List<RecipeData> _recipes;
	[SerializeField] private Interactable _interactable;
	[SerializeField] private InventoryComponent _playerInvRepresentation;
	[SerializeField] private InventoryComponent _ingredientInput;
	[SerializeField] private InventoryComponent _outputInventory;
	[SerializeField] private Button _craftButton;
	
	/// <summary>
	/// Add your validation code here after the base.OnValidate(); call.
	/// </summary>
	protected override void OnValidate() {
		base.OnValidate();
	}

	// NOTE: Do not put objects in DontDestroyOnLoad (DDOL) in Awake.  You can do that in Start instead.

	private void Start() { 
		_interactable.OnInteract.AddListener(OnInteract);
		_ingredientInput.OnInventoryChange.AddListener(InputChanged);
		_craftButton.onClick.AddListener(OnCraftButtonClicked);
	}

	private void OnDisable() {
		_interactable.OnInteract.RemoveListener(OnInteract);
	}

	/// <summary>
	/// Should be called by a Interactor event.
	/// </summary>
	public void OnInteract() {
		SyncToPlayerInventory(_playerInvRepresentation);
		_craftButton.interactable = CanCraftAnything();
	}

	// Attached to events in start
	private void OnCraftButtonClicked() {
		if (CanCraftAnything()) {
			TryCraft();
		}
	}
	// Attached to events in start
	private void InputChanged(List<ItemInstance> items) {
		print($"Can craft anything: {CanCraftAnything()}");
		_craftButton.interactable = CanCraftAnything();
	}
	
	public bool CanCraftAnything() {
		List<ItemInstance> ingredients = GetIngredients();
		foreach (RecipeData recipe in _recipes) {
			if (CheckRecipe(ingredients, recipe)) return true;
		}
		return false;
	}
	public bool TryCraft() {
		List<ItemInstance> ingredients = GetIngredients();
		foreach (RecipeData recipe in _recipes) {
			print($"Checking recipe, ingredients: {ingredients}, recipe: {recipe._ingredients}");
			if (!CheckRecipe(ingredients, recipe)) continue;
			_ingredientInput.ClearItems();
			SetOutput(_outputInventory, ItemsListToDatasList(SetListToItemsList(recipe._result)));
			return true;
		}
		Debug.LogWarning($"{gameObject.name} CraftingStationNetworked: No recipe found for the given ingredients. Did you check the recipe before crafting?");
		return false;
	}
	
	public bool CheckRecipe(List<ItemInstance> input, RecipeData recipe) {
		return SetListsAreEqual(ItemsListToSetList(input), recipe._ingredients);
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
				ingredientSet._count += 1;
				found = true;
				break;
			}
			if (!found) {
				ingredientSets.Add(new ItemSet { _item = item.ItemData, _count = 1 });
			}
		}
		return ingredientSets;
	}
	
	private List<ItemInstance> SetListToItemsList(List<ItemSet> sets) {
		List<ItemInstance> items = new();
		foreach (ItemSet set in sets) {
			for (int i = 0; i < set._count; i++) {
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
	
	private void SyncToPlayerInventory(InventoryComponent invComponent) {
		InventoryComponent mainPlayerInventory = NetworkClient.localPlayer.gameObject.GetComponentInChildren<InventoryComponent>();
		print(mainPlayerInventory.InventoryData == null);
		invComponent.CreateInvFromInventoryData(mainPlayerInventory.InventoryData);
	}
	
	private List<ItemInstance> GetIngredients() {
		return _ingredientInput.GetItems();
	}

	private bool SetOutput(InventoryComponent component, List<ItemData> itemsToOutput) {
		if (component.TryAddItemsByData(itemsToOutput) > 0) {
			Debug.LogWarning($"{gameObject.name} CraftingStationNetworked failed to set output items into inventory.");
			component.ClearItems();
		}
		return true;
	}
	
}