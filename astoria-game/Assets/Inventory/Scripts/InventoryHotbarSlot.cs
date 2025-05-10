using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHotbarSlot : MonoBehaviour
{
	public InventoryEquipableSlot AttachedSlot => _attachedSlot;
	public bool Selected { get; private set; }
	[SerializeField] [ReadOnly] private InventoryHotbar _hotbar;
	[SerializeField] private Image _slotBGImage;
	[SerializeField] private Image _itemImage;
	[SerializeField] private Sprite _unselectedBG;
	[SerializeField] private Sprite _selectedBG;

	private InventoryEquipableSlot _attachedSlot;

	public bool Initialize(InventoryHotbar hotbar) {
		if (_hotbar != null) return false;
		_hotbar = hotbar;
		_slotBGImage.sprite = _unselectedBG;
		return true;
	}

	public void Select() {
		Selected = true;
		_slotBGImage.sprite = _selectedBG;
		if (_attachedSlot.HeldItem == null) return;
		_attachedSlot.HeldItem.OnHotbarSelected();
	}

	public void Deselect() {
		Selected = false;
		_slotBGImage.sprite = _unselectedBG;
		if (_attachedSlot.HeldItem == null) return;
		_attachedSlot.HeldItem.OnHotbarDeselected();
	}

	public void AttachSlot(InventoryEquipableSlot slot) {
		if (_attachedSlot != null) {
			_attachedSlot.OnItemAdded.RemoveListener(UpdateSlotState);
			_attachedSlot.OnItemRemoved.RemoveListener(RemoveIcon);
		}

		_attachedSlot = slot;
		_attachedSlot.OnItemAdded.AddListener(UpdateSlotState);
		_attachedSlot.OnItemRemoved.AddListener(RemoveIcon);
		UpdateSlotState(_attachedSlot.HeldItem);
	}

	private void RemoveIcon(ItemInstance item = null) {
		_itemImage.sprite = null;
		_itemImage.color = Color.clear;
		Deselect();
	}

	private void UpdateSlotState(ItemInstance item) {
		if (_attachedSlot.HeldItem == null)
			RemoveIcon();
		else
			ShowIcon();
	}

	private void ShowIcon() {
		_itemImage.color = Color.white;
		_itemImage.sprite = _attachedSlot.HeldItem.ItemData.ItemIcon;
		_itemImage.preserveAspect = true;
	}
}