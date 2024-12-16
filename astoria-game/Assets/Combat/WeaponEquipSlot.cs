using System;
using Mirror;
using TMPro;
using TMPro.EditorUtilities;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

[RequireComponent(typeof(ClickableEvents))]
public class WeaponEquipSlot : InventoryEquipableSlot
{
	[SerializeField] private CombatInventory _combatInventory;
	[SerializeField] private CombatInventory.WeaponSlot _slotType;

	public override bool TryAddToSlot(ItemInstance itemInstance) {
		if (_heldItemInstance != null) return false;
		if (itemInstance.ItemData is not GunData gunData) return false;
		if (_combatInventory.SlotHasWeapon(_slotType)) return false;
		if (!_combatInventory.AddWeaponInstanceToSlot(itemInstance as GunInstance, _slotType)) Debug.LogError("WeaponEquipSlot: Failed to add weapon instance to slot.");
		_heldItemInstance = itemInstance;
		_itemImage.sprite = itemInstance.ItemData.ItemIcon;
		_itemImage.color = Color.white;
		_itemText.text = itemInstance.ItemData.ItemName;
		OnItemAdded.Invoke(itemInstance);
		return true;
	}

	public override void OnRemove() {
		if (_heldItemInstance == null) return;
		InstantiateDraggedItem(_combatInventory.RemoveWeaponFromSlot(_slotType));
		_itemImage.sprite = null;
		_itemImage.color = Color.clear;
		_itemText.text = "";
		OnItemRemoved.Invoke(_heldItemInstance);
		_heldItemInstance = null;
	}
}