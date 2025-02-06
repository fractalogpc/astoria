using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Random = UnityEngine.Random;

public class HarvestingManager : Singleton<HarvestingManager>
{
	public List<Harvestable> _harvestables = new();
	public bool RegisterHarvestable(Harvestable harvestable) {
		if (_harvestables.Contains(harvestable)) return false;
		_harvestables.Add(harvestable);
		return true;
	}
	public bool DeregisterHarvestable(Harvestable harvestable) {
		return _harvestables.Remove(harvestable);
	}
	
	
}

