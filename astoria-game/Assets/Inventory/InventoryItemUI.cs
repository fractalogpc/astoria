using System.Collections.Generic;
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
	[SerializeField] private InventoryComponent _parentInventory;
	
	private RectTransform _rectTransform;
	private ClickableEvents _clickableEvents;
	private Vector2Int _BLContainerIndex;
	private GameObject _draggedInstance;

	/// <summary>
	/// Sets the item to be held. Records the inventory the item belongs to, and the item's position in the InventoryData.
	/// </summary>
	/// <param name="itemInstance">The item the InventoryItemUI holds.</param>
	/// <param name="parentInventory">The inventory the InventoryItemUI belongs to.</param>
	public void InitializeWithItem(ItemInstance itemInstance, InventoryComponent parentInventory) {
		_parentInventory = parentInventory;
		ItemInstance = itemInstance;
		_BLContainerIndex = parentInventory.InventoryData.GetSlotIndexOf(itemInstance);
		_itemImage.sprite = itemInstance.ItemData.ItemIcon;
		_itemText.text = itemInstance.ItemData.ItemName;
	}
	
	private void Start() {
		// InitializeWithItem(Item); should be called by instantiating object
		_rectTransform = GetComponent<RectTransform>();
		_clickableEvents = GetComponent<ClickableEvents>();
		_clickableEvents.OnClickDownSelected.AddListener(OnClickedOn);
	}

	private void OnDisable() {
		_clickableEvents.OnClickDownSelected.RemoveListener(OnClickedOn);
	}
	
	private void OnClickedOn() {
		InstantiateDraggedItem();
		_parentInventory.RemoveItem(ItemInstance);
	}

	private void InstantiateDraggedItem() {
		// Parented to whole canvas
		_draggedInstance = Instantiate(_inventoryItemDraggedPrefab, _rectTransform.GetComponentInParent<Canvas>().transform);
		InventoryItemDraggedUI script = _draggedInstance.GetComponent<InventoryItemDraggedUI>();
		script.InitializeFromInventory(_parentInventory, ItemInstance, _BLContainerIndex);
	}
}