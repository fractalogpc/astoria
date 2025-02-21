using UnityEngine;

[CreateAssetMenu(fileName = "New Meds", menuName = "Scriptable Objects/Consumables/MedsData")]
public class MedsData : ConsumableData
{
	public float _healAmount;
	
	public override ItemInstance CreateItem() {
		return new MedsInstance(this);
	}
}