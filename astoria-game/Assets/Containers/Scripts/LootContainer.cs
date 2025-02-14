using System;
using System.Collections.Generic;
using UnityEngine;

public class LootContainer : ContainerManager
{
	
	/// <summary>
	/// A set of loot pools drawn from sequentially.
	/// </summary>
	[Tooltip("A set of loot pools drawn from sequentially, which create the loot in the container.")]
	public LootTable LootTable;

	public void GenerateLoot() {
		List<ItemData> loot = LootTable.GenerateLoot();
		foreach (ItemData item in loot) {
			if (_containerInventory.AddItemByData(item)) {
				Debug.Log("Added " + item.ItemName + " to container.");
			}
			else {
				Debug.Log("Failed to add " + item.ItemName + " to container. Either the container is full or the item is not stackable.");
			}
		}
	}

	private void OnValidate() {
		LootTable.ValidateInput();
	}

	protected override void Start() {
		base.Start();
		LootTable.ValidateInput();
		GenerateLoot();
	}
}
