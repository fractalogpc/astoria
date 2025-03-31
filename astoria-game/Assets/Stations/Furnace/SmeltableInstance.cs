using System.Collections.Generic;
using UnityEngine;

public class SmeltableInstance : ItemInstance
{
	public SmeltableData ItemData => (SmeltableData)base.ItemData;
	/// <summary>
	/// The amount of fuel contributed to smelting this item.
	/// </summary>
	public float FuelUsed { get; private set; }
	public bool IsSmelted => FuelUsed >= ItemData.FuelCost;
	public SmeltableInstance(ItemData itemData) : base(itemData) {
	}
	
	public void AddFuel(float fuel) {
		FuelUsed += fuel;
	}

	public List<ItemData> GetResult() {
		return ItemData.SmeltResults.ToDatasList();
	}
}
