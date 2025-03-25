using UnityEngine;

public class HarvestableRock : HealthManager
{
	[SerializeField] private ItemData _stoneItem;
	[SerializeField] private float _damagePerDrop = 10f;
	
	private float _damageAccountedFor = 0f;
	
	public override void Damage(float damagePoints, Vector3 hitPosition) {
		base.Damage(damagePoints, hitPosition);
		
		float damageUnaccountedFor = MaxHealth - CurrentHealth - _damageAccountedFor;
		for (int i = 0; i < damageUnaccountedFor / _damagePerDrop; i++) {
			PlayerInstance.Instance.GetComponentInChildren<InventoryComponent>().AddItemByData(_stoneItem);
			_damageAccountedFor += _damagePerDrop;
		}

		if (IsDead) {
			// Drop resources
			// PlayerInstance.Instance.GetComponentInChildren<InventoryComponent>().AddItemByData(_stoneItem);
			// Destroy game object
			Destroy(gameObject);
		}
	}
}
