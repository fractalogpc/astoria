using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A class containing a list of ItemSets.
/// Can be used anywhere a list of items defined by type and count is useful. (e.g. crafting costs, loot tables).
/// || Properties: ItemSets, Count | Methods: ContainedWithin, IsEqualTo, ToItemsList, ToDatasList ||
/// </summary>
[Serializable]
public class ItemSetList
{
	[Tooltip("The ItemSets in this ItemSetList. Each set contains an ItemData and a count of that item.")]	
	public List<ItemSet> ItemSets;

	public int Count => ItemSets.Count;

	/// <summary>
	/// Returns true if input at least contains the ItemSet.Count of items matching ItemSet.Item. The ItemSet.ItemCount is scaled by multiplier.
	/// </summary>
	/// <param name="input">A list of ItemInstances to check containment for.</param>
	/// <param name="multiplier">An integer the count of each ItemSet is multiplied by before checking.</param>
	/// <returns>Whether input at least contains the ItemSet.ItemCount of items matching ItemSet.Item.</returns>
	public bool ContainedWithin(List<ItemInstance> input, int multiplier = 1) {
		foreach (ItemSet itemSet in ItemSets) {
			int itemTypeCount = itemSet.ItemCount * multiplier;

			// Scan through input to find the items
			foreach (ItemInstance inputItem in input.Where(inputItem => inputItem.ItemData == itemSet.ItemData)) {
				itemTypeCount--;
				if (itemTypeCount == 0) break;
			}

			if (itemTypeCount > 0) {
				return false;
			}
		}

		return true;
	}

	/// <summary>
	/// Tests if another ItemSetList is equal to this one.
	/// Requires the other sets are in the same order, and have the same ItemData and count.
	/// </summary>
	/// <param name="otherList">The ItemSetList to compare to.</param>
	/// <returns>Whether the other list has the same sets in the same order as this one.</returns>
	public bool IsEqualTo(ItemSetList otherList) {
		if (ItemSets.Count != otherList.ItemSets.Count) return false;
		for (int i = 0; i < ItemSets.Count; i++) {
			if (!ItemSets[i].Equals(otherList.ItemSets[i])) return false;
		}
		return true;
	}

	/// <summary>
	/// Constructs an ItemSetList from a list of ItemInstances.
	/// Will populate ItemSets with a new ItemSet for each unique ItemData in items, with the count of each item in the list.
	/// </summary>
	/// <param name="items">The ItemInstances to create the ItemSetList with.</param>
	public ItemSetList(List<ItemInstance> items) {
		List<ItemSet> newSets = new();
		foreach (ItemInstance item in items) {
			bool found = false;
			foreach (ItemSet set in newSets.Where(set => item.ItemData == set.ItemData)) {
				set.ItemCount += 1;
				found = true;
				break;
			}

			if (!found) newSets.Add(new ItemSet { ItemData = item.ItemData, ItemCount = 1 });
		}

		ItemSets = newSets;
	}

	/// <summary>
	/// Creates a list of ItemInstances from the sets in this ItemSetList.
	/// Each set will be converted to a number of ItemInstances equal to the type and count in ItemSets.
	/// </summary>
	/// <returns>The list of ItemInstances converted from this ItemSetList.</returns>
	public List<ItemInstance> ToItemsList() {
		List<ItemInstance> items = new();
		foreach (ItemSet set in ItemSets) {
			for (int i = 0; i < set.ItemCount; i++) {
				items.Add(new ItemInstance(set.ItemData));
			}
		}

		return items;
	}

	/// <summary>
	/// Grabs all the ItemDatas, ignoring count, from the sets in this ItemSetList.
	/// Use this for finding what items are found at least once in ItemSets.
	/// </summary>
	/// <returns>The list of the ItemDatas contained in ItemSets</returns>
	public List<ItemData> ToDatasList() {
		return ItemSets.Select(set => set.ItemData).ToList();
	}
}