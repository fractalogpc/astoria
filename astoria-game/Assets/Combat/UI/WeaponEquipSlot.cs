using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ClickableEvents))]
public class WeaponEquipSlot : InventoryEquipableSlot
{
	public override bool TryAddToSlot(ItemInstance itemInstance) {
		if (HeldItem != null) {
			Debug.Log("WeaponEquipSlot: Slot already has an item instance.");
			return false;
		}
		if (itemInstance is not GunInstance) {
			Debug.Log("WeaponEquipSlot: Item instance is not a gun instance.");
			return false;
		}
		GunInstance gunInstance = (GunInstance)itemInstance;
		HeldItem = gunInstance;
		_itemImage.sprite = gunInstance.ItemData.ItemIcon;
		_itemImage.type = Image.Type.Simple;
		_itemImage.preserveAspect = true;
		_itemImage.color = Color.white;
		if (_itemText != null) _itemText.text = gunInstance.ItemData.ItemName;
		OnItemAdded.Invoke(gunInstance);
		return true;
	}

	public override void OnPickup() {
		if (HeldItem == null) return;
		InstantiateDraggedItem(HeldItem);
		_itemImage.sprite = null;
		_itemImage.color = Color.clear;
		if (_itemText != null) _itemText.text = "";
		OnItemRemoved.Invoke(HeldItem);
		HeldItem = null;
	}
}