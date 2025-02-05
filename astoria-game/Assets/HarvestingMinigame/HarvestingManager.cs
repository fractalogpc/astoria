using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class HarvestingManager : NetworkBehaviour
{
	public Dictionary<Harvestable, HarvestBar> HarvestBars {get; private set;} = new();
	[SerializeField] private HarvestingUI _harvestUI;

	public void FocusOnHarvestable(Harvestable harvestable, float critWidthPercent) {
		HarvestBar bar;
		if (HarvestBars.ContainsKey(harvestable)) {
			bar = HarvestBars[harvestable];
		}
		else {
			bar = MakeHarvestBarFrom(harvestable, critWidthPercent);
			HarvestBars.Add(harvestable, bar);
		}
		
		_harvestUI.SetDisplayedBarTo(bar);
		_harvestUI.ShowBar();
	}
	public void HideHarvestingBar() {
		_harvestUI.HideBar();
	}

	public void Harvest(Harvestable harvestable) {
		
	}
	
	private HarvestBar MakeHarvestBarFrom(Harvestable harvestable, float critWidthPercent) {
		return new HarvestBar(harvestable, critWidthPercent);
	}
	
}

public class HarvestBar
{
	public float CurrentValue;
	public float MaxValue;
	public float CritPosition {get; private set;}
	public float CritWidth {get; private set;}

	public enum HarvestBarState
	{
		Progress,
		Minigame,
		Hidden,
	}
	
	public HarvestBarState State {get; private set;}
	
	public HarvestBar(Harvestable harvestable, float critWidthPercent)
	{
		CurrentValue = harvestable.CurrentHealth;
		MaxValue = harvestable.MaxHealth;
		CritWidth = critWidthPercent;
		float rangeHigh = 100 - (critWidthPercent / 2);
		float rangeLow = critWidthPercent / 2;
		CritPosition = Random.Range(rangeLow, rangeHigh);
		if (harvestable.CurrentHealth > 0) {
			State = HarvestBarState.Progress;
		}
		else if (harvestable.FinalSwingAvailable) {
			State = HarvestBarState.Minigame;
		}
		else {
			State = HarvestBarState.Hidden;
		}
	}
}