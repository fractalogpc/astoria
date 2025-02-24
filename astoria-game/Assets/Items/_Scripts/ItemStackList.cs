using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A class containing a list of ItemStacks.
/// </summary>
[Serializable]
public class ItemStackList
{
	[SerializeField] private List<ItemStack> list;

	public int Count => list.Count;

	/// <summary>
	/// Constructs an empty ItemStackList.
	/// </summary>
	public ItemStackList() {}
	
	/// <summary>
	/// Constructs an ItemStackList from a list of ItemInstances.
	/// Will populate ItemSets with a new ItemSet for each unique ItemData in items, with the count of each item in the list.
	/// </summary>
	/// <param name="items">The ItemInstances to create the ItemStackList with.</param>
	public ItemStackList(List<ItemInstance> items) {
		List<ItemStack> newStacks = new();
		foreach (ItemInstance item in items) {
			bool found = false;
			foreach (ItemStack stack in newStacks) {
				if (stack.Push(item)) found = true;
				break;
			}
			if (!found) newStacks.Add(new ItemStack(item.ItemData));
		}
		list = newStacks;
	}

	/// <summary>
	/// Creates a list of ItemInstances from the sets in this ItemStackList.
	/// Each set will be converted to a number of ItemInstances equal to the type and count in ItemSets.
	/// </summary>
	/// <returns>The list of ItemInstances converted from this ItemStackList.</returns>
	public List<ItemInstance> ToItemsList() {
		List<ItemInstance> items = new();
		foreach (ItemStack stack in list) {
			items.AddRange(stack.Items);
		}
		return items;
	}

	/// <summary>
	/// Grabs all the ItemDatas, ignoring count, from the sets in this ItemStackList.
	/// Use this for finding what items are found at least once in ItemSets.
	/// </summary>
	/// <returns>The list of the ItemDatas contained in ItemSets</returns>
	public List<ItemData> ToDatasList() {
		return list.Select(stack => stack.StackType).ToList();
	}
	
	public int IndexOf(ItemStack stack) {
		return list.IndexOf(stack);
	}
	
	public ItemStack StackContaining(ItemInstance item) {
		return list.FirstOrDefault(stack => stack.Contains(item));
	}
	
	public bool Contains(ItemStack other) {
		return list.Any(stack => stack == other);
	}
	
	public bool Add(ItemInstance item) {
		foreach (ItemStack stack in list) {
			if (stack.Push(item)) return true;
		}
		list.Add(new ItemStack(item.ItemData));
		return true;
	}
}