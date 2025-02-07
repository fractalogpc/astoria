

public class HarvesterInstance : BaseToolInstance
{
	public new HarvesterData ItemData => (HarvesterData)base.ItemData;
	
	public HarvesterInstance(ItemData itemData) : base(itemData) {
	}
	
	public override void OnEquip() {
		base.OnEquip();
		_toolCore._harvestingManager.RegisterHarvester(this.ItemData);
	}
	public override void OnUnequip() {
		base.OnUnequip();
		_toolCore._harvestingManager.DeregisterHarvester();
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