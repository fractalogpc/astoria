using System.Collections;
using UnityEngine;

public class MultitoolInstance : BaseToolInstance
{
	public new MultitoolData ItemData => (MultitoolData)base.ItemData;
	public MultitoolInstance(ItemData itemData) : base(itemData)
	{
	}

	public override void OnEquip()
	{
		base.OnEquip();
	}
	public override void OnUnequip()
	{
		PlayerInstance.Instance.GetComponentInChildren<TogglePlayerBuildingUI>().SetVisibility(false);

		base.OnUnequip();
	}
	public override void OnTick()
	{
		base.OnTick();
	}
	public override void OnUseDown()
	{
		base.OnUseDown();
	}
	public override void OnUseUp()
	{
		base.OnUseUp();
	}
	public override void OnUseHold()
	{
		base.OnUseHold();
	}
	public override void OnAltUseDown()
	{
		PlayerInstance.Instance.GetComponentInChildren<TogglePlayerBuildingUI>().SetVisibility(true);

		base.OnAltUseDown();
	}
	public override void OnAltUseUp()
	{
		PlayerInstance.Instance.GetComponentInChildren<TogglePlayerBuildingUI>().SetVisibility(false);

		base.OnAltUseUp();
	}
	public override void OnAltUseHold()
	{
		base.OnAltUseHold();
	}
}
