using Mirror.BouncyCastle.Ocsp;
using UnityEngine;
using UnityEngine.UI;

public class KeyItemSlot : InventoryEquipableSlot
{
	public bool RequirementMet { get; private set; }
	[SerializeField] private ItemData _keyItem;
	[SerializeField] private Image _slotBG;
	[SerializeField] private Sprite _missingBG;
	[SerializeField] private Sprite _fulfilledBG;
	[SerializeField][ColorUsage(true, false)] private Color _missingColor;


	public override bool TryAddToSlot(ItemInstance itemInstance) {
		if (HeldItem != null) {
			return false;
		}
		if (itemInstance.ItemData != _keyItem) return false;
		HeldItem = itemInstance;
		_itemImage.sprite = itemInstance.ItemData.ItemIcon;
		_itemImage.type = Image.Type.Simple;
		_itemImage.preserveAspect = true;
		_itemImage.color = Color.white;
		if (_itemText != null) _itemText.text = itemInstance.ItemData.ItemName;
		OnItemAdded.Invoke(itemInstance);
		_slotBG.sprite = _fulfilledBG;
		RequirementMet = true;
		return true;
	}

	public override void RemoveItem() {
		base.RemoveItem();
		_itemImage.color = _missingColor;
		_itemImage.sprite = _keyItem.ItemIcon;
		_slotBG.sprite = _missingBG;
		RequirementMet = false;
	}
	
	protected override void Start() {
		base.Start();
		_itemImage.sprite = _keyItem.ItemIcon;
		_itemImage.color = _missingColor;
		_slotBG.sprite = _missingBG;
		RequirementMet = false;
	}
}
