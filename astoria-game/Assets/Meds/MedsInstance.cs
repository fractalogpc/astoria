using UnityEngine;

public class MedsInstance : ConsumableInstance
{
	public new MedsData ItemData => (MedsData)base.ItemData;
	public MedsInstance(MedsData itemData) : base(itemData) {
		
	}

	protected override void Use() {
		HealthManager healthManager = PlayerInstance.Instance.GetComponentInChildren<HealthManager>();
		InventoryHotbar hotbar = PlayerInstance.Instance.GetComponentInChildren<InventoryHotbar>();
		if (healthManager.CurrentHealth >= ItemData._healthThreshold) {
			Debug.Log("Player is at too high health to use this item.");
			return;
		}
		healthManager.Heal(ItemData._healAmount);
		hotbar.RemoveItem(this);
	}
}