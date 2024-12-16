using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class represents an item. Inherit this class whenever you extend ItemData to account for the new runtime variables.
/// </summary>
[Serializable]
public class ItemInstance
{
	public ItemData ItemData;
	public bool Rotated;
	
	public ItemInstance(ItemData itemData) {
		ItemData = itemData;
	}

	public Vector2Int Size {
		get {
			if (ItemData == null) return Vector2Int.zero;
			return Rotated
				? new Vector2Int(ItemData.ItemSize.y, ItemData.ItemSize.x)
				: ItemData.ItemSize;
		}
	}
}