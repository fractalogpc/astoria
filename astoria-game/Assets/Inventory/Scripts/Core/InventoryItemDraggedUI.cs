using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vector2Int = UnityEngine.Vector2Int;

[AddComponentMenu("")]
public class InventoryItemDraggedUI : MonoBehaviour
{
	public ItemStack ItemStack { get; private set; }

	[SerializeField] private RectTransform _rectTransform;
	[SerializeField] private CanvasGroup _canvasGroup;
	[SerializeField] private Image _itemIconImage;
	[SerializeField] private InventoryComponent _currentInventoryAbove;

	private Vector2Int _originSlotIndex;
	private InventoryComponent _originInventory;
	private InventoryEquipableSlot _originSlot;
	private GraphicRaycaster _canvasGraphicRaycaster;
	private PointerEventData _pointerEventData = new(EventSystem.current);
	private bool _followMouse;

	/// <summary>
	/// Used on inventory components.
	/// </summary>
	/// <param name="originalInventory">The inventory that the InventoryUI spawning the object belongs to.</param>
	/// <param name="itemInstance">The item this draggable holds.</param>
	/// <param name="itemUI">The InventoryUI that this object was spawned by.</param>
	public void InitializeFromInventory(InventoryComponent originalInventory, ItemStack itemStack, Vector2Int slotIndexBL, float slotSize)
	{
		ItemStack = itemStack;
		_originInventory = originalInventory;
		_originSlotIndex = slotIndexBL;
		_itemIconImage.sprite = ItemStack.StackType.ItemIcon;
		SetVisualSize();
		_followMouse = true;
		_canvasGroup.alpha = 0;
	}

	/// <summary>
	/// Used with WeaponEquipSlot. It doesn't have an InventoryComponent or InventoryItemUI.
	/// </summary>
	/// <param name="itemInstance">The item this draggable will hold.</param>
	public void InitializeFromSlot(InventoryEquipableSlot slot, ItemInstance itemInstance)
	{
		_originSlot = slot;
		ItemStack = new ItemStack(itemInstance);
		_itemIconImage.sprite = ItemStack.StackType.ItemIcon;
		SetVisualSize();
		_followMouse = true;
		_canvasGroup.alpha = 0;
	}

	// Look, I know its messy, but trust me, it makes sense.

	/// <summary>
	/// Called when the mouse is let go of.
	/// </summary>
	/// <returns>Returns whether the item was successfully transferred.</returns>
	public bool OnLetGoOfDraggedItem()
	{
		bool overSlot = GetEquipableSlotHoveredOver(out InventoryEquipableSlot slot);
		bool overItemUI = GetItemUIHoveredOver(out InventoryStackUI itemUI);

		// Over another inventory
		if (_currentInventoryAbove != null)
		{
			_originInventory?.ResetAllContainerHighlights();
			_currentInventoryAbove.ResetAllContainerHighlights();
			Vector2Int slotIndexBL = GetSlotIndexInInventory(_currentInventoryAbove, _rectTransform.anchoredPosition);
			if (_currentInventoryAbove.PlaceStack(ItemStack, slotIndexBL))
			{
				Destroy(gameObject);
				return true;
			}
			// Could not fit item over hover spot
			Debug.LogWarning($"Could not move item {ItemStack.StackType.ItemName} to {_currentInventoryAbove.name} at position {slotIndexBL}.");
			if (_originInventory != null)
			{
				ReturnToOriginalInventory();
			}
			else
			{
				ReturnToOriginalSlot();
			}
			return false;
		}

		// Over an equipable slot
		if (overSlot)
		{
			bool itemsLeft = ItemStack.Pop(out ItemInstance item);
			if (!slot.TryAddToSlot(item))
			{
				ItemStack.Push(item);
				itemsLeft = true;
			}
			if (!itemsLeft)
			{
				Destroy(gameObject);
				return true;
			}
			if (_originInventory != null)
			{
				ReturnToOriginalInventory();
			}
			else
			{
				ReturnToOriginalSlot();
			}
			Destroy(gameObject);
			return true;
		}

		// Over nothing, drop the item
		_originInventory?.ResetAllContainerHighlights();
		_originInventory?.SpawnDroppedItem(ItemStack);
		Debug.Log($"Inventory: Dropping Item {ItemStack.StackType.ItemName}");
		Destroy(gameObject);
		return false;
	}

	/// <summary>
	/// Uses the recorded parent inventory and initial position to try to put the item back where it was.
	/// </summary>
	private void ReturnToOriginalInventory()
	{
		if (!_originInventory.PlaceStack(ItemStack, _originSlotIndex))
		{
			_originInventory.AddStack(ItemStack);
			Debug.LogWarning($"Could not put item {ItemStack.StackType.ItemName} back in inventory at origin position. If a new item was added while dragging, this is expected.");
		}
		Destroy(gameObject);
	}

	private void ReturnToOriginalSlot()
	{
		ItemStack.Pop(out ItemInstance item);
		// If the item came from a slot, there couldn't be more than one item in the stack.
		// This is bad, but later slots wont hold items anyways, so its fine.
		if (!_originSlot.TryAddToSlot(item))
		{
			Debug.LogError("InventoryItemDraggedUI: Could not return item to slot. Check for unexpected draggable or slot logic.");
		}
		Destroy(gameObject);
	}

	private void OnValidate()
	{
		_rectTransform = GetComponent<RectTransform>();
		_rectTransform.anchorMin = Vector2.zero;
	}

