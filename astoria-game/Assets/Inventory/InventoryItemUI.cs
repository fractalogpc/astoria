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
	[Header("Events")]
	public UnityEvent<GameObject> OnDestroyItem;
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
	
	/// <summary>
	/// Uses the recorded parent inventory and initial position to try to put the item back where it was.
	/// </summary>
	public void ResetToOriginalPosition() {
		if (!_parentInventory.InventoryData.TryAddItemAtPosition(_parentInventory, ItemInstance, _BLContainerIndex)) {
			Debug.LogError($"Could not put item {ItemInstance.ItemData.ItemName} back in inventory. Check for unexpected inventory logic.");
			Destroy(gameObject);
		}
	}

	/// <summary>
	/// Removes the item from the inventory and destroys the game object.
	/// If this is called without transferring the item to another inventory, the item is lost.
	/// In that case, please call the item's OnItemDestruction method before calling this function.
	/// </summary>
	public void DeleteSelfFromInventory() {
		_parentInventory.InventoryData.RemoveItem(_parentInventory, ItemInstance);
		OnDestroyItem?.Invoke(gameObject);
		Destroy(gameObject);
	}

	/// <summary>
	/// Calls the PlaceItem(ItemInstance, position) function of the given inventory with this InventoryItemUIs ItemInstance, then destroys this InventoryItemUI.
	/// </summary>
	/// <param name="inventory">The InventoryComponent the held ItemInstance will be placed in.</param>
	/// <param name="slotIndexBL">The position the held ItemInstance will be placed at in the new inventory.</param>
	public void MoveToInventoryAtPosition(InventoryComponent inventory, Vector2Int slotIndexBL) {
		if (!inventory.PlaceItem(ItemInstance, slotIndexBL)) {
			Debug.LogWarning($"Could not move item {ItemInstance.ItemData.ItemName} to {inventory.name} at position {slotIndexBL}.");
			ResetToOriginalPosition();
		}
		else {
			DeleteSelfFromInventory();
		}
	}
	
	private void Start() {
		// InitializeWithItem(Item); should be called by instantiating object
		_rectTransform = GetComponent<RectTransform>();
		_clickableEvents = GetComponent<ClickableEvents>();
		_clickableEvents.OnClickDownSelected.AddListener(OnClickedOn);
		_clickableEvents.OnClickUpAnywhere.AddListener(OnClickUpAnywhere);
	}

	private void OnDisable() {
		_clickableEvents.OnClickDownSelected.RemoveListener(OnClickedOn);
		_clickableEvents.OnClickUpAnywhere.RemoveListener(OnClickUpAnywhere);
	}
	
	private void OnClickedOn() {
		// Remove the item from the inventory so we can shift it, store the index so we can put it back later
		_parentInventory.InventoryData.GetSlotIndexOf(ItemInstance);
		_parentInventory.InventoryData.RemoveItem(_parentInventory, ItemInstance);
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
		script.InitializeFromInventory(_parentInventory, ItemInstance, this);
	}
}