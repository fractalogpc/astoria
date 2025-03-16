using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class BuildToolInstance : BaseToolInstance
{
	public new BuildToolData ItemData => (BuildToolData)base.ItemData;
	public BuildToolInstance(ItemData itemData) : base(itemData) {
	}
	
	public override void OnEquip() {
		base.OnEquip();
	}
	public override void OnUnequip() {
		base.OnUnequip();
	}

	public override void OnTick() {
		base.OnTick();
	}
	public override void OnUseDown() {
		base.OnUseDown();
		_viewmodelManager.SetTrigger("Use");
	}
	public override void OnUseUp() {
		base.OnUseUp();
	}
	public override void OnUseHold() {
		base.OnUseHold();
	}
	public override void OnAltUseDown() {
		base.OnAltUseDown();
	}
	public override void OnAltUseUp() {
		base.OnAltUseUp();
	}
	public override void OnAltUseHold() {
		base.OnAltUseHold();
	}
}
