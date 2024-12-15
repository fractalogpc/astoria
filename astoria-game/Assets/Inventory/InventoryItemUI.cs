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
	public ItemInstance _itemInstance;

	[Header("Ext Refs")]
	[SerializeField] private GameObject _inventoryItemDraggedPrefab;

	private GameObject _draggedInstance;

	[Header("Self Refs")]
	[SerializeField] private Image _itemImage;

	[SerializeField] private TextMeshProUGUI _itemText;
	private RectTransform _rectTransform;
	private GraphicRaycaster _canvasGraphicRaycaster;
	private CanvasGroup _canvasGroup;
	private ClickableEvents _clickableEvents;

	[SerializeField] private InventoryComponent _parentInventory;

	private PointerEventData _pointerEventData = new(EventSystem.current);
	private List<RaycastResult> _raycastResults;
	private Vector2Int _BLContainerIndex;

	[Header("Events")]
	public UnityEvent<GameObject> OnDestroyItem;

	private void Start() {
		// InitializeWithItem(Item); should be called by instantiating object
		_rectTransform = GetComponent<RectTransform>();
		_canvasGroup = GetComponent<CanvasGroup>();
		_canvasGraphicRaycaster = _rectTransform.GetComponentInParent<GraphicRaycaster>();
		_clickableEvents = GetComponent<ClickableEvents>();
		_clickableEvents.OnClickDownSelected.AddListener(OnClickedOn);
		_clickableEvents.OnClickUpAnywhere.AddListener(OnClickUpAnywhere);
		_raycastResults = new List<RaycastResult>();
	}

	private void OnDisable() {
		_clickableEvents.OnClickDownSelected.RemoveListener(OnClickedOn);
		_clickableEvents.OnClickUpAnywhere.RemoveListener(OnClickUpAnywhere);
	}

	public void InitializeWithItem(ItemInstance itemInstance, InventoryComponent parentInventory) {
		_parentInventory = parentInventory;
		_itemInstance = itemInstance;
		_BLContainerIndex = parentInventory.InventoryData.GetSlotIndexOf(itemInstance);
		_itemImage.sprite = itemInstance.ItemData.ItemIcon;
		_itemText.text = itemInstance.ItemData.ItemName;
	}

	private void OnClickedOn() {
		// Remove the item from the inventory so we can shift it, store the index so we can put it back later
		_parentInventory.InventoryData.GetSlotIndexOf(_itemInstance);
		_parentInventory.InventoryData.RemoveItem(_parentInventory, _itemInstance);
		InstantiateDraggedItem();
	}

	private void OnClickUpAnywhere() {
		if (_draggedInstance == null) return;
		_draggedInstance.GetComponent<InventoryItemDraggedUI>().OnLetGoOfDraggedItem();
		_draggedInstance = null;
	}

	private void InstantiateDraggedItem() {
		// Parented to whole canvas
		_draggedInstance = Instantiate(_inventoryItemDraggedPrefab, _rectTransform.GetComponentInParent<Canvas>().transform);
		InventoryItemDraggedUI script = _draggedInstance.GetComponent<InventoryItemDraggedUI>();
		script.InitializeWithInventory(_parentInventory, _itemInstance, this);
	}

	public void ResetToOriginalPosition() {
		if (!_parentInventory.InventoryData.TryAddItemAtPosition(_parentInventory, _itemInstance, _BLContainerIndex)) {
			Debug.LogError($"Could not put item {_itemInstance.ItemData.ItemName} back in inventory. Check for unexpected inventory logic.");
			Destroy(gameObject);
		}
	}

	public void RemoveSelfFromInventory() {
		_parentInventory.InventoryData.RemoveItem(_parentInventory, _itemInstance);
		OnDestroyItem?.Invoke(gameObject);
		Destroy(gameObject);
	}

	public void MoveToInventoryAtPosition(InventoryComponent inventory, Vector2Int slotIndexBL) {
		if (!inventory.TryPlaceItem(_itemInstance, slotIndexBL)) {
			Debug.LogWarning($"Could not move item {_itemInstance.ItemData.ItemName} to {inventory.name} at position {slotIndexBL}.");
			ResetToOriginalPosition();
		}
		else {
			OnDestroyItem?.Invoke(gameObject);
			Destroy(gameObject);
		}
	}
}