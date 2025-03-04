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
public class InventoryStackUI : MonoBehaviour
{
	[Header("Data")]
	public ItemStack ItemStack;
	[Header("Ext Refs")]
	[SerializeField] private GameObject _inventoryItemDraggedPrefab;
	[Header("Self Refs")]
	[SerializeField] private Image _itemImage;
	[SerializeField] private TextMeshProUGUI _itemText;
	[SerializeField] private TextMeshProUGUI _itemCountText;
	[ReadOnly][SerializeField] private InventoryComponent _parentInventory;
	
	private RectTransform _rectTransform;
	private ClickableEvents _clickableEvents;
	private Vector2Int _BLContainerIndex;
	private GameObject _draggedInstance;

	/// <summary>
	/// Sets the item to be held. Records the inventory the item belongs to, and the item's position in the InventoryData.
	/// </summary>
	/// <param name="parentInventory">The inventory the InventoryItemUI belongs to.</param>
	/// <param name="itemStack">The item the InventoryItemUI holds.</param>
	/// <param name="slotIndexBL">The bottom-left index of the slot the item is in.</param>
	/// <param name="slotSize">The size of a slot visually in units.</param>
	public void InitializeWithStack(InventoryComponent parentInventory, ItemStack itemStack, Vector2Int slotIndexBL, float slotSize) {
		_rectTransform = GetComponent<RectTransform>();
		_parentInventory = parentInventory;
		ItemStack = itemStack;
		_BLContainerIndex = parentInventory.GetSlotIndexOf(itemStack);
		_rectTransform.anchoredPosition = new Vector2(slotIndexBL.x * slotSize + slotSize * itemStack.Size.x / 2, slotIndexBL.y * slotSize + slotSize * itemStack.Size.y / 2);
		_rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, itemStack.Size.x * slotSize);
		_rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, itemStack.Size.y * slotSize);
		
		_itemImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, itemStack.StackType.ItemSize.x * slotSize);
		_itemImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, itemStack.StackType.ItemSize.y * slotSize);
		_itemImage.sprite = itemStack.StackType.ItemIcon;
		
		_itemImage.rectTransform.rotation = ItemStack.Rotated ? Quaternion.Euler(0, 0, 90) : Quaternion.identity;
		
		_itemText.text = itemStack.StackType.ItemName;
		
		_itemCountText.text = itemStack.StackCount > 1 ? itemStack.StackCount.ToString() : "";
	}
	
	private void Start() {
		// InitializeWithItem(Item); should be called by instantiating object
		_clickableEvents = GetComponent<ClickableEvents>();
		_clickableEvents.OnClickDownSelected.AddListener(OnClickedOnStack);
	}

	private void OnDisable() {
		// Getting errors with clickable events being null? This is probably bad
		_clickableEvents?.OnClickDownSelected.RemoveListener(OnClickedOnStack);
	}
	
	private void OnClickedOnStack() {
		_parentInventory.RemoveStack(ItemStack);
		InstantiateDraggedStack(ItemStack);
	}
	private void OnClickedOnSingle() {
		_parentInventory.PopFrom(ItemStack, out ItemInstance item, out bool left);
		InstantiateDraggedStack(new ItemStack(item));
	}

	private void InstantiateDraggedStack(ItemStack stack) {
		// Parented to whole canvas
		_draggedInstance = Instantiate(_inventoryItemDraggedPrefab, _rectTransform.GetComponentInParent<Canvas>().transform);
		InventoryItemDraggedUI script = _draggedInstance.GetComponent<InventoryItemDraggedUI>();
		script.InitializeFromInventory(_parentInventory, stack, _BLContainerIndex, _parentInventory.SlotSizeUnits);
	}
}