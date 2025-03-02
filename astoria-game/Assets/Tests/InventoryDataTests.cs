using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

public class InventoryDataTests
{
	
	
	
	
	
	
	
	
	
	
	
	
	private ItemStack CreateStackWith(ItemData stackType, int count) {
		ItemStack stack = new(stackType);
		for (int i = 0; i < count; i++) {
			stack.Push(new ItemInstance(stackType));
		}
		return stack;
	}

	private ItemData GetTestItemData(int stackLimit = 10, bool stackable = true) {
		ItemData itemData = ScriptableObject.CreateInstance<ItemData>();
		itemData.ItemName = "Test Item";
		itemData.MaxStackSize = stackLimit;
		itemData.IsStackable = stackable;
		itemData.ItemSize = new Vector2Int(1, 1);
		return itemData;
	}

	private ItemInstance GetTestItemInstance(ItemData itemData) {
		return new ItemInstance(itemData);
	}

	private List<ItemInstance> GetTestItemInstances(ItemData itemData, int count) {
		List<ItemInstance> items = new();
		for (int i = 0; i < count; i++) {
			items.Add(GetTestItemInstance(itemData));
		}
		return items;
	}
}