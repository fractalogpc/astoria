using UnityEngine;

public class MedsInstance : ConsumableInstance
{
	public new MedsData ItemData => (MedsData)base.ItemData;
	public MedsInstance(MedsData itemData) : base(itemData) {
		
	}

	protected override void Use() {
		HealthInterface healthInterface = PlayerInstance.Instance.GetComponentInChildren<HealthInterface>();
		InventoryHotbar hotbar = PlayerInstance.Instance.GetComponentInChildren<InventoryHotbar>();
		if (healthInterface.CurrentHealth >= ItemData._healthThreshold) {
			Debug.Log("Player is at too high health to use this item.");
			return;
		}
		healthInterface.Heal(ItemData._healAmount);
		hotbar.RemoveItem(this);
	}
}