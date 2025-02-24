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
	
	public ItemInstance(ItemData itemData) {
		ItemData = itemData;
	}
	
	public virtual void OnItemDestruction() {
		Debug.Log("Item destroyed: " + ItemData.ItemName);
	}
	
	public virtual void OnHotbarSelected() {
		Debug.Log("Item selected: " + ItemData.ItemName);
	}
	
	public virtual void OnHotbarDeselected() {
		Debug.Log("Item deselected: " + ItemData.ItemName);
	}
}