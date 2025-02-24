using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class represents a single, unique item.
/// It is constructed from an ItemData that defines its properties.
/// Because it is created for each item, it can store per-item variables.
/// Inherit from this whenever you need to store new per-item variables.
/// Ensure that a corresponding ItemData is inherited as well.
/// </summary>
public class ItemInstance
{
	public ItemData ItemData;
	public bool Rotated;
	
	public ItemInstance(ItemData itemData) {
		ItemData = itemData;
	}

	public Vector2Int OriginalSize => ItemData.ItemSize;
	public Vector2Int Size {
		get {
			if (ItemData == null) return Vector2Int.zero;
			return Rotated
				? new Vector2Int(ItemData.ItemSize.y, ItemData.ItemSize.x)
				: ItemData.ItemSize;
		}
	}
	
	public int StackCount { get; private set; }
	public void AddToStack(int amount) {
		if (!ItemData.IsStackable) {
			Debug.LogError("Tried to add to stack of non-stackable item: " + ItemData.ItemName);
		}
		StackCount += amount;
	}
	
	public virtual void OnItemDestruction() {
		Debug.Log("Item destroyed: " + ItemData.ItemName);
	}
	
	public virtual void OnHotbarSelected(InventoryHotbarSlot hotbarSlot) {
		Debug.Log("Item selected: " + ItemData.ItemName);
	}
	
	public virtual void OnHotbarDeselected(InventoryHotbarSlot hotbarSlot) {
		Debug.Log("Item deselected: " + ItemData.ItemName);
	}
}