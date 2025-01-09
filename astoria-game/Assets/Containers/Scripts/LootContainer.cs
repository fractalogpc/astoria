using System;
using System.Collections.Generic;
using UnityEngine;



public class LootContainer : ContainerManager
{
	[Header("Loot Table")]
	public LootTable LootTable;
	
	public override void Interact() {
		base.Interact();
	}
}
