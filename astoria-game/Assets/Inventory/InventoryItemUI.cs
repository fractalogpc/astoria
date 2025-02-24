using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[AddComponentMenu("")]
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(CanvasRenderer))]
[RequireComponent(typeof(ClickableEvents))]
public class InventoryItemUI : MonoBehaviour
{
	[Header("Data")]
	public ItemInstance ItemInstance;
	[Header("Ext Refs")]
	[SerializeField] private GameObject _inventoryItemDraggedPrefab;
	[Header("Self Refs")]
	[SerializeField] private Image _itemImage;
	[SerializeField] private TextMeshProUGUI _itemText;
	[ReadOnly][SerializeField] private InventoryComponent _parentInventory;
	
	private RectTransform _rectTransform;
	private ClickableEvents _clickableEvents;
	private Vector2Int _BLContainerIndex;
	private GameObject _draggedInstance;

	/// <summary>
	/// Sets the item to be held. Records the inventory the item belongs to, and the item's position in the InventoryData.
	/// </summary>
	/// <param name="parentInventory">The inventory the InventoryItemUI belongs to.</param>
	/// <param name="itemInstance">The item the InventoryItemUI holds.</param>
	/// <param name="slotIndexBL">The bottom-left index of the slot the item is in.</param>
	/// <param name="slotSize">The size of a slot visually in units.</param>
	public void InitializeWithItem(InventoryComponent parentInventory, ItemInstance itemInstance, Vector2Int slotIndexBL, float slotSize) {
		_rectTransform = GetComponent<RectTransform>();
		_parentInventory = parentInventory;
		ItemInstance = itemInstance;
		_BLContainerIndex = parentInventory.InventoryData.GetSlotIndexOf(itemInstance);
		_rectTransform.anchoredPosition = new Vector2(slotIndexBL.x * slotSize + slotSize * itemInstance.Size.x / 2, slotIndexBL.y * slotSize + slotSize * itemInstance.Size.y / 2);
		_rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, itemInstance.Size.x * slotSize);
		_rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, itemInstance.Size.y * slotSize);
		
		_itemImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, itemInstance.ItemData.ItemSize.x * slotSize);
		_itemImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, itemInstance.ItemData.ItemSize.y * slotSize);
		_itemImage.sprite = itemInstance.ItemData.ItemIcon;
		
		_itemImage.rectTransform.rotation = ItemInstance.Rotated ? Quaternion.Euler(0, 0, 90) : Quaternion.identity;
		
		_itemText.text = itemInstance.ItemData.ItemName;
	}
	
	private void Start() {
		// InitializeWithItem(Item); should be called by instantiating object
		_clickableEvents = GetComponent<ClickableEvents>();
		_clickableEvents.OnClickDownSelected.AddListener(OnClickedOn);
	}

	private void OnDisable() {
		// Getting errors with clickable events being null? This is probably bad
		_clickableEvents?.OnClickDownSelected.RemoveListener(OnClickedOn);
	}
	
	private void OnClickedOn() {
		InstantiateDraggedItem();
		_parentInventory.RemoveItem(ItemInstance);
	}

	private void InstantiateDraggedItem() {
		// Parented to whole canvas
		_draggedInstance = Instantiate(_inventoryItemDraggedPrefab, _rectTransform.GetComponentInParent<Canvas>().transform);
		InventoryItemDraggedUI script = _draggedInstance.GetComponent<InventoryItemDraggedUI>();
		script.InitializeFromInventory(_parentInventory, ItemInstance, _BLContainerIndex, _parentInventory.SlotSizeUnits);
	}
}