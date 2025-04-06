using UnityEngine;

public class FoodInstance : ConsumableInstance
{
	public new FoodData ItemData => (FoodData)base.ItemData;
	public FoodInstance(FoodData itemData) : base(itemData) {
		
	}

	protected override void Use() {
		PlayerInstance.Instance.GetComponentInChildren<PlayerHunger>().AddFood(ItemData._foodAmount);
		PlayerInstance.Instance.GetComponentInChildren<PlayerThirst>().AddWater(ItemData._waterAmount);
		InventoryHotbar hotbar = PlayerInstance.Instance.GetComponentInChildren<InventoryHotbar>();
		hotbar.RemoveItem(this);
	}
}