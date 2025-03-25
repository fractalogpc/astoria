using UnityEngine;

[CreateAssetMenu(fileName = "FuelData", menuName = "Scriptable Objects/Items/Furnace/FuelData")]
public class FuelData : ItemData
{
	[Tooltip("The amount of fuel this item provides.")]
	public float FuelValue;
	
	public override ItemInstance CreateItem() {
		return new FuelInstance(this);
	}
}
