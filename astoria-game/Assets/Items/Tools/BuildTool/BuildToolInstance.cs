using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class BuildToolInstance : BaseToolInstance
{
	public new BuildToolData ItemData => (BuildToolData)base.ItemData;
	private float _timeSinceLastSideChop = float.MaxValue;
	private float _timeSinceLastDownChop = float.MaxValue;	
	public BuildToolInstance(ItemData itemData) : base(itemData) {
	}
	
	public override void OnEquip() {
		base.OnEquip();
		Debug.Log("Build tool equipped");
	}
	public override void OnUnequip() {
		base.OnUnequip();
	}

	public override void OnTick() {
		base.OnTick();
		_timeSinceLastSideChop += Time.deltaTime;
		_timeSinceLastDownChop += Time.deltaTime;
	}
	public override void OnUseDown() {
		Debug.Log("Called input");
		base.OnUseDown();
		
		_viewmodelManager.PlayToolUse();
		Debug.Log("Axe used");
	}
	public override void OnUseUp() {
		base.OnUseUp();
	}
	public override void OnUseHold() {
		base.OnUseHold();
	}
	public override void OnAltUseDown() {
		base.OnAltUseDown();

		_viewmodelManager.PlayUseSecondary();
	}
	public override void OnAltUseUp() {
		base.OnAltUseUp();
	}
	public override void OnAltUseHold() {
		base.OnAltUseHold();
	}
}
