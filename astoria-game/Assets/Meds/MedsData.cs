using UnityEngine;

[CreateAssetMenu(fileName = "New Meds", menuName = "Scriptable Objects/Items/Consumables/MedsData")]
public class MedsData : ConsumableData
{
	[Tooltip("The amount of health that the player will be healed by.")]
	public float _healAmount;
	[Tooltip("The maximum health percentage that the player can be at to use this item.")]
	public float _healthThreshold;
	
	public override ItemInstance CreateItem() {
		return new MedsInstance(this);
	}
}