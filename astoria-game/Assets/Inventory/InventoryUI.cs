using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

/// <summary>
/// Attach this script to RectTransforms to create Inventory UIs.
/// </summary>
[RequireComponent(typeof(AutoRegister))]
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Image))]
public class InventoryUI : MonoBehaviour, IStartExecution
{
	[Header("Refs, Should be assigned by default.")]
	[SerializeField] private GameObject _inventoryItemPrefab;
	private List<GameObject> _inventoryItemPrefabInstances;
	private RectTransform _rect;
	private Image _colliderImage; // Need a collider image so hovered items can raycast and see the inventory
	[Header("Ensure Slot Prefab is Square, and has InventoryContainerUI component.")]
	[SerializeField] private GameObject _slotPrefab;
	private GameObject[,] _slotPrefabInstances;
	[Header("Settings")]
	[SerializeField] private Vector2Int InventorySize;
	/// <summary>
	/// WARNING: DO NOT USE THIS OUTSIDE OF THE INVENTORY SYSTEM. THIS IS MEANT FOR INVENTORY SYSTEM USE ONLY.
	/// </summary>
	public Inventory InventoryData;
	[Header("Ensure that this object is placed on an Overlay Canvas that has a Graphic Raycaster, and is a direct child of it.")]
	private List<InventoryItem> _itemsAssignedInEditor;
	public float SlotSizeUnits => _slotPrefab.GetComponent<RectTransform>().sizeDelta.x;

	
	private void OnValidate() {
		_rect = GetComponent<RectTransform>();
		if (_rect != null && _slotPrefab != null) {
			_rect.sizeDelta = new Vector2(InventorySize.x * SlotSizeUnits, InventorySize.y * SlotSizeUnits);
		}
		_colliderImage = GetComponent<Image>();
		_colliderImage.raycastTarget = true;
	}
	public void InitializeStart() {
		Debug.Log("Remind Matthew to add rotating items later.");
		if (InventoryData != null) {
			_itemsAssignedInEditor = InventoryData.Items;
		}
		InitializeInventoryContainers();
		if (InstanceEditorItemsIntoInventory() > 0) {
			Debug.LogWarning($"Could not place all items in {gameObject.name}. Some items may be too large or the inventory too small.");
		}
	}

	private void Update() {
		ResetAllContainerHighlights();
	}

