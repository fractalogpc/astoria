using System;
using System.Collections.Generic;
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
        get => _itemStacks.ToItemsList();
    }
    [SerializeField] private ItemStackList _itemStacks = new();
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
    /// <param name="itemSet">The item to add.</param>
    /// <param name="slotIndex">The slot index the item is placed at.</param>
    /// <returns>Whether the placement was successful.</returns>
    public bool TryAddItemSet(InventoryComponent caller, ItemSet itemSet, out Vector2Int slotIndex) {
        slotIndex = new Vector2Int(-1, -1);
        if (_itemStacks.List.Contains(itemSet)) {
            Debug.LogError("Exact same ItemInstance attempted to be added to inventory data. Check for logic errors in usages of TryAddItemSet.");    
            return false; // Item already in inventory, how did this happen?
        }
        Vector2Int bounds = itemSet.;
        for (int y = 0; y < Height; y++) {
            for (int x = 0; x < Width; x++) {
                // if (Containers[x, y].HeldItem != null) continue;
                // if (!CornersWithinGrid(new Vector2Int(x, y), bounds)) continue;
                // if (!IsNotOverlapping(new Vector2Int(x, y), bounds)) continue;
                slotIndex = new Vector2Int(x, y);
                
                bool successNormal;
                bool successRotated;
                
                // Try normally
                itemSet.Rotated = false;
                successNormal = TryAddItemAtPosition(caller, itemSet, new Vector2Int(x, y));
                if (successNormal) {
                    return true;
                }
                // Try Rotated
                itemSet.Rotated = true;
                successRotated = TryAddItemAtPosition(caller, itemSet, new Vector2Int(x, y));
                if (successRotated) {
                    return true;
                }
            }
        }
        return false;
    }
    /// <summary>
    /// Attempts to add an item to the inventory at a specific position. Calls OnInventoryUpdate if successful.
    /// </summary>
    /// <param name="itemInstance">The item to add.</param>
    /// <param name="slotIndexBL">The slotIndex to add the item at.</param>
    /// <returns>Whether adding the item was successful.</returns>
    public bool TryAddItemAtPosition(InventoryComponent caller, ItemInstance itemInstance, Vector2Int slotIndexBL) {
        if (Items.Contains(itemInstance)) return false;
        if (slotIndexBL.x < 0 || slotIndexBL.x >= Width || slotIndexBL.y < 0 || slotIndexBL.y >= Height) return false;
        Vector2Int bounds = itemInstance.Size;
        if (!CornersWithinGrid(slotIndexBL, bounds)) return false;
        for (int y = slotIndexBL.y; y < slotIndexBL.y + bounds.y; y++) {
            for (int x = slotIndexBL.x; x < slotIndexBL.x + bounds.x; x++) {
                Containers[x, y].HeldItemSetList = itemInstance;
            }
        }
        Items.Add(itemInstance);
        OnInventoryUpdate?.Invoke(caller);
        return true;
    }
    /// <summary>
    /// Finds if an item at slotIndex with size is within the inventory bounds.
    /// </summary>
    /// <param name="slotIndexBL">The slotIndex to test the bounds from.</param>
    /// <param name="size">The size of the bounds in slots.</param>
    /// <returns>Whether the item bounds are within the inventory.</returns>
    public bool CornersWithinGrid(Vector2Int slotIndexBL, Vector2Int size) {
        bool bottomLeftInGrid = slotIndexBL.x >= 0 && slotIndexBL.y >= 0;
        bool topRightInGrid = slotIndexBL.x + size.x <= Width && slotIndexBL.y + size.y <= Height;
        return bottomLeftInGrid && topRightInGrid;
    }
    public bool IsNotOverlapping(Vector2Int slotIndexBL, Vector2Int size) {
        for (int y = slotIndexBL.y; y < slotIndexBL.y + size.y; y++) {
            for (int x = slotIndexBL.x; x < slotIndexBL.x + size.x; x++) {
                if (Containers[x, y].HeldItemSetList != null) return false;
            }
        }
        return true;
    }
    /// <summary>
    /// Removes an item from the inventory. Calls OnInventoryUpdate if successful.
    /// </summary>
    /// <param name="itemInstance">The item instance to remove.</param>
    /// <returns>Whether the item was found and able to be removed.</returns>
    public bool RemoveItem(InventoryComponent caller, ItemInstance itemInstance) {
        if (!Items.Contains(itemInstance)) return false;
        Vector2Int bottomLeftContainerIndex = GetSlotIndexOf(itemInstance);
        Vector2Int bounds = itemInstance.Size;
        for (int y = bottomLeftContainerIndex.y; y < bottomLeftContainerIndex.y + bounds.y; y++) {
            for (int x = bottomLeftContainerIndex.x; x < bottomLeftContainerIndex.x + bounds.x; x++) {
                Containers[x, y].HeldItemSetList = null;
            }
        }
        Items.Remove(itemInstance);
        OnInventoryUpdate?.Invoke(caller);
        return true;
    }
    public Vector2Int GetSlotIndexOf(ItemInstance itemInstance) {
        for (int y = 0; y < Height; y++) {
            for (int x = 0; x < Width; x++) {
                if (Containers[x, y].HeldItemSetList == itemInstance) return new Vector2Int(x, y);
            }
        }
        return new Vector2Int(-1, -1);
    }
    public bool SlotIndexInBounds(Vector2Int slotIndex) {
        return slotIndex.x >= 0 && slotIndex.x < Width && slotIndex.y >= 0 && slotIndex.y < Height;
    }
    
    public bool UpdateHighlight(ItemInstance itemInstance, Vector2Int bottomLeftContainerIndex) {
        Vector2Int bounds = itemInstance.Size;
        // Not at all within grid
        if (!CornersWithinGrid(bottomLeftContainerIndex, Vector2Int.one)) {
            ClearHighlights();
            return false;
        }
        // Partly out of grid
        if (!CornersWithinGrid(bottomLeftContainerIndex, bounds)) {
            HighlightRed(bottomLeftContainerIndex, bounds);
            return false;
        }
        // Overlapping
        if (!IsNotOverlapping(bottomLeftContainerIndex, bounds)) {
            HighlightRed(bottomLeftContainerIndex, bounds);
            return false;
        }
        // All good
        HighlightGreen(bottomLeftContainerIndex, bounds);
        return true;
    }
    
    // These methods probably shouldn't iterate through the entire grid, but it's fine for now.
    private void HighlightRed(Vector2Int bottomLeftContainerIndex, Vector2Int bounds) {
        for (int y = bottomLeftContainerIndex.y; y < bottomLeftContainerIndex.y + bounds.y; y++) {
            for (int x = bottomLeftContainerIndex.x; x < bottomLeftContainerIndex.x + bounds.x; x++) {
                Containers[x, y].Highlight = ContainerHighlight.Red;
            }
        }
    }
    private void HighlightGreen(Vector2Int bottomLeftContainerIndex, Vector2Int bounds) {
        for (int y = bottomLeftContainerIndex.y; y < bottomLeftContainerIndex.y + bounds.y; y++) {
            for (int x = bottomLeftContainerIndex.x; x < bottomLeftContainerIndex.x + bounds.x; x++) {
                Containers[x, y].Highlight = ContainerHighlight.Green;
            }
        }
    }
    private void ClearHighlights() {
        for (int y = 0; y < Height; y++) {
            for (int x = 0; x < Width; x++) {
                Containers[x, y].Highlight = ContainerHighlight.None;
            }
        }
    }
}
