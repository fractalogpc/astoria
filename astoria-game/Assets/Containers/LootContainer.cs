using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
[Serializable]
public class LootTable
{
	public List<LootTableElement> Elements;
}
[Serializable]
public class LootTableElement
{
	public List<ItemData> Items;
	public int ChanceFactor;
}

public class LootContainer : Interactable
{
	
	[Header("Set Inventory Values on container's inventory")]
	[SerializeField] private InventoryComponent _containerInventory;
	[SerializeField] private InventoryComponent _playerInvDisplay;
	
	private InventoryComponent _playerInventory;
	
	public override void Interact() {
		base.Interact();
		
	}
}
