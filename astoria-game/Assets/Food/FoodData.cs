using UnityEngine;

[CreateAssetMenu(fileName = "New Food", menuName = "Scriptable Objects/Items/Consumables/FoodData")]
public class FoodData : ConsumableData
{
	public float _foodAmount;
    public float _waterAmount;
	
	public override ItemInstance CreateItem() {
		return new FoodInstance(this);
	}
}