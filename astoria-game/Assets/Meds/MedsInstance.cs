using UnityEngine;

public class MedsInstance : ConsumableInstance
{
	public new MedsData ItemData => (MedsData)base.ItemData;
	public MedsInstance(MedsData itemData) : base(itemData) {
		
	}

	protected override void Use() {
		PlayerInstance.Instance.GetComponentInChildren<HealthInterface>().Heal(ItemData._healAmount);
	}
}