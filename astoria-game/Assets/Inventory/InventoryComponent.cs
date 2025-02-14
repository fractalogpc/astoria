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
	[HideInInspector] public InventoryData InventoryData = null;

	public Vector2Int AssignedInventorySize => _assignedInventorySize;
	public float SlotSizeUnits => _slotPrefab.GetComponent<RectTransform>().sizeDelta.x;
	/// <summary>
	/// Includes a list of the InventoryItems in the inventory.
	/// </summary>
	public UnityEvent<List<ItemInstance>> OnInventoryChange;


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

	public void Awake() {
		FindReferences();
		if (_useAssignedInventoryData) {
			InventoryData = null;
			CreateInvFromItemDatas(_spawnInventoryWith, _assignedInventorySize);
		}
	}

	private void AttachToInventoryData(InventoryData inventoryData) {
		InventoryData = inventoryData;
		InventoryData.OnInventoryUpdate += UpdateInventory;
		_rect.sizeDelta = new Vector2(InventoryData.Width * SlotSizeUnits, InventoryData.Height * SlotSizeUnits);
	}
	private void DetachFromCurrentInventoryData() {
		InventoryData.OnInventoryUpdate -= UpdateInventory;
		InventoryData = null;
	}
	
	private void UpdateInventory(InventoryComponent caller) {
		if (caller != this) {
			Debug.Log($"Rebuilding inventory {gameObject.name}. Rebuilding caller was {caller.gameObject.name}.");
			CreateInvFromInventoryData(InventoryData);
		}
		OnInventoryChange.Invoke(InventoryData.Items);
	}
	
	private void OnDisable() {
		if (InventoryData == null) return;
		InventoryData.OnInventoryUpdate -= UpdateInventory;
	}

	private void FindReferences() {
		_rect = GetComponent<RectTransform>();
		if (_rect != null && _slotPrefab != null)
			_rect.sizeDelta = new Vector2(_assignedInventorySize.x * SlotSizeUnits,
				_assignedInventorySize.y * SlotSizeUnits);
		_colliderImage = GetComponent<Image>();
		_colliderImage.raycastTarget = true;
		_colliderImage.color = new Color(0, 0, 0, 0.1f);
	}

	/// <summary>
	/// Creates a new empty Inventory with the assigned inventory size in the inspector.
	/// </summary>
	public void CreateInventory() {
		CreateInvFromItemDatas(new List<ItemData>(), _assignedInventorySize);
	}
	
	/// <summary>
	/// Creates a new InventoryData of inventorySize, containing itemDatas. Packs the item instances into the inventory.
	/// </summary>
	/// <param name="itemDatas">The items to place in the new InventoryData</param>
	/// <param name="inventorySize">The size of the InventoryData to create.</param>
	// /// <returns>The number of items that could not be placed into the inventory.</returns>
	public int CreateInvFromItemDatas(List<ItemData> itemDatas, Vector2Int inventorySize) {
		AttachToInventoryData(new InventoryData(inventorySize.x, inventorySize.y));
		CreateAndAttachContainersTo(InventoryData);
		_inventoryItemPrefabInstances = new List<GameObject>();
		List<ItemInstance> items = new();
		foreach (ItemData itemData in itemDatas) {
			items.Add(itemData.CreateItem());
		}

		int notPlaced = AddItemsByData(itemDatas);
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
		if (InventoryData != null) {
			DetachFromCurrentInventoryData();
			DestroyInventoryContainersAndItems();
		}
		
		AttachToInventoryData(inventoryData);
		CreateAndAttachContainersTo(inventoryData);
		InstantiateInventoryItems(inventoryData);
	}

	private void Update() {
		if (InventoryData == null) return;
		ResetAllContainerHighlights();
	}

	public void CreateAndAttachContainersTo(InventoryData inventoryData) {
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
		foreach (ItemInstance item in inventoryData.Items) {
			CreateItemPrefab(item, inventoryData.GetSlotIndexOf(item));
		}
	}

	/// <summary>
	/// Only used by the editor script to destroy previewed inventory containers.
	/// </summary>
	public void DestroyInventoryContainersAndItems() {
		_slotPrefabInstances = null;
		_inventoryItemPrefabInstances.Clear();
		DeleteChildrenOf(_rect.transform);
	}

	/// <summary>
	/// Gets a list of all item instances that match the persistent ItemData.
	/// </summary>
	/// <param name="itemData">The ItemData to match against.</param>
	/// <returns>A list of InventoryItem that match the ItemData.</returns>
	public List<ItemInstance> GetItemsOfType(ItemData itemData) {
		return InventoryData.Items.FindAll(item => item.ItemData == itemData);
	}

	/// <summary>
	/// Get all the item instances in the inventory.
	/// </summary>
	/// <returns>A list of all the InventoryItems in the inventory.</returns>
	public List<ItemInstance> GetItems() {
		return InventoryData.Items;
	}
	
	/// <summary>
	/// Finds whether there are count instances or more of the item in the inventory.
	/// </summary>
	/// <param name="itemData">The ItemData to match against.</param>
	/// <param name="count">The count of instances.</param>
	/// <returns>True if there are count or more InventoryItems matching itemData.</returns>
	public bool ItemCountOrMoreInInventory(ItemData itemData, int count = 1) {
		List<ItemInstance> matchingItemInstances = InventoryData.Items.FindAll(item => item.ItemData == itemData);
		if (matchingItemInstances.Count < count) return false;
		return true;
	}
	public bool AddItem(ItemInstance itemInstance) {
		if (!InventoryData.TryAddItem(this, itemInstance, out Vector2Int slotIndexBL)) {
			return false;
		}
		CreateItemPrefab(itemInstance, slotIndexBL);
		return true;
	}
	/// <summary>
	/// Attempts to add count items to the inventory. If items are non-rectangular, this does not pack items very well.
	/// Use this when interacting with the inventory from non-inventory systems.
	/// </summary>
	/// <param name="itemData">The ItemData to instantiate InventoryItems with, and add to the inventory.</param>
	/// <param name="count">The count of InventoryItems to instantiate.</param>
	/// <returns>Whether adding all the items was successful.</returns>
	public bool AddItemByData(ItemData itemData) {
		ItemInstance itemInstance = itemData.CreateItem();
		if (!InventoryData.TryAddItem(this, itemInstance, out Vector2Int slotIndexBL)) {
			Debug.Log("Item dropped");
			SpawnDroppedItem(itemInstance);
			return false;
		}
		CreateItemPrefab(itemInstance, slotIndexBL);
		return true;
	}

	/// <summary>
	/// Tries to place the item with the bottom left at the slot position closest to positionSS.
	/// </summary>
	/// <param name="itemInstance">The InventoryItem to place.</param>
	/// <param name="positionSS">The position to place at in screen space.</param>
	/// <returns></returns>
	public bool PlaceItem(ItemInstance itemInstance, Vector2Int slotIndexBL) {
		if (!InventoryData.SlotIndexInBounds(slotIndexBL)) return false;
		GameObject cntrSlot = _slotPrefabInstances[slotIndexBL.x, slotIndexBL.y];
		InventoryContainerUI cntrSlotScript = cntrSlot.GetComponent<InventoryContainerUI>();
		Vector2Int index = cntrSlotScript.AttachedContainer.Index;
		bool couldPlace = InventoryData.TryAddItemAtPosition(this, itemInstance, index);
		if (couldPlace) {
			CreateItemPrefab(itemInstance, index);
			return true;
		}
		return false;
	}
	
	/// <summary>
	/// Attempts to add the items to the inventory. If items are non-rectangular, this does not pack items very well. Use this when interacting with the inventory from non-inventory systems.
	/// </summary>
	/// <param name="items">The ItemData to instantiate InventoryItems with, and add to the inventory.</param>
	/// <returns>The amount of items left over.</returns>
	public int AddItemsByData(List<ItemData> items) {
		int itemsPlaced = 0;
		if (items.Count == 0) return 0;
		foreach (ItemData item in items) {
			if (!AddItemByData(item)) {
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
	public bool RemoveItemByData(ItemData itemData, int count = 1) {
		List<GameObject> itemInstancesToRemove = new();
		foreach (GameObject itemUIInstance in _inventoryItemPrefabInstances) {
			InventoryItemUI itemUIScript = itemUIInstance.GetComponent<InventoryItemUI>();
			if (itemInstancesToRemove.Count == count) break;
			if (itemUIScript.ItemInstance.ItemData == itemData) itemInstancesToRemove.Add(itemUIInstance);
		}

		Debug.Log($"Found {itemInstancesToRemove.Count} {itemData.ItemName} in {gameObject.name}. Trying to remove {count}.");
		if (itemInstancesToRemove.Count != count) return false;
		// for future reference, if second term in the for loop == true, keep iterating
		for (int i = count - 1; i >= 0; i--) {
			InventoryItemUI itemUIScript = itemInstancesToRemove[i].GetComponent<InventoryItemUI>();
			_inventoryItemPrefabInstances.Remove(itemInstancesToRemove[i]);
			itemUIScript.DeleteSelfFromInventory();
		}
		return true;
	}

	public bool RemoveItem(ItemInstance item) {
		InventoryItemUI itemUIScript = _inventoryItemPrefabInstances.Find(itemUI => itemUI.GetComponent<InventoryItemUI>().ItemInstance == item).GetComponent<InventoryItemUI>();
		if (itemUIScript == null) return false;
		_inventoryItemPrefabInstances.Remove(itemUIScript.gameObject);
		itemUIScript.DeleteSelfFromInventory();
		return true;
	}
	
	public void ClearItems() {
		for (int i = _inventoryItemPrefabInstances.Count - 1; i >= 0; i--) {
			GameObject itemUIInstance = _inventoryItemPrefabInstances[i];
			InventoryItemUI itemUIScript = itemUIInstance.GetComponent<InventoryItemUI>();
			_inventoryItemPrefabInstances.Remove(itemUIInstance);
			itemUIScript.DeleteSelfFromInventory();
		}
	}

	private GameObject CreateItemPrefab(ItemInstance itemInstance, Vector2Int slotIndexBL) {
		GameObject itemPrefab = Instantiate(_inventoryItemPrefab, _rect.transform);
		itemPrefab.name = itemInstance.ItemData.ItemName + Random.Range(0, 100000);
		RectTransform itemRect = itemPrefab.GetComponent<RectTransform>();
		InventoryItemUI newItemUIScript = itemPrefab.GetComponent<InventoryItemUI>();
		newItemUIScript.InitializeWithItem(itemInstance, this);
		newItemUIScript.OnDestroyItem.AddListener(RemoveItemFromInstancesList);
		itemRect.anchoredPosition = new Vector2(slotIndexBL.x * SlotSizeUnits + SlotSizeUnits * itemInstance.Size.x / 2,
			slotIndexBL.y * SlotSizeUnits + SlotSizeUnits * itemInstance.Size.y / 2);
		itemRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, itemInstance.Size.x * SlotSizeUnits);
		itemRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, itemInstance.Size.y * SlotSizeUnits);
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

	public void SpawnDroppedItem(ItemInstance itemInstance) {
		GameObject dropped = Instantiate(itemInstance.ItemData.DroppedItemPrefab);
		Debug.LogWarning("Fix this localPlayer reference to reference using NetworkClient instead of GameObject.FindWithTag.");
		GameObject localPlayer = GameObject.FindWithTag("Player");
		dropped.transform.position = localPlayer.transform.position + Vector3.up * 2f;
		dropped.GetComponentInChildren<Rigidbody>().AddForce(Vector3.down * 0.5f + localPlayer.transform.forward * 2.5f + RandomJitter(0.1f), ForceMode.Impulse);
		dropped.GetComponentInChildren<Rigidbody>().AddTorque(localPlayer.transform.right * 0.5f + RandomJitter(0.1f), ForceMode.Impulse);
		dropped.GetComponent<DroppedItem>().Item = itemInstance;
	}

	private Vector3 RandomJitter(float jitterAmount) {
		return new Vector3(UnityEngine.Random.Range(-jitterAmount, jitterAmount), UnityEngine.Random.Range(-jitterAmount, jitterAmount), UnityEngine.Random.Range(-jitterAmount, jitterAmount));
	}
	


	public bool HighlightSlotsUnderItem(ItemInstance itemInstance, Vector2Int slotIndexBL) {
		List<InventoryContainer> containersItemOverlaps = new();
		bool couldPlace = true;
		for (int y = slotIndexBL.y; y < slotIndexBL.y + itemInstance.Size.y; y++) {
			for (int x = slotIndexBL.x; x < slotIndexBL.x + itemInstance.Size.x; x++) {
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
			if (containersItemOverlaps[i].HeldItemInstance != null) {
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