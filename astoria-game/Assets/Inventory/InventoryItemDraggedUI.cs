using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[AddComponentMenu("")]
public class InventoryItemDraggedUI : MonoBehaviour
{
	public InventoryItem Item { get; private set; }
	private InventoryItemUI _itemUI;

	[SerializeField] private RectTransform _rectTransform;
	[SerializeField] private CanvasGroup _canvasGroup;
	[SerializeField] private Image _itemIconImage;
	private InventoryComponent _startingInventory;
	private InventoryEquipableSlot _startingSlot;
	[SerializeField] private InventoryComponent _currentInventoryAbove;
	private GraphicRaycaster _canvasGraphicRaycaster;
	private PointerEventData _pointerEventData = new(EventSystem.current);

	private bool _followMouse;

	private void OnValidate()
	{
		_rectTransform = GetComponent<RectTransform>();
		_rectTransform.anchorMin = Vector2.zero;
	}


	private void Start()
	{
		_canvasGraphicRaycaster = _rectTransform.GetComponentInParent<GraphicRaycaster>();
		print($"graphic raycaster for {gameObject.name} is on {_canvasGraphicRaycaster.gameObject.name}");
	}

	private void Update()
	{
		if (!_followMouse)
		{
			_canvasGroup.alpha = 0;
			return;
		}
		// Only works if the canvas is set to Overlay, and rectTransform is under the canvas.
		_canvasGroup.alpha = 1;
		_rectTransform.anchoredPosition = Input.mousePosition;
		GetInventoryUIHoveredOver(out _currentInventoryAbove);
		if (_currentInventoryAbove != null)
		{
			_startingInventory?.ResetAllContainerHighlights();
			_currentInventoryAbove.HighlightSlotsUnderItem(Item, GetSlotIndexInInventory(_currentInventoryAbove, _rectTransform.anchoredPosition));
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			RotateItem();
		}
	}

	private void RotateItem()
	{
		Item.Rotated = !Item.Rotated;
		SetVisualSize();
	}
	
	/// <summary>
	/// Used on inventory components.
	/// </summary>
	/// <param name="originalInventory">The inventory that the InventoryUI spawning the object belongs to.</param>
	/// <param name="item">The item this draggable holds.</param>
	/// <param name="itemUI">The InventoryUI that this object was spawned by.</param>
	public void InitializeWithInventory(InventoryComponent originalInventory, InventoryItem item, InventoryItemUI itemUI)
	{
		_itemUI = itemUI;
		Item = item;
		_startingInventory = originalInventory;
		_itemIconImage.sprite = Item.ItemData.ItemIcon;
		SetVisualSize();
		_followMouse = true;
		_canvasGroup.alpha = 0;
	}

	/// <summary>
	/// Used with WeaponEquipSlot. It doesn't have an InventoryComponent or InventoryItemUI.
	/// </summary>
	/// <param name="item">The item this draggable will hold.</param>
	public void InitializeWithSlot(InventoryEquipableSlot slot, InventoryItem item) {
		_startingSlot = slot;
		Item = item;
		_itemIconImage.sprite = Item.ItemData.ItemIcon;
		SetVisualSize();
		_followMouse = true;
		_canvasGroup.alpha = 0;
	}

	private void SetVisualSize()
	{
		if (_startingInventory != null) {
			_rectTransform.sizeDelta = new Vector2(Item.Size.x * _startingInventory.SlotSizeUnits, Item.Size.y * _startingInventory.SlotSizeUnits);
			_rectTransform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(Item.Size.x * _startingInventory.SlotSizeUnits / 2, Item.Size.y * _startingInventory.SlotSizeUnits / 2);
			_rectTransform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(Item.Size.x * _startingInventory.SlotSizeUnits, Item.Size.y * _startingInventory.SlotSizeUnits);
		}
		else {
			Debug.LogWarning("Make item slot size some kind of global variable, and make sure to refactor everything to use it when you do. Currently 96px.");
			_rectTransform.sizeDelta = new Vector2(Item.Size.x * 96, Item.Size.y * 96);
			_rectTransform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(Item.Size.x * 96f / 2f, Item.Size.y * 96f / 2f);
			_rectTransform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(Item.Size.x * 96, Item.Size.y * 96);
		}
	}

	// Look, I know its messy, but trust me, it makes sense. God help me if there are other kinds of slots though.
	
