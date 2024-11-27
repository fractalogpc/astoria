using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

/// <summary>
/// Attach this script to RectTransforms to create Inventory UIs.
/// </summary>
[AddComponentMenu("Inventory/Inventory Component")]
[RequireComponent(typeof(AutoRegister))]
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Image))]
public class InventoryComponent : MonoBehaviour
{
	/// <summary>
	/// WARNING: DO NOT REFERENCE THIS OUTSIDE OF THE INVENTORY SYSTEM. THIS IS MEANT FOR INVENTORY SYSTEM USE ONLY.
	/// </summary>
	[HideInInspector] public InventoryData InventoryData;

	public Vector2Int AssignedInventorySize => _assignedInventorySize;
	public float SlotSizeUnits => _slotPrefab.GetComponent<RectTransform>().sizeDelta.x;
	public UnityEvent<List<InventoryItem>> OnInventoryChange;


	[Header("Ensure that the object this is placed on is a direct child of an Overlay Canvas that has a Graphic Raycaster.")]
	[Header("Ensure Slot Prefab is Square, and has InventoryContainerUI component.")]
	[SerializeField]
	private GameObject _inventoryItemPrefab;

	[SerializeField] private GameObject _slotPrefab;

	[Header("If outside scripts initialize the inventory, don't use this.")]
	[SerializeField] private bool _useAssignedInventoryData;

	[SerializeField] private Vector2Int _assignedInventorySize;
	[SerializeField] private List<ItemData> _spawnInventoryWith;

	private List<GameObject> _inventoryItemPrefabInstances;
	private RectTransform _rect;
	private Image _colliderImage; // Need a collider image so hovered items can raycast and see the inventory
	private GameObject[,] _slotPrefabInstances;

	private void OnValidate() {
		FindReferences();
	}

	public void Start() {
		FindReferences();
		if (_useAssignedInventoryData) {
			InventoryData = null;
			CreateInvFromItemDatas(_spawnInventoryWith, _assignedInventorySize);
		}
	}

	private void FindReferences() {
		_rect = GetComponent<RectTransform>();
		if (_rect != null && _slotPrefab != null)
			_rect.sizeDelta = new Vector2(_assignedInventorySize.x * SlotSizeUnits,
				_assignedInventorySize.y * SlotSizeUnits);
		_colliderImage = GetComponent<Image>();
		_colliderImage.raycastTarget = true;
	}

	/// <summary>
	/// Creates a new InventoryData of inventorySize, containing itemDatas. Packs the item instances into the inventory.
	/// </summary>
	/// <param name="itemDatas">The items to place in the new InventoryData</param>
	/// <param name="inventorySize">The size of the InventoryData to create.</param>
	/// <returns>The number of items that could not be placed into the inventory.</returns>
	public int CreateInvFromItemDatas(List<ItemData> itemDatas, Vector2Int inventorySize) {
		InventoryData = new InventoryData(inventorySize.x, inventorySize.y);
		CreateAndAttachContainersTo(InventoryData);
		_inventoryItemPrefabInstances = new List<GameObject>();
		List<InventoryItem> items = new();
		foreach (ItemData itemData in itemDatas) {
			items.Add(new InventoryItem(itemData));
		}

		int notPlaced = TryAddItemsByData(itemDatas);
		if (notPlaced > 0) {
			Debug.LogWarning($"Could not place all items in inventory of {gameObject.name}. Some items may be too large or the inventory too small.");
			return notPlaced;
		}

		return 0;
	}

	/// <summary>
	/// Instantiates the component with the InventoryData.
	/// </summary>
	/// <param name="inventoryData">The InventoryData to instantiate with.</param>
	public void CreateInvFromInventoryData(InventoryData inventoryData) {
		CreateAndAttachContainersTo(inventoryData);
		InstantiateInventoryItems(inventoryData);
	}

	private void Update() {
		if (InventoryData == null) return;
		ResetAllContainerHighlights();
	}

	public void CreateAndAttachContainersTo(InventoryData inventoryData) {
		DeleteChildrenOf(_rect.transform);
		_slotPrefabInstances = new GameObject[inventoryData.Width, inventoryData.Height];
		for (int y = 0; y < inventoryData.Height; y++) {
			for (int x = 0; x < inventoryData.Width; x++) {
				GameObject slot = Instantiate(_slotPrefab, _rect.transform);
				slot.name = $"Slot {x}, {y}";
				slot.GetComponent<RectTransform>().anchoredPosition = new Vector2(x * SlotSizeUnits + SlotSizeUnits / 2, y * SlotSizeUnits + SlotSizeUnits / 2);
				slot.GetComponent<InventoryContainerUI>().AttachContainer(inventoryData.Containers[x, y]);
				_slotPrefabInstances[x, y] = slot;
			}
		}

		_colliderImage = GetComponent<Image>();
		_colliderImage.raycastTarget = true;
	}

