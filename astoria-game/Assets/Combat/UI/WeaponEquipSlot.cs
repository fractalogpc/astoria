using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ClickableEvents))]
public class WeaponEquipSlot : InventoryEquipableSlot
{
	[SerializeField] private CombatCore _combatCore;
	
	public override bool TryAddToSlot(ItemInstance itemInstance) {
		if (_heldItemInstance != null) {
			Debug.Log("WeaponEquipSlot: Slot already has an item instance.");
			return false;
		}
		if (itemInstance is not GunInstance) {
			Debug.Log("WeaponEquipSlot: Item instance is not a gun instance.");
			return false;
		}
		GunInstance gunInstance = (GunInstance)itemInstance;
		_heldItemInstance = gunInstance;
		_itemImage.sprite = gunInstance.ItemData.ItemIcon;
		_itemImage.type = Image.Type.Simple;
		_itemImage.preserveAspect = true;
		_itemImage.color = Color.white;
		_itemText.text = gunInstance.ItemData.ItemName;
		_combatCore.InitializeWeapon(gunInstance);
		OnItemAdded.Invoke(gunInstance);
		return true;
	}

	public override void OnPickup() {
		if (_heldItemInstance == null) return;
		InstantiateDraggedItem(_heldItemInstance);
		_itemImage.sprite = null;
		_itemImage.color = Color.clear;
		_itemText.text = "";
		OnItemRemoved.Invoke(_heldItemInstance);
		_heldItemInstance = null;
	}
}