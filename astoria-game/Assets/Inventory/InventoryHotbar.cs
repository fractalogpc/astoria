using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

public class InventoryHotbar : InputHandlerBase
{
	[SerializeField] [ReadOnly] private GameObject _hotbarSlotPrefab;
	[SerializeField] private RectTransform _slotParent;
	[Range(3, 10)][SerializeField] private int _slotCount = 10;
	[SerializeField] private WeaponEquipSlot _primarySlot;
	[SerializeField] private WeaponEquipSlot _secondarySlot;
	[SerializeField] private WeaponEquipSlot _specialSlot;
	[SerializeField] private InventoryEquipableSlot _slotFour;
	[SerializeField] private InventoryEquipableSlot _slotFive;
	[SerializeField] private InventoryEquipableSlot _slotSix;
	[SerializeField] private InventoryEquipableSlot _slotSeven;
	[SerializeField] private InventoryEquipableSlot _slotEight;
	[SerializeField] private InventoryEquipableSlot _slotNine;
	[SerializeField] private InventoryEquipableSlot _slotTen;
	
	private List<GameObject> _hotbarSlots = new();
	
	private void Start() {
		for (int i = _slotParent.childCount - 1; i >= 0; i--) {
			Destroy(_slotParent.GetChild(i).gameObject);
		}
		_hotbarSlots.Clear();
		
		for (int i = 0; i < _slotCount; i++) {
			GameObject slot = Instantiate(_hotbarSlotPrefab, _slotParent);
			InventoryHotbarSlot hotbarSlot = slot.GetComponent<InventoryHotbarSlot>();
			if (!hotbarSlot.Initialize(this)) continue;
			_hotbarSlots.Add(slot);
		}
		
		_hotbarSlots[0].GetComponent<InventoryHotbarSlot>().AttachSlot(_primarySlot);
		_hotbarSlots[1].GetComponent<InventoryHotbarSlot>().AttachSlot(_secondarySlot);
		_hotbarSlots[2].GetComponent<InventoryHotbarSlot>().AttachSlot(_specialSlot);
		for (int i = 3; i < _slotCount; i++) {
			_hotbarSlots[i].GetComponent<InventoryHotbarSlot>().AttachSlot(i switch {
				3 => _slotFour,
				4 => _slotFive,
				5 => _slotSix,
				6 => _slotSeven,
				7 => _slotEight,
				8 => _slotNine,
				9 => _slotTen,
				_ => throw new ArgumentOutOfRangeException()
			});
		}
	}

	protected override void InitializeActionMap() {
		RegisterAction(_inputActions.Player.EquipPrimary, ctx => SelectSlot(0));
		RegisterAction(_inputActions.Player.EquipSecondary, ctx => SelectSlot(1));
		RegisterAction(_inputActions.Player.EquipSpecial, ctx => SelectSlot(2));
		RegisterAction(_inputActions.Player.EquipSlotFour, ctx => SelectSlot(3));
		RegisterAction(_inputActions.Player.EquipSlotFive, ctx => SelectSlot(4));
		RegisterAction(_inputActions.Player.EquipSlotSix, ctx => SelectSlot(5));
		RegisterAction(_inputActions.Player.EquipSlotSeven, ctx => SelectSlot(6));
		RegisterAction(_inputActions.Player.EquipSlotEight, ctx => SelectSlot(7));
		RegisterAction(_inputActions.Player.EquipSlotNine, ctx => SelectSlot(8));
		RegisterAction(_inputActions.Player.EquipSlotTen, ctx => SelectSlot(9));
	}

	private void SelectSlot(int slotIndex) {
		if (slotIndex < 0 || slotIndex >= _hotbarSlots.Count) return;
		foreach (GameObject slot in _hotbarSlots.Where(slot => slot.GetComponent<InventoryHotbarSlot>().Selected)) {
			if (_hotbarSlots.IndexOf(slot) == slotIndex) return;
			slot.GetComponent<InventoryHotbarSlot>().Deselect();
		}
		_hotbarSlots[slotIndex].GetComponent<InventoryHotbarSlot>().Select();
	}
}