	private void InstantiateInventoryItems(InventoryData inventoryData) {
		_inventoryItemPrefabInstances = new List<GameObject>();
		foreach (InventoryItem item in inventoryData.Items) {
			CreateItemPrefab(item, inventoryData.GetSlotIndexOf(item));
		}
	}

	public void DestroyInventoryContainers() {
		DeleteChildrenOf(transform);
	}

	/// <summary>
	/// Gets a list of all item instances that match the persistent ItemData.
	/// </summary>
	/// <param name="itemData">The ItemData to match against.</param>
	/// <returns>A list of InventoryItem that match the ItemData.</returns>
	public List<InventoryItem> GetItemsOfType(ItemData itemData) {
		return InventoryData.Items.FindAll(item => item.ItemData == itemData);
	}

	/// <summary>
	/// Finds whether there are count instances or more of the item in the inventory.
	/// </summary>
	/// <param name="itemData">The ItemData to match against.</param>
	/// <param name="count">The count of instances.</param>
	/// <returns>True if there are count or more InventoryItems matching itemData.</returns>
	public bool ItemCountOrMoreInInventory(ItemData itemData, int count = 1) {
		List<InventoryItem> matchingItemInstances = InventoryData.Items.FindAll(item => item.ItemData == itemData);
		if (matchingItemInstances.Count < count) return false;
		return true;
	}

	/// <summary>
	/// Attempts to add count items to the inventory. If items are non-rectangular, this does not pack items very well.
	/// Use this when interacting with the inventory from non-inventory systems.
	/// </summary>
	/// <param name="itemData">The ItemData to instantiate InventoryItems with, and add to the inventory.</param>
	/// <param name="count">The count of InventoryItems to instantiate.</param>
	/// <returns>Whether adding all the items was successful.</returns>
	public bool TryAddItemsByData(ItemData itemData) {
		InventoryItem item = new(itemData);
		if (!InventoryData.TryAddItem(item, out Vector2Int slotIndexBL)) return false;
		CreateItemPrefab(item, slotIndexBL);

		OnInventoryChange.Invoke(InventoryData.Items);
		return true;
	}

	/// <summary>
	/// Attempts to add the items to the inventory. If items are non-rectangular, this does not pack items very well. Use this when interacting with the inventory from non-inventory systems.
	/// </summary>
	/// <param name="items">The ItemData to instantiate InventoryItems with, and add to the inventory.</param>
	/// <returns>The amount of items left over.</returns>
	public int TryAddItemsByData(List<ItemData> items) {
		int itemsPlaced = 0;
		if (items.Count == 0) return 0;
		foreach (ItemData item in items) {
			if (!TryAddItemsByData(item)) {
				Debug.LogWarning($"Could not add item {item.ItemName} to {gameObject.name}.");
				continue;
			}

			itemsPlaced++;
		}

		return items.Count - itemsPlaced;
	}

	/// <summary>
	/// Tries to remove count items from the inventory that match the ItemData.
	/// </summary>
	/// <param name="itemData">The ItemData to check against.</param>
	/// <param name="count">The count of items to remove.</param>
	/// <returns>Whether the count of matching items could be removed.</returns>
	public bool TryRemoveItemByData(ItemData itemData, int count = 1) {
		List<GameObject> itemInstancesToRemove = new();
		foreach (GameObject itemUIInstance in _inventoryItemPrefabInstances) {
			InventoryItemUI itemUIScript = itemUIInstance.GetComponent<InventoryItemUI>();
			if (itemInstancesToRemove.Count == count) break;
			if (itemUIScript.Item.ItemData == itemData) itemInstancesToRemove.Add(itemUIInstance);
		}

		Debug.Log($"Found {itemInstancesToRemove.Count} {itemData.ItemName} in {gameObject.name}. Trying to remove {count}.");
		if (itemInstancesToRemove.Count != count) return false;
		// for future reference, if second term in the for loop == true, keep iterating
		for (int i = count - 1; i >= 0; i--) {
			InventoryItemUI itemUIScript = itemInstancesToRemove[i].GetComponent<InventoryItemUI>();
			_inventoryItemPrefabInstances.Remove(itemInstancesToRemove[i]);
			itemUIScript.RemoveSelfFromInventory();
		}

		OnInventoryChange.Invoke(InventoryData.Items);
		return true;
	}

