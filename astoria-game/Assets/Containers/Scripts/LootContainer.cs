using System;
using System.Collections.Generic;
using UnityEngine;



public class LootContainer : ContainerManager
{
	[Header("Loot Table")]
	public LootTable LootTable;

	protected override void Interact() {
		base.Interact();
		// Add items to container from loot table
	}
	
}
