using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

/// <summary>
/// Attach this script to RectTransforms to create Inventory UIs.
/// </summary>
[AddComponentMenu("Inventory/Inventory Component")]
// [RequireComponent(typeof(AutoRegister))]
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Image))]
public class InventoryComponent : MonoBehaviour
{
	public bool Initialized => InventoryData != null;

	public Vector2Int AssignedInventorySize => _assignedInventorySize;
	public float SlotSizeUnits => _slotPrefab.GetComponent<RectTransform>().sizeDelta.x;

	/// <summary>
	/// Includes a list of the InventoryItems in the inventory.
	/// </summary>
	public UnityEvent<List<ItemInstance>> OnInventoryChange;

	public InventoryData InventoryData { get; private set; }

	[Header("Ensure that the object this is placed on is a direct child of an Overlay Canvas that has a Graphic Raycaster.")]
	[Header("Ensure Slot Prefab is Square, and has InventoryContainerUI component.")]
	[SerializeField]
	private GameObject _inventoryItemPrefab;

	[SerializeField] private GameObject _slotPrefab;

	[Header("If outside scripts initialize the inventory, don't use this.")]
	[SerializeField] private bool _useAssignedInventoryData;

	[SerializeField] private Vector2Int _assignedInventorySize;
	[SerializeField] private List<ItemData> _spawnInventoryWith;

	private List<GameObject> _stackPrefabInstances = new();
	private RectTransform _rect;
	private Image _colliderImage; // Need a collider image so hovered items can raycast and see the inventory
	private GameObject[,] _slotPrefabInstances;

	private void OnValidate() {
		FindReferences();
	}

	public void Awake() {
		FindReferences();
	}

