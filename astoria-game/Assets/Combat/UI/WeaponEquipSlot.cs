using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ClickableEvents))]
public class WeaponEquipSlot : InventoryEquipableSlot
{
	public override bool TryAddToSlot(ItemInstance itemInstance) {
		if (_heldItemInstance != null) {
			Debug.Log("WeaponEquipSlot: Slot already has an item instance.");
			return false;
		}
		if (itemInstance.ItemData is not GunData) {
			Debug.Log("WeaponEquipSlot: Item instance is not a gun instance.");
			return false;
		}
		_heldItemInstance = itemInstance;
		_itemImage.sprite = itemInstance.ItemData.ItemIcon;
		_itemImage.type = Image.Type.Simple;
		_itemImage.preserveAspect = true;
		_itemImage.color = Color.white;
		_itemText.text = itemInstance.ItemData.ItemName;
		OnItemAdded.Invoke(itemInstance);
		return true;
	}

	public override void OnRemove() {
		if (_heldItemInstance == null) return;
		InstantiateDraggedItem(_heldItemInstance);
		_itemImage.sprite = null;
		_itemImage.color = Color.clear;
		_itemText.text = "";
		OnItemRemoved.Invoke(_heldItemInstance);
		_heldItemInstance = null;
	}
}