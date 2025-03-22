using UnityEngine;

public class ConsumableData : ItemData
{
	// TODO: Make this work
	public float _useDelay;
	public override ItemInstance CreateItem() {
		return new ConsumableInstance(this);
	}
}
