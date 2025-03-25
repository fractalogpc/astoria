using UnityEngine;

[CreateAssetMenu(fileName = "SmeltableData", menuName = "Scriptable Objects/Items/Furnace/SmeltableData")]
public class SmeltableData : ItemData
{
	[Tooltip("The items that will be produced when this item is smelted.")]
	public ItemSetList SmeltResults;
	[Tooltip("The amount of fuel that will be consumed when this item is smelted. Also dictates smelting time.")]
	public float FuelCost;
	
	public override ItemInstance CreateItem() {
		return new SmeltableInstance(this);
	}
}
