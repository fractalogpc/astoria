using Mirror;
using UnityEngine;

public class HarvestableRock : HealthInterface
{
	[SerializeField] private ItemData _stoneItem;
	public override void Damage(float damagePoints, Vector3 hitPosition) {
		base.Damage(damagePoints, hitPosition);
		if (IsDead) {
			// Drop resources
			NetworkClient.localPlayer.GetComponentInChildren<InventoryComponent>().AddItemByData(_stoneItem);
			// Destroy game object
			Destroy(gameObject);
		}
	}
}