	public void Start() {
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
		_stackPrefabInstances = new List<GameObject>();
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

	/// <summary>
	/// Only used by the editor script to destroy previewed inventory containers.
	/// </summary>
	public void DestroyInventoryContainersAndItems() {
		_slotPrefabInstances = null;
		_stackPrefabInstances.Clear();
		DeleteChildrenOf(_rect.transform);
	}

	/// <summary>
	/// Gets a list of all item instances that match the persistent ItemData.
	/// </summary>
	/// <param name="itemData">The ItemData to match against.</param>
	/// <returns>A list of InventoryItem that match the ItemData.</returns>
	public List<ItemInstance> GetItemsOfType(ItemData itemData) {
		if (!Initialized) {
			Debug.LogError("InventoryComponent: Inventory not initialized! Cannot add item. Check for initialization race conditions.", gameObject);
			return new List<ItemInstance>();
		}

		return InventoryData.Items.FindAll(item => item.ItemData == itemData);
	}

	/// <summary>
	/// Get all the item instances in the inventory.
	/// </summary>
	/// <returns>A list of all the InventoryItems in the inventory.</returns>
	public List<ItemInstance> GetItems() {
		if (!Initialized) {
			Debug.LogError("InventoryComponent: Inventory not initialized! Cannot add item. Check for initialization race conditions.", gameObject);
			return new List<ItemInstance>();
		}

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
	
	public bool AddStack(ItemStack itemStack) {
		if (!InventoryData.TryAddStack(this, itemStack, out Vector2Int slotIndexBL)) return false;
		CreateInvFromInventoryData(InventoryData);
		return true;
	}
	public bool AddItem(ItemInstance itemInstance) {
		if (!InventoryData.TryAddStack(this, new ItemStack(itemInstance), out Vector2Int slotIndexBL)) {
			SpawnDroppedItem(new ItemStack(itemInstance));
			return false;
		}
		CreateInvFromInventoryData(InventoryData);
		return true;
	}

	/// <summary>
	/// Attempts to add count items to the inventory. If items are non-rectangular, this does not pack items very well.
	/// Use this when interacting with the inventory from non-inventory systems.
	/// </summary>
	/// <param name="itemData">The ItemData to instantiate InventoryItems with, and add to the inventory.</param>
	/// <param name="count">The count of InventoryItems to instantiate.</param>
	/// <returns>Whether adding all the items was successful.</returns>
	public bool AddItemByData(ItemData itemData, int count = 1) {
		if (!Initialized) {
			Debug.LogError("InventoryComponent: Inventory not initialized! Cannot add item. Check for initialization race conditions.", gameObject);
			return false;
		}
		for (int i = 0; i < count; i++) {
			AddItem(itemData.CreateItem());
		}
		return true;
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
	/// Tries to place the item with the bottom left at the slot position closest to positionSS.
	/// </summary>
	/// <param name="itemStack">The InventoryItem to place.</param>
	/// <param name="slotIndexBL">The bottom left slot index to place at.</param>
	/// <returns></returns>
	public bool PlaceStack(ItemStack itemStack, Vector2Int slotIndexBL) {
		if (!InventoryData.TryAddStackAtPosition(this, itemStack, slotIndexBL)) return false;
		CreateInvFromInventoryData(InventoryData);
		return true;
	}
	public bool RemoveStack(ItemStack itemStack) {
		InventoryData.RemoveStack(this, itemStack);
		CreateInvFromInventoryData(InventoryData);
		return true;
	}
	
	
	// TODO: Ensure public functions after this point properly CreateInvFromInventoryData after modifying the InventoryData.
	
	
	/// <summary>
	/// Tries to remove count items from the inventory that match the ItemData.
	/// </summary>
	/// <param name="itemData">The ItemData to check against.</param>
	/// <param name="count">The count of items to remove.</param>
	/// <returns>Whether the count of matching items could be removed.</returns>
	public bool RemoveItemByData(ItemData itemData, int count = 1) {
		List<ItemInstance> itemsToRemove = new();
		foreach (ItemInstance item in InventoryData.Items) {
			if (itemsToRemove.Count == count) break;
			if (item.ItemData == itemData) itemsToRemove.Add(item);
		}
		Debug.Log($"Found {itemsToRemove.Count} {itemData.ItemName} in {gameObject.name}. Trying to remove {count}.");
		if (itemsToRemove.Count != count) return false;
		// for future reference, if second term in the for loop == true, keep iterating
		for (int i = itemsToRemove.Count - 1; i >= 0; i--) {
			ItemInstance itemToRemove = itemsToRemove[i];
			itemsToRemove.RemoveAt(i);
			RemoveItem(itemToRemove);
		}
		return true;
	}

	public bool PopFrom(ItemStack itemStack, out ItemInstance item, out bool hasItemsLeft) {
		hasItemsLeft = true;
		item = null;
		if (!InventoryData.Stacks.Contains(itemStack)) return false;
		InventoryData.PopItemFrom(this, itemStack, out ItemInstance poppedItem, out hasItemsLeft);
		item = poppedItem;
		return true;
	}
	
	public bool RemoveItem(ItemInstance item) {
		if (!InventoryData.Items.Contains(item)) return false;
		InventoryData.RemoveItem(this, item);
		return true;
	}

	public void ClearItems() {
		for (int i = InventoryData.Items.Count - 1; i >= 0; i--) {
			RemoveItem(InventoryData.Items[i]);
		}
	}
	
	public Vector2Int GetSlotIndexOf(ItemStack itemStack) {
		return InventoryData.GetSlotIndexOf(itemStack);
	}

	public void SpawnDroppedItem(ItemStack itemStack) {
		GameObject dropped = Instantiate(itemStack.StackType.DroppedItemPrefab);
		Debug.LogWarning("Fix this localPlayer reference to reference using NetworkClient instead of GameObject.FindWithTag.");
		GameObject localPlayer = GameObject.FindWithTag("Player");
		dropped.transform.position = localPlayer.transform.position + Vector3.up * 2f;
		dropped.GetComponentInChildren<Rigidbody>().AddForce(Vector3.down * 0.5f + localPlayer.transform.forward * 2.5f + RandomJitter(0.1f), ForceMode.Impulse);
		dropped.GetComponentInChildren<Rigidbody>().AddTorque(localPlayer.transform.right * 0.5f + RandomJitter(0.1f), ForceMode.Impulse);
		dropped.GetComponent<DroppedItem>().ItemStack = itemStack;
	}
	
	public bool HighlightSlotsUnderStack(ItemStack itemStack, Vector2Int slotIndexBL) {
		List<InventoryContainer> containersItemOverlaps = new();
		bool couldPlace = true;
		for (int y = slotIndexBL.y; y < slotIndexBL.y + itemStack.Size.y; y++) {
			for (int x = slotIndexBL.x; x < slotIndexBL.x + itemStack.Size.x; x++) {
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
			if (containersItemOverlaps[i].HeldStack != null) {
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
	
	private GameObject CreateItemPrefab(ItemStack itemStack, Vector2Int slotIndexBL) {
		GameObject itemPrefab = Instantiate(_inventoryItemPrefab, _rect.transform);
		itemPrefab.name = itemStack.StackType.ItemName + Random.Range(0, 100000); // Doesn't actually need to be unique, just enough to identify during debugging
		RectTransform itemRect = itemPrefab.GetComponent<RectTransform>();
		InventoryStackUI newStackUIScript = itemPrefab.GetComponent<InventoryStackUI>();
		newStackUIScript.InitializeWithStack(this, itemStack, slotIndexBL, SlotSizeUnits);
		// print($"Item {item.ItemData.ItemName} placed at {slotIndexBL} in {gameObject.name}. Size: {itemRect.rect.size}. Position: {itemRect.anchoredPosition}");
		_stackPrefabInstances.Add(itemPrefab);
		// Debug.Log($"ItemUI Created with name {itemPrefab.name}, in inventory {gameObject.transform.parent.name}. Clickable events null: {itemPrefab.GetComponent<ClickableEvents>() == null}");
		return itemPrefab;
	}
	
	private void Update() {
		if (InventoryData == null) return;
		ResetAllContainerHighlights();
	}
	
	private void DeleteChildrenOf(Transform parent) {
		for (int i = parent.childCount - 1; i >= 0; i--) {
			Destroy(parent.GetChild(i).gameObject);
		}
	}
	
	private Vector3 RandomJitter(float jitterAmount) {
		return new Vector3(Random.Range(-jitterAmount, jitterAmount), Random.Range(-jitterAmount, jitterAmount), Random.Range(-jitterAmount, jitterAmount));
	}
	
	private void InstantiateInventoryItems(InventoryData inventoryData) {
		ItemStackList stacks = inventoryData.Stacks;
		for (int i = 0; i < stacks.StackCount; i++) {
			CreateItemPrefab(stacks[i], inventoryData.GetSlotIndexOf(stacks[i]));
		}
	}
	private void HighlightContainers(List<InventoryContainer> containers, ContainerHighlight highlight) {
		ResetAllContainerHighlights();
		for (int i = 0; i < containers.Count; i++) {
			containers[i].Highlight = highlight;
		}
	}
}