	private void Start()
	{
		_canvasGraphicRaycaster = _rectTransform.GetComponentInParent<GraphicRaycaster>();
		// print($"graphic raycaster for {gameObject.name} is on {_canvasGraphicRaycaster.gameObject.name}");
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

		// Assign anchored position because it's used for other stuff
		_rectTransform.anchoredPosition = Input.mousePosition;

		GetInventoryUIHoveredOver(out _currentInventoryAbove);
		if (_currentInventoryAbove != null)
		{
			_originInventory?.ResetAllContainerHighlights();
			_currentInventoryAbove.HighlightSlotsUnderStack(ItemStack, GetSlotIndexInInventory(_currentInventoryAbove, _rectTransform.anchoredPosition));
		}
		if (Input.GetMouseButtonUp(0)) OnLetGoOfDraggedItem();
		if (Input.GetKeyDown(KeyCode.R)) RotateItem();

		// // Now set the position because it needs to be scaled by screen size
		// // Yeah it's not great to fetch parent component every frame but someone else (matthew) can fix this if they feel like it.
		// // Convert mouse position to local position relative to the canvas
		// Vector2 localPoint;
		// Canvas canvas = GetComponentInParent<Canvas>();
		// Camera camera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
		//
		// if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform.parent as RectTransform, Input.mousePosition, camera, out localPoint))
		// {
		// 	// Adjust for the screen center offset
		// 	Vector2 parentSize = (_rectTransform.parent as RectTransform).sizeDelta;
		// 	localPoint += parentSize * 0.5f;
		//
		// 	_rectTransform.anchoredPosition = localPoint;
		// }
	}

	private void RotateItem()
	{
		ItemStack.Rotated = !ItemStack.Rotated;
		SetVisualSize();
	}

	private void SetVisualSize()
	{
		if (_originInventory != null)
		{
			_rectTransform.sizeDelta = new Vector2(ItemStack.Size.x * _originInventory.SlotSizeUnits, ItemStack.Size.y * _originInventory.SlotSizeUnits);
			_rectTransform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(ItemStack.Size.x * _originInventory.SlotSizeUnits / 2, ItemStack.Size.y * _originInventory.SlotSizeUnits / 2);
			_rectTransform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(ItemStack.Size.x * _originInventory.SlotSizeUnits, ItemStack.Size.y * _originInventory.SlotSizeUnits);
		}
		else
		{
			Debug.LogWarning("Make item slot size some kind of global variable, and make sure to refactor everything to use it when you do. Currently 96px.");
			_rectTransform.sizeDelta = new Vector2(ItemStack.Size.x * 96, ItemStack.Size.y * 96);
			_rectTransform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(ItemStack.Size.x * 96f / 2f, ItemStack.Size.y * 96f / 2f);
			_rectTransform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(ItemStack.Size.x * 96, ItemStack.Size.y * 96);
		}
		_itemIconImage.rectTransform.pivot = new Vector2(0.5f, 0.5f);
		_itemIconImage.rectTransform.anchoredPosition = Vector2.zero;

		float slotSizeUnits = _originInventory != null ? _originInventory.SlotSizeUnits : _originSlot.SlotSizeUnits;

		_itemIconImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ItemStack.StackType.ItemSize.x * slotSizeUnits);
		_itemIconImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ItemStack.StackType.ItemSize.y * slotSizeUnits);

		_itemIconImage.sprite = ItemStack.StackType.ItemIcon;
		_itemIconImage.rectTransform.rotation = ItemStack.Rotated ? Quaternion.Euler(0, 0, 90) : Quaternion.identity;
	}

	private bool GetInventoryUIHoveredOver(out InventoryComponent inventory)
	{
		List<RaycastResult> raycastHits = new();
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
		List<RaycastResult> raycastHits = new();
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

	private bool GetItemUIHoveredOver(out InventoryStackUI stackUI)
	{
		List<RaycastResult> raycastHits = new();
		_pointerEventData.position = Input.mousePosition;
		_canvasGraphicRaycaster.Raycast(_pointerEventData, raycastHits);
		foreach (RaycastResult hit in raycastHits)
		{
			if (hit.gameObject.TryGetComponent(out InventoryStackUI itemUIScript))
			{
				stackUI = itemUIScript;
				return true;
			}
		}

		stackUI = null;
		return false;
	}

	private Vector2Int GetSlotIndexInInventory(InventoryComponent inventory, Vector2 positionSS)
	{
		// Add an offset to get the position of the bottom left grid slot of the item.
		positionSS += new Vector2(-_rectTransform.rect.width / 2 + _rectTransform.rect.width / ItemStack.Size.x / 2, -_rectTransform.rect.height / 2 + _rectTransform.rect.height / ItemStack.Size.y / 2);
		RectTransform inventoryRect = inventory.GetComponent<RectTransform>();
		Vector2 localPoint = inventoryRect.InverseTransformPoint(positionSS) + new Vector3(inventoryRect.sizeDelta.x / 2, inventoryRect.sizeDelta.y / 2, 0);
		// print("item anchored pos" + positionSS);
		// print("item transform.position" + _rectTransform.position);
		// print("mouse pos" + Input.mousePosition);
		// print("Inventory TransformPoint: " + localPoint);
		Vector2Int slotIndex = new(
			Mathf.FloorToInt(localPoint.x / inventory.SlotSizeUnits),
			Mathf.FloorToInt(localPoint.y / inventory.SlotSizeUnits)
		);
		return slotIndex;
	}
}