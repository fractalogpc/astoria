using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This represents an Inventory. An Inventory is defined as a grid of slots with a specific size.
/// Grid starts at bottom left corner. (For similarity to graph coordinates)
/// X - Left To Right
/// Y - Bottom To Top
/// </summary>
[AddComponentMenu("")]
public class InventoryData
{
    public List<ItemInstance> Items {
        get => _stacks.ToItemsList();
    }
    public List<ItemData> Datas {
        get => _stacks.ToDatasList();
    }
    public ItemStackList Stacks {
        get => _stacks;
    }
    [SerializeField] private ItemStackList _stacks = new();
    public InventoryContainer[,] Containers { get; }

    public int Width => Containers.GetLength(0);
    public int Height => Containers.GetLength(1);

    public delegate void InventoryUpdate(InventoryComponent caller);
    public event InventoryUpdate OnInventoryUpdate;
    
    public InventoryData(int width, int height) {
        Containers = new InventoryContainer[width, height];
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                Containers[x, y] = new InventoryContainer(new Vector2Int(x, y));
            }
        }
    }
    
    /// <summary>
    /// Adds an item to the inventory if possible. Attempts to find a position for the item. Calls OnInventoryUpdate if successful.
    /// </summary>
    /// <param name="caller">The inventory component that is calling this method.</param>
    /// <param name="itemStack">The item to add.</param>
    /// <param name="slotIndex">The slot index the item is placed at.</param>
    /// <returns>Whether the placement was successful.</returns>
    public bool TryAddStack(InventoryComponent caller, ItemStack itemStack, out Vector2Int slotIndex) {
        slotIndex = new Vector2Int(-1, -1);
        if (_stacks.Contains(itemStack)) return false;
        for (int y = 0; y < Height; y++) {
            for (int x = 0; x < Width; x++) {
                slotIndex = new Vector2Int(x, y);
                
                bool successNormal;
                bool successRotated;
                
                // Try normally
                itemStack.Rotated = false;
                successNormal = TryAddStackAtPosition(caller, itemStack, new Vector2Int(x, y));
                if (successNormal) {
                    return true;
                }
                // Try Rotated
                itemStack.Rotated = true;
                successRotated = TryAddStackAtPosition(caller, itemStack, new Vector2Int(x, y));
                if (successRotated) {
                    return true;
                }
            }
        }
        slotIndex = new Vector2Int(-1, -1);
        return false;
    }
    
    /// <summary>
    /// Attempts to add an item to the inventory at a specific position. Calls OnInventoryUpdate if successful.
    /// </summary>
    /// <param name="itemStack">The item to add.</param>
    /// <param name="slotIndexBL">The slotIndex to add the item at.</param>
    /// <returns>Whether adding the item was successful.</returns>
    public bool TryAddStackAtPosition(InventoryComponent caller, ItemStack itemStack, Vector2Int slotIndexBL) {
        if (_stacks.Contains(itemStack)) return false;
        if (!SlotIndexInBounds(slotIndexBL)) return false;
        Vector2Int bounds = itemStack.Size;
        if (!CornersWithinGrid(slotIndexBL, bounds)) return false;
        if (OverlappingNothing(slotIndexBL, bounds)) {
            UpdateHeldStack(slotIndexBL, bounds, itemStack);
            _stacks.Add(itemStack);
            OnInventoryUpdate?.Invoke(caller);
            return true;
        }
        if (OverlappingStackAndCanAdd(slotIndexBL, bounds, itemStack)) {
            ItemStack stack = Containers[slotIndexBL.x, slotIndexBL.y].HeldStack;
            stack.Push(itemStack.Items);
            OnInventoryUpdate?.Invoke(caller);
            return true;
        }
        return false;
    }

    public bool CouldAddStackAtPosition(ItemStack itemStack, Vector2Int slotIndexBL) {
        if (_stacks.Contains(itemStack)) return false;
        if (!SlotIndexInBounds(slotIndexBL)) return false;
        Vector2Int bounds = itemStack.Size;
        if (!CornersWithinGrid(slotIndexBL, bounds)) return false;
        if (OverlappingNothing(slotIndexBL, bounds)) {
            return true;
        }
        if (OverlappingStackAndCanAdd(slotIndexBL, bounds, itemStack)) {
            return true;
        }
        return false;
    }
    /// <summary>
    /// Removes an item from the inventory. Calls OnInventoryUpdate if successful.
    /// </summary>
    /// <param name="itemInstance">The item instance to remove.</param>
    /// <returns>Whether the item was found and able to be removed.</returns>
    public bool RemoveItem(InventoryComponent caller, ItemInstance itemInstance) {
        if (!Items.Contains(itemInstance)) return false;
        Vector2Int indexBL = SlotIndexOf(itemInstance);
        if (!Containers[indexBL.x, indexBL.y].HeldStack.Remove(itemInstance)) {
            Debug.LogError($"InventoryData: Failed to remove item instance {itemInstance.ItemData.ItemName} from inventory.");
            return false;
        }
        if (Containers[indexBL.x, indexBL.y].HeldStack.StackCount <= 0) {
            _stacks.Remove(Containers[indexBL.x, indexBL.y].HeldStack);
            UpdateHeldStack(indexBL, Vector2Int.one, null);
        }
        // Debug.Log("Invoking OnInventoryUpdate in RemoveItem of InventoryData");
        OnInventoryUpdate?.Invoke(caller);
        return true;
    }
    /// <summary>
    /// Removes a specified amount of items from the inventory. Calls OnInventoryUpdate if successful.
    /// </summary>
    /// <param name="itemData">The item data type to remove.</param>
    /// <param name="count">The amount of items to remove.</param>
    /// <returns>Whether the amount of items was found and able to be removed.</returns>
    public bool RemoveItems(InventoryComponent caller, ItemData itemData, int count) {
        if (_stacks.CountOf(itemData) < count) return false;
        for (int i = 0; i < count; i++) {
            Vector2Int indexBL = SlotIndexOf(itemData);
            if (!Containers[indexBL.x, indexBL.y].HeldStack.Pop(out _)) {
                Debug.LogError($"InventoryData: Failed to remove item of data {itemData.ItemName} from inventory.");
                return false;
            }
            if (Containers[indexBL.x, indexBL.y].HeldStack.StackCount > 0) continue;
            _stacks.Remove(Containers[indexBL.x, indexBL.y].HeldStack);
            UpdateHeldStack(indexBL, Vector2Int.one, null);
        }
        OnInventoryUpdate?.Invoke(caller);
        return true;
    }
    /// <summary>
    /// Removes a specified amount of items from the inventory. Calls OnInventoryUpdate if successful.
    /// </summary>
    /// <param name="items">The item datas to remove.</param>
    /// <returns>Whether the amount of items was found and able to be removed.</returns>
    public bool RemoveItems(InventoryComponent caller, List<ItemData> items) {
        if (items.Any(data => !Datas.Contains(data))) {
            return false;
        }
        foreach (ItemData itemData in items) {
            Vector2Int indexBL = SlotIndexOf(itemData);
            ItemStack holdingStack = Containers[indexBL.x, indexBL.y].HeldStack;
            // Removed last item
            if (holdingStack.Pop(out _)) continue;
            _stacks.Remove(holdingStack);
            UpdateHeldStack(indexBL, holdingStack.Size, null);
        }
        OnInventoryUpdate?.Invoke(caller);
        return true;
    }
    public bool RemoveStack(InventoryComponent caller, ItemStack stack) {
        if (!_stacks.Contains(stack)) return false;
        Vector2Int indexBL = SlotIndexOf(stack);
        _stacks.Remove(stack);
        UpdateHeldStack(indexBL, stack.Size, null);
        OnInventoryUpdate?.Invoke(caller);
        return true;
    }

    public bool PopItemFrom(InventoryComponent caller, ItemStack itemStack, out ItemInstance item, out bool itemsLeft) {
        itemsLeft = true;
        if (!_stacks.Contains(itemStack)) {
            item = null;
            return false;
        }
        if (!itemStack.Pop(out ItemInstance poppedItem)) {
            Vector2Int indexBL = SlotIndexOf(itemStack);
            _stacks.Remove(itemStack);
            UpdateHeldStack(indexBL, Vector2Int.one, null);
            itemsLeft = false;
        }
        item = poppedItem;
        _stacks.Remove(itemStack);
        OnInventoryUpdate?.Invoke(caller);
        return true;
    }
    /// <summary>
    /// Gets the first index where this stack is found, going from left to right, bottom to top.
    /// </summary>
    /// <param name="itemStack">The stack to search for.</param>
    /// <returns>Whether the stack was found.</returns>
    public Vector2Int SlotIndexOf(ItemStack itemStack) {
        for (int y = 0; y < Height; y++) {
            for (int x = 0; x < Width; x++) {
                if (Containers[x, y].HeldStack == null) continue;
                if (Containers[x, y].HeldStack == itemStack) return new Vector2Int(x, y);
            }
        }
        return new Vector2Int(-1, -1);
    }
    /// <summary>
    /// Gets the first index where this item is found, going from left to right, bottom to top.
    /// </summary>
    /// <param name="itemInstance">The item to search for.</param>
    /// <returns>Whether the item was found.</returns>
    public Vector2Int SlotIndexOf(ItemInstance itemInstance) {
        for (int y = 0; y < Height; y++) {
            for (int x = 0; x < Width; x++) {
                InventoryContainer container = Containers[x, y];
                if (container.HeldStack == null) continue;
                if (container.HeldStack.Contains(itemInstance)) return new Vector2Int(x, y);
            }
        }
        return new Vector2Int(-1, -1);
    }
    /// <summary>
    /// Gets the first index where this item data is found, going from left to right, bottom to top.
    /// </summary>
    /// <param name="itemData">The item data to search for.</param>
    /// <returns>The slot index where the item data is found.</returns>
    public Vector2Int SlotIndexOf(ItemData itemData) {
        for (int y = 0; y < Height; y++) {
            for (int x = 0; x < Width; x++) {
                if (Containers[x, y].HeldStack == null) continue;
                if (Containers[x, y].HeldStack.StackType == itemData) return new Vector2Int(x, y);
            }
        }
        return new Vector2Int(-1, -1);
    }
    public bool SlotIndexInBounds(Vector2Int slotIndex) {
        return slotIndex.x >= 0 && slotIndex.x < Width && slotIndex.y >= 0 && slotIndex.y < Height;
    }
    private void UpdateHeldStack(Vector2Int slotIndexBL, Vector2Int size, ItemStack itemStack) {
        for (int y = slotIndexBL.y; y < slotIndexBL.y + size.y; y++) {
            for (int x = slotIndexBL.x; x < slotIndexBL.x + size.x; x++) {
                Containers[x, y].HeldStack = itemStack;
            }
        }
    }
    /// <summary>
    /// Finds if an item at slotIndex with size is within the inventory bounds.
    /// </summary>
    /// <param name="slotIndexBL">The slotIndex to test the bounds from.</param>
    /// <param name="size">The size of the bounds in slots.</param>
    /// <returns>Whether the item bounds are within the inventory.</returns>
    private bool CornersWithinGrid(Vector2Int slotIndexBL, Vector2Int size) {
        bool bottomLeftInGrid = slotIndexBL.x >= 0 && slotIndexBL.y >= 0;
        bool topRightInGrid = slotIndexBL.x + size.x <= Width && slotIndexBL.y + size.y <= Height;
        return bottomLeftInGrid && topRightInGrid;
    }
    private bool OverlappingNothing(Vector2Int slotIndexBL, Vector2Int size) {
        for (int y = slotIndexBL.y; y < slotIndexBL.y + size.y; y++) {
            for (int x = slotIndexBL.x; x < slotIndexBL.x + size.x; x++) {
                if (Containers[x, y].HeldStack != null) return false;
            }
        }
        return true;
    }
    private bool OverlappingStackAndCanAdd(Vector2Int slotIndexBL, Vector2Int size, ItemStack stack) {
        ItemStack itemStack = Containers[slotIndexBL.x, slotIndexBL.y].HeldStack;
        if (itemStack == null) return false;
        if (!itemStack.CouldPush(stack)) return false;
        for (int y = slotIndexBL.y; y < slotIndexBL.y + size.y; y++) {
            for (int x = slotIndexBL.x; x < slotIndexBL.x + size.x; x++) {
                if (Containers[x, y].HeldStack != itemStack) return false;
            }
        }
        return true;
    }
}
