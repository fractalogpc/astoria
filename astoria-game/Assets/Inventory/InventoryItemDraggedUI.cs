using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class InventoryItemDraggedUI : MonoBehaviour
{
	public InventoryItem Item { get; private set; }
	private InventoryItemUI _itemUI;
	
	[SerializeField] private RectTransform _rectTransform;
	[SerializeField] private CanvasGroup _canvasGroup;
	[SerializeField] private Image _itemIconImage;
	private InventoryUI _startingInventory;
	[SerializeField] private InventoryUI _currentInventoryAbove;
	private GraphicRaycaster _canvasGraphicRaycaster;
	private PointerEventData _pointerEventData = new(EventSystem.current);
	
	private bool _followMouse;

	private void OnValidate() {
		_rectTransform = GetComponent<RectTransform>();
		_rectTransform.anchorMin = Vector2.zero;
	}
	

	private void Start() {
		_canvasGraphicRaycaster = _rectTransform.GetComponentInParent<GraphicRaycaster>();
	}
	
	private void Update() {
		if (!_followMouse) {
			_canvasGroup.alpha = 0;
			return;
		}
		// Only works if the canvas is set to Overlay, and rectTransform is under the canvas.
		_canvasGroup.alpha = 1;
		_rectTransform.anchoredPosition = Input.mousePosition;
		GetInventoryUIHoveredOver(out _currentInventoryAbove);
		if (_currentInventoryAbove != null) {
			_startingInventory.ResetAllContainerHighlights();
			_currentInventoryAbove.HighlightSlotsUnderItem(Item, GetSlotIndexInInventory(_currentInventoryAbove, _rectTransform.anchoredPosition));
		}
	}
	
	public void Initalize(InventoryUI originalInventory, InventoryItem item, InventoryItemUI itemUI) {
		_itemUI = itemUI;
		Item = item;
		_startingInventory = originalInventory;
		_itemIconImage.sprite = item.ItemData.ItemIcon;
		_rectTransform.sizeDelta = new Vector2(item.Size.x * originalInventory.SlotSizeUnits, item.Size.y * originalInventory.SlotSizeUnits);
		_rectTransform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(item.Size.x * originalInventory.SlotSizeUnits / 2, item.Size.y * originalInventory.SlotSizeUnits / 2);
		_rectTransform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(item.Size.x * originalInventory.SlotSizeUnits, item.Size.y * originalInventory.SlotSizeUnits);
		_followMouse = true;
		_canvasGroup.alpha = 0;
	}
	
	public bool OnLetGoOfDraggedItem() {
		print($"let go of item {Item.ItemData.ItemName} Draggable");
		if (_currentInventoryAbove == null) {
			_startingInventory.ResetAllContainerHighlights();
			_itemUI.ResetToOriginalPosition();
			Destroy(gameObject);
			return false;
		}
		else {
			_startingInventory.ResetAllContainerHighlights();
			_currentInventoryAbove.ResetAllContainerHighlights();
			_itemUI.MoveToInventoryAtPosition(_currentInventoryAbove, GetSlotIndexInInventory(_currentInventoryAbove, _rectTransform.anchoredPosition));
			Destroy(gameObject);
			return true;
		}
		
	}
	
	private bool GetInventoryUIHoveredOver(out InventoryUI inventory) {
		List<RaycastResult> raycastHits = new List<RaycastResult>();
		_pointerEventData.position = Input.mousePosition;
		_canvasGraphicRaycaster.Raycast(_pointerEventData, raycastHits);
		foreach (RaycastResult hit in raycastHits) {
			if (hit.gameObject.TryGetComponent(out InventoryUI inventoryUI)) {
				inventory = inventoryUI;
				return true;
			}
		}
		inventory = null;
		return false;
	}
	
	private Vector2Int GetSlotIndexInInventory(InventoryUI inventory, Vector2 positionWS) {
		// Debug.Log("Change this logic here later to support placing items based on center instead of bottom left.");
		positionWS = positionWS + new Vector2(-_rectTransform.rect.width / 4, -_rectTransform.rect.height / 4);
		RectTransform inventoryRect = inventory.GetComponent<RectTransform>();
		Vector2 localPoint;
		localPoint = inventoryRect.InverseTransformPoint(positionWS) + new Vector3(inventoryRect.sizeDelta.x / 2, inventoryRect.sizeDelta.y / 2, 0);
		// print("item anchored pos" + positionWS);
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