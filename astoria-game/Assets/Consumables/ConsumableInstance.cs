using System;
using UnityEngine;

public class ConsumableInstance : ItemInstance
{
	public new ConsumableData ItemData => (ConsumableData)base.ItemData;
	public ConsumableInstance(ItemData itemData) : base(itemData) {
		
	}
	public override void OnHotbarSelected( ) {
		base.OnHotbarSelected();
		Use();
	}
	protected virtual void Use() {
		throw new NotImplementedException();
	}
}
