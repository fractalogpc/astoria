using UnityEngine;

public class FuelInstance : ItemInstance
{
	public FuelData ItemData => (FuelData)base.ItemData;

	public FuelInstance(ItemData itemData) : base(itemData) {
	}
	
}
