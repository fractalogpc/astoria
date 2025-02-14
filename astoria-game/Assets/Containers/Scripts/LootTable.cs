using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mirror;
using UnityEditor.Rendering;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class LootTable
{
	/// <summary>
	/// Pools first in the list have priority over pools later in the list.
	/// If the container fills up before all pools are drawn from, the remaining pools will not have room to fit their items.
	/// </summary>
	[Tooltip("Pools are drawn sequentially.\nIf the container fills up before all pools are drawn from, the remaining pools will not have room to fit their items.")]
	public List<LootPool> LootPools;
	
	public int MinItems {
		get {
			return LootPools.Sum(pool => pool.MinItems);
		}
	}
	public int MaxItems {
		get {
			return LootPools.Sum(pool => pool.MaxItems);
		}
	}
	
	public void ValidateInput() {
		foreach (LootPool lootPool in LootPools) {
			lootPool.ValidateInput();
		}
	}
	
	public List<ItemData> GenerateLoot() {
		List<ItemData> output = new();
		foreach (LootPool lootPool in LootPools) {
			output.AddRange(lootPool.DrawItems());
		}
		return output;
	}
}

[Serializable]
public class LootPool
{
	/// <summary>
	/// The list of all the items in the pool and their weights.
	/// </summary>
	[Tooltip("The list of all the items in the pool and their draw weights.\nA higher weight relative to the total means a more likely pick.")]
	public List<ItemWeight> ItemWeights;
	/// <summary>
	/// The total weight of all items in the pool.
	/// </summary>
	public int TotalWeight {
		get {
			return ItemWeights.Sum(element => element.Weight);
		}
	}
	/// <summary>
	/// The minimum amount of items that will be drawn from the pool.
	/// Can be 0.
	/// </summary>
	[Tooltip("The minimum amount of items that will be drawn from the pool.\nCan be 0.")]
	public int MinItems;
	/// <summary>
	/// The maximum amount of items that will be drawn from the pool.
	/// </summary>
	[Tooltip("The maximum amount of items that will be drawn from the pool.\nCan be equal to MinItems.")]
	public int MaxItems;
	/// <summary>
	/// True if this pool should only be drawn on the first generation of the loot table.
	/// This can be used for key items or other items that should only be drawn once, regardless of the number of times the loot table is generated.
	/// </summary>
	[Tooltip("True if this pool should only be drawn on the first generation of the loot table.\nThis can be used for key items or other items that should only be drawn once, regardless of the number of times the loot table is generated.")]
	public bool OnlyOnFirstGeneration;
	[SerializeField][ReadOnly] private bool _drawnBefore;

	/// <summary>
	/// Draws items from the pool.
	/// </summary>
	/// <returns>A list of the drawn items.</returns>
	public List<ItemData> DrawItems() {
		List<ItemData> output = new();
		
		if (OnlyOnFirstGeneration && _drawnBefore) {
			return output;
		}
		_drawnBefore = true;
		
		// Create bag
		List<ItemData> itemBag = new();
		foreach (ItemWeight itemWeight in ItemWeights.Where(itemWeight => itemWeight.Weight != 0)) {
			for (int i = 0; i < itemWeight.Weight; i++) {
				itemBag.Add(itemWeight.Item);
			}
		}
		
		// Pick items in bag (doesn't remove item from bag, just adds same item to output)
		int itemsToDraw = Random.Range(MinItems, MaxItems + 1);
		for (int i = 0; i < itemsToDraw; i++) {
			if (itemBag.Count == 0) {
				Debug.LogWarning("Loot pool " + this + " has no items left to draw, but still needs to draw " + (itemsToDraw - i) + " more items.");
				break;
			}
			int index = Random.Range(0, itemBag.Count);
			ItemData item = itemBag[index];
			ItemWeight weight = ItemWeights.Find(weight => weight.Item == item);
			weight.Pick();
			output.Add(item);
			if (!weight.CanPick) {
				itemBag.RemoveAll(bagItems => bagItems == item);
			}
		}
		
		foreach (ItemWeight itemWeight in ItemWeights) {
			itemWeight.ResetPicksRemaining();
		}
		return output;
	}

	public void ValidateInput() {
		if (MinItems < 0) {
			MinItems = 0;
		}
		if (MaxItems < MinItems) {
			MaxItems = MinItems;
		}
		foreach (ItemWeight itemWeight in ItemWeights) {
			itemWeight.ValidateInput();
		}
	}
}

/// <summary>
/// A class containing an ItemData, weight and draw limit for the item. Used in LootPool.
/// </summary>
[Serializable]
public class ItemWeight
{
	/// <summary>
	/// The Item this weight is for.
	/// </summary>
	[Tooltip("The Item this weight is for.")]
	public ItemData Item;
	/// <summary>
	/// The weight of the item being picked.
	/// The higher the weight, compared to the sum of all weights in the pool, the more likely it is to be picked.
	/// Weights less than or equal to 0 are ignored.
	/// </summary>
	[Tooltip("The weight of the item being picked.\nThe higher the weight, compared to the sum of all weights in the pool, the more likely it is to be picked.\nWeights less than or equal to 0 are ignored.")]
	public int Weight;
	/// <summary>
	/// The number of times this item can be picked in a single draw.
	/// A draw limit of 0 is treated as no limit.
	/// Once the limit is reached, the item's weight will be removed, and the total weight of the pool will be recalculated.
	/// </summary>
	[Tooltip("The number of times this item can be picked in a single draw.\nA draw limit of 0 is treated as no limit.\nOnce the limit is reached, the item's weight will be removed, and the total weight of the pool will be recalculated.")]
	public int SingleDrawPickLimit;
	
	public int SingleDrawPicksRemaining { get; private set; }
	public bool CanPick {
		get {
			if (SingleDrawPickLimit == 0) {
				return true;
			}
			return SingleDrawPicksRemaining > 0;
		}	
	}
	
	/// <summary>
	/// Decrements the number of picks remaining for this item, if there is a limit.
	/// </summary>
	public void Pick() {
		if (SingleDrawPicksRemaining > 0) {
			SingleDrawPickLimit--;
		}
	}
	public void ResetPicksRemaining() {
		SingleDrawPicksRemaining = SingleDrawPickLimit;
	}
	
	public void ValidateInput() {
		if (Weight < 0) {
			Weight = 0;
		}
		if (Weight == 0) {
			Debug.LogWarning("Item " + Item + " has a weight of 0. This item will never be picked. Consider removing it from the pool.");
		}
		if (SingleDrawPickLimit < 0) {
			SingleDrawPickLimit = 0;
		}
		SingleDrawPicksRemaining = SingleDrawPickLimit;
	}
}