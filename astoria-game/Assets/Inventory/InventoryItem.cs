using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class represents an item. Extend this class whenever you extend ItemData to account for the new dev-time variables.
/// </summary>
[Serializable]
public class InventoryItem
{
	public ItemData ItemData;
	public Dictionary<string, object> ItemRuntimeData { get; private set; }
	
	public InventoryItem(ItemData itemData) {
		ItemData = itemData;
		ItemRuntimeData = new Dictionary<string, object>();
	}
	public InventoryItem(ItemData itemData, Dictionary<string, object> itemRuntimeData) {
		ItemData = itemData;
		ItemRuntimeData = itemRuntimeData;
	}

	public Vector2Int Size {
		get {
			if (ItemData == null) return Vector2Int.zero;
			return Rotated
				? new Vector2Int(ItemData.ItemSize.y, ItemData.ItemSize.x)
				: ItemData.ItemSize;
		}
	}

	public bool Rotated;

	/// <summary>
	/// Gets data from the item's dictionary.
	/// </summary>
	/// <param name="key">The key of the data.</param>
	/// <param name="value">The retrieved data from the item.</param>
	/// <returns>Whether the data exists or not.</returns>
	public bool GetData(string key, out object value) {
		return ItemRuntimeData.TryGetValue(key, out value);
	}

	/// <summary>
	/// Adds data to the item's dictionary.
	/// </summary>
	/// <param name="key">The key to add.</param>
	/// <param name="value">The object to add.</param>
	public void AddData(string key, object value) {
		ItemRuntimeData[key] = value;
	}

	/// <summary>
	/// Removes data from the item's dictionary.
	/// </summary>
	/// <param name="key">The key to index and remove by.</param>
	/// <returns>Whether the data exists and was removed successfully.</returns>
	public bool RemoveData(string key) {
		if (!ItemRuntimeData.ContainsKey(key)) return false;
		ItemRuntimeData.Remove(key);
		return true;
	}
}