using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

[Serializable]
public class LootTable
{
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
	
}

[Serializable]
public class LootPool
{
	public List<ItemWeight> LootItems;
	public int TotalWeight {
		get {
			return LootItems.Sum(element => element.SelectionWeight);
		}
	}
	public int MinItems;
	public int MaxItems;
	public bool DrawOnlyOnce;
	[SerializeField][ReadOnly] private bool _drawnBefore;
	
}

[Serializable]
public class ItemWeight
{
	public ItemData Item;
	public int SelectionWeight;
}