	/// <summary>
	/// Called when the mouse is let go of.
	/// </summary>
	/// <returns>Returns whether the item was successfully transferred.</returns>
	public bool OnLetGoOfDraggedItem()
	{
		bool overSlot = GetEquipableSlotHoveredOver(out InventoryEquipableSlot slot);
		
		// Over another inventory
		if (_currentInventoryAbove != null)
		{
			_startingInventory?.ResetAllContainerHighlights();
			_currentInventoryAbove.ResetAllContainerHighlights();
			// Started from an inventory
			if (_itemUI != null) {
				_itemUI.MoveToInventoryAtPosition(_currentInventoryAbove, GetSlotIndexInInventory(_currentInventoryAbove, _rectTransform.anchoredPosition));
			}
			// Started from a slot
			else {
				Vector2Int slotIndex = GetSlotIndexInInventory(_currentInventoryAbove, _rectTransform.anchoredPosition);
				if (!_currentInventoryAbove.TryPlaceItem(Item, slotIndex)) {
					if (!_startingSlot.TryAddToSlot(Item)) {
						Debug.LogError("InventoryItemDraggedUI: Could not return item to slot. Check for unexpected draggable or slot logic.");
					}
				}
			}
			
			Destroy(gameObject);
			return true;
		}
		// Over an equipable slot
		if (overSlot) {
			if (slot.TryAddToSlot(Item))
			{
				_itemUI?.RemoveSelfFromInventory();
				Destroy(gameObject);
				return true;
			}
			_startingInventory?.ResetAllContainerHighlights();
			_itemUI?.ResetToOriginalPosition();
			Destroy(gameObject);
			return false;
		}
		// Over nothing
		_startingInventory?.ResetAllContainerHighlights();
		_startingInventory?.SpawnDroppedItem(Item);
		_itemUI?.RemoveSelfFromInventory();
		Destroy(gameObject);
		return false;
	}


	private bool GetInventoryUIHoveredOver(out InventoryComponent inventory)
	{
		List<RaycastResult> raycastHits = new List<RaycastResult>();
		_pointerEventData.position = Input.mousePosition;
		_canvasGraphicRaycaster.Raycast(_pointerEventData, raycastHits);
		foreach (RaycastResult hit in raycastHits)
		{
			if (hit.gameObject.TryGetComponent(out InventoryComponent inventoryUI))
			{
				inventory = inventoryUI;
				return true;
			}
		}
		inventory = null;
		return false;
	}
	
	private bool GetEquipableSlotHoveredOver(out InventoryEquipableSlot equipableSlot)
	{
		List<RaycastResult> raycastHits = new List<RaycastResult>();
		_pointerEventData.position = Input.mousePosition;
		_canvasGraphicRaycaster.Raycast(_pointerEventData, raycastHits);
		foreach (RaycastResult hit in raycastHits)
		{
			if (hit.gameObject.TryGetComponent(out InventoryEquipableSlot equipableSlotScript))
			{
				equipableSlot = equipableSlotScript;
				return true;
			}
		}
		equipableSlot = null;
		return false;
	}

	private Vector2Int GetSlotIndexInInventory(InventoryComponent inventory, Vector2 positionSS)
	{
		// Add an offset to get the position of the bottom left grid slot of the item.
		positionSS += new Vector2(-_rectTransform.rect.width / 2 + _rectTransform.rect.width / Item.Size.x / 2, -_rectTransform.rect.height / 2 + _rectTransform.rect.height / Item.Size.y / 2);
		RectTransform inventoryRect = inventory.GetComponent<RectTransform>();
		Vector2 localPoint = inventoryRect.InverseTransformPoint(positionSS) + new Vector3(inventoryRect.sizeDelta.x / 2, inventoryRect.sizeDelta.y / 2, 0);
		// print("item anchored pos" + positionSS);
		// print("item transform.position" + _rectTransform.position);
		// print("mouse pos" + Input.mousePosition);
		// print("Inventory TransformPoint: " + localPoint);
		Vector2Int slotIndex = new Vector2Int(
			Mathf.FloorToInt(localPoint.x / inventory.SlotSizeUnits),
			Mathf.FloorToInt(localPoint.y / inventory.SlotSizeUnits)
		);
		return slotIndex;
	}
}