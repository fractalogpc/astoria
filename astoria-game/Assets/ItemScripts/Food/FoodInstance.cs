using UnityEngine;

public class FoodInstance : ConsumableInstance
{
	public new FoodData ItemData => (FoodData)base.ItemData;
	public FoodInstance(FoodData itemData) : base(itemData) {
		
	}

	protected override void Use() {
		PlayerInstance.Instance.GetComponentInChildren<PlayerNeeds>().AddHunger(ItemData._foodAmount);
        PlayerInstance.Instance.GetComponentInChildren<PlayerNeeds>().AddThirst(ItemData._waterAmount);
	}
}