	private GameObject CreateItemPrefab(InventoryItem item, Vector2Int slotIndexBL) {
		GameObject itemPrefab = Instantiate(_inventoryItemPrefab, _rect.transform);
		itemPrefab.name = item.ItemData.ItemName + Random.Range(0, 100000);
		RectTransform itemRect = itemPrefab.GetComponent<RectTransform>();
		InventoryItemUI newItemUIScript = itemPrefab.GetComponent<InventoryItemUI>();
		newItemUIScript.InitializeWithItem(item, this);
		newItemUIScript.OnDestroyItem.AddListener(RemoveItemFromInstancesList);
		itemRect.anchoredPosition = new Vector2(slotIndexBL.x * SlotSizeUnits + SlotSizeUnits * item.Size.x / 2,
			slotIndexBL.y * SlotSizeUnits + SlotSizeUnits * item.Size.y / 2);
		itemRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, item.Size.x * SlotSizeUnits);
		itemRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, item.Size.y * SlotSizeUnits);
		// print($"Item {item.ItemData.ItemName} placed at {slotIndexBL} in {gameObject.name}. Size: {itemRect.rect.size}. Position: {itemRect.anchoredPosition}");
		_inventoryItemPrefabInstances.Add(itemPrefab);
		return itemPrefab;
	}

	private void RemoveItemFromInstancesList(GameObject itemPrefabInstance) {
		_inventoryItemPrefabInstances.Remove(itemPrefabInstance);
	}

	private void DeleteChildrenOf(Transform parent) {
		for (int i = parent.childCount - 1; i >= 0; i--) {
			DestroyImmediate(parent.GetChild(i).gameObject);
		}
	}

	/// <summary>
	/// Tries to place the item with the bottom left at the slot position closest to positionSS.
	/// </summary>
	/// <param name="item">The InventoryItem to place.</param>
	/// <param name="positionSS">The position to place at in screen space.</param>
	/// <returns></returns>
	public bool TryPlaceItem(InventoryItem item, Vector2Int slotIndexBL) {
		GameObject cntrSlot = _slotPrefabInstances[slotIndexBL.x, slotIndexBL.y];
		InventoryContainerUI cntrSlotScript = cntrSlot.GetComponent<InventoryContainerUI>();
		Vector2Int index = cntrSlotScript.AttachedContainer.Index;
		bool couldPlace = InventoryData.TryAddItemAtPosition(item, index);
		if (couldPlace) {
			CreateItemPrefab(item, index);
			return true;
		}

		return false;
	}

	public bool HighlightSlotsUnderItem(InventoryItem item, Vector2Int slotIndexBL) {
		List<InventoryContainer> containersItemOverlaps = new();
		bool couldPlace = true;
		for (int y = slotIndexBL.y; y < slotIndexBL.y + item.Size.y; y++) {
			for (int x = slotIndexBL.x; x < slotIndexBL.x + item.Size.x; x++) {
				try {
					InventoryContainer container = _slotPrefabInstances[x, y].GetComponent<InventoryContainerUI>().AttachedContainer;
					containersItemOverlaps.Add(container);
				}
				catch (IndexOutOfRangeException) {
					couldPlace = false;
				}
			}
		}
		
		for (int i = 0; i < containersItemOverlaps.Count; i++) {
			if (containersItemOverlaps[i].HeldItem != null) {
				couldPlace = false;
				break;
			}
		}

		if (couldPlace) {
			HighlightContainers(containersItemOverlaps, ContainerHighlight.Green);
			return true;
		}
		else {
			HighlightContainers(containersItemOverlaps, ContainerHighlight.Red);
			return false;
		}
	}

	public void ResetAllContainerHighlights() {
		for (int y = 0; y < InventoryData.Height; y++) {
			for (int x = 0; x < InventoryData.Width; x++) {
				InventoryData.Containers[x, y].Highlight = ContainerHighlight.None;
			}
		}
	}

	private void HighlightContainers(List<InventoryContainer> containers, ContainerHighlight highlight) {
		ResetAllContainerHighlights();
		for (int i = 0; i < containers.Count; i++) {
			containers[i].Highlight = highlight;
		}
	}
}