using UnityEngine;

public class AxeInstance : BaseToolInstance
{
	public AxeInstance(ItemData itemData) : base(itemData) {
	}

	public override void Initialize(ToolCore toolCore, ViewmodelManager viewmodelManager) {
		base.Initialize(toolCore, viewmodelManager);
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
