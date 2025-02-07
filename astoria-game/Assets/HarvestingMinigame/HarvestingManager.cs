using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Random = UnityEngine.Random;

public class HarvestingManager : Singleton<HarvestingManager>
{
	public List<Harvestable> Harvestables = new();
	public HarvesterData CurrentHarvester { get; private set; }
	public bool RegisterHarvestable(Harvestable harvestable) {
		if (Harvestables.Contains(harvestable)) return false;
		Harvestables.Add(harvestable);
		return true;
	}
	public bool DeregisterHarvestable(Harvestable harvestable) {
		return Harvestables.Remove(harvestable);
	}
	public void RegisterHarvester(HarvesterData harvester) {
		CurrentHarvester = harvester;
	}
	public void DeregisterHarvester() {
		CurrentHarvester = null;
	}
	private void OnHarvestableLookedAt(Harvestable harvestable) {
		
	}
	private void OnHarvestableHit(Harvestable harvestable) {
		
	}
	
	

}