	public void InitializeInventoryContainers() {
		DeleteChildrenOf(_rect.transform);
		InventoryData = new Inventory(InventorySize.x, InventorySize.y);
		_slotPrefabInstances = new GameObject[InventoryData.Width, InventoryData.Height];
		for (int y = 0; y < InventoryData.Height; y++) {
			for (int x = 0; x < InventoryData.Width; x++) {
				GameObject slot = Instantiate(_slotPrefab, _rect.transform);
				slot.GetComponent<RectTransform>().anchoredPosition = new Vector2(x * SlotSizeUnits + SlotSizeUnits / 2, y * SlotSizeUnits + SlotSizeUnits / 2);
				slot.GetComponent<InventoryContainerUI>().AttachContainer(InventoryData.Containers[x, y]);
				_slotPrefabInstances[x, y] = slot;
			}
		}
		_colliderImage = GetComponent<Image>();
		_colliderImage.raycastTarget = true;
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
	/// Finds whether there are exactly count instances of the item in the inventory.
	/// </summary>
	/// <param name="itemData">The ItemData to match against.</param>
	/// <param name="count">The count of instances.</param>
	/// <returns>True if there are count InventoryItems matching itemData.</returns>
	public bool ItemCountInInventory(ItemData itemData, int count = 1) {
		List<InventoryItem> matchingItemInstances = InventoryData.Items.FindAll(item => item.ItemData == itemData);
		if (matchingItemInstances.Count != count) return false;
		return true;
	}
	/// <summary>
	/// Attempts to add count items to the inventory. This does not pack items very well. It is recommended to have the player pick up items one by one.
	/// </summary>
	/// <param name="itemData">The ItemData to instantiate InventoryItems with, and add to the inventory.</param>
	/// <param name="count">The count of InventoryItems to instantiate.</param>
	/// <returns>Whether or not adding all the items was successful.</returns>
	public bool TryAddItemByData(ItemData itemData, int count = 1) {
		InventoryItem item = new InventoryItem(itemData);
		if (!InventoryData.TryAddItem(item, out Vector2Int slotIndexBL)) {
			return false;
		}
		CreateItemPrefab(item, slotIndexBL);
		return true;
	}
	/// <summary>
	/// Tries to remove count items from the inventory that match the ItemData.
	/// </summary>
	/// <param name="itemData">The ItemData to check against.</param>
	/// <param name="count">The count of items to remove.</param>
	/// <returns>Whether or not the count of matching items could be removed.</returns>
	public bool TryRemoveItemByData(ItemData itemData, int count = 1) {
		List<GameObject> itemInstancesToRemove = new();
		foreach (GameObject itemUIInstance in _inventoryItemPrefabInstances) {
			InventoryItemUI itemUIScript = itemUIInstance.GetComponent<InventoryItemUI>();
			if (itemInstancesToRemove.Count == count) break;
			if (itemUIScript.Item.ItemData == itemData) {
				itemInstancesToRemove.Add(itemUIInstance);
			}
		}
		if (itemInstancesToRemove.Count != count) return false;
		for (int i = itemInstancesToRemove.Count; i == 0; i--) {
			InventoryItemUI itemUIScript = itemInstancesToRemove[i].GetComponent<InventoryItemUI>();
			itemUIScript.RemoveSelfFromInventory();
		}
		return true;
	}
	
	
	private int InstanceEditorItemsIntoInventory() {
		int itemsPlaced = 0;
		if (_itemsAssignedInEditor.Count == 0) return 0;
		foreach (InventoryItem item in _itemsAssignedInEditor) {
			if (!InventoryData.TryAddItem(item, out Vector2Int slotIndexBL)) {
				Debug.LogWarning($"Could not add item {item.ItemData.ItemName} to {gameObject.name}.");
				continue;
			}
			CreateItemPrefab(item, slotIndexBL);
			itemsPlaced++;
		}
		return _itemsAssignedInEditor.Count - itemsPlaced;
	}
	private GameObject CreateItemPrefab(InventoryItem item, Vector2Int slotIndexBL) {
		GameObject itemPrefab = Instantiate(_inventoryItemPrefab, _rect.transform);
		itemPrefab.name = item.ItemData.ItemName + Random.Range(0, 100000).ToString();
		RectTransform itemRect = itemPrefab.GetComponent<RectTransform>();
		InventoryItemUI newItemUIScript = itemPrefab.GetComponent<InventoryItemUI>();
		newItemUIScript.InitializeWithItem(item, this);
		itemRect.anchoredPosition = new Vector2(slotIndexBL.x * SlotSizeUnits + SlotSizeUnits * item.Size.x / 2, slotIndexBL.y * SlotSizeUnits + SlotSizeUnits * item.Size.y / 2);
		itemRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, item.Size.x * SlotSizeUnits);
		itemRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, item.Size.y * SlotSizeUnits);
		// print($"Item {item.ItemData.ItemName} placed at {slotIndexBL} in {gameObject.name}. Size: {itemRect.rect.size}. Position: {itemRect.anchoredPosition}");
		_inventoryItemPrefabInstances.Add(itemPrefab);
		return itemPrefab;
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
		InventoryContainer originContainer = _slotPrefabInstances[slotIndexBL.x, slotIndexBL.y].GetComponent<InventoryContainerUI>().AttachedContainer;
		if (originContainer == null) {
			ResetAllContainerHighlights();
			return false;
		}
		containersItemOverlaps.Add(originContainer);
		for (int y = slotIndexBL.y; y < slotIndexBL.y + item.Size.y; y++) {
			for (int x = slotIndexBL.x; x < slotIndexBL.x + item.Size.x; x++) {
				if (x == slotIndexBL.x && y == slotIndexBL.y) continue;
				try {
					InventoryContainer container = _slotPrefabInstances[x, y].GetComponent<InventoryContainerUI>().AttachedContainer;
					containersItemOverlaps.Add(container);
				}
				catch (IndexOutOfRangeException) {
					HighlightContainers(containersItemOverlaps, ContainerHighlight.Red);
					return false;
				}
			}
		}
		bool allContainersEmpty = true;
		for (int i = 0; i < containersItemOverlaps.Count; i++) {
			if (containersItemOverlaps[i].HeldItem != null) {
				allContainersEmpty = false;
				break;
			}
		}
		if (allContainersEmpty) {
			HighlightContainers(containersItemOverlaps, ContainerHighlight.Green);
			return true;
		}
		HighlightContainers(containersItemOverlaps, ContainerHighlight.Red);
		return false;
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