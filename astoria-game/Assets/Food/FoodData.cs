using UnityEngine;

[CreateAssetMenu(fileName = "New Food", menuName = "Scriptable Objects/Consumables/FoodData")]
public class FoodData : ConsumableData
{
	public float _foodAmount;
    public float _waterAmount;
	
	public override ItemInstance CreateItem() {
		return new FoodInstance(this);
	}
}