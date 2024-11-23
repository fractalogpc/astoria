using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This represents an Inventory. An Inventory is defined as a grid of slots with a specific size.
/// Grid starts at bottom left corner. (For similarity to graph coordinates)
/// X - Left To Right
/// Y - Bottom To Top
/// </summary>
[Serializable]
public class Inventory
{
    public List<InventoryItem> Items {
        get => _items;
        private set => _items = value;
    }
    [SerializeField] private List<InventoryItem> _items = new List<InventoryItem>();
    public InventoryContainer[,] Containers { get; }

    public int Width => Containers.GetLength(0);
    public int Height => Containers.GetLength(1);
    
    public Inventory(int width, int height) {
        Containers = new InventoryContainer[width, height];
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                Containers[x, y] = new InventoryContainer(new Vector2Int(x, y));
            }
        }
    }
    public bool UpdateHighlight(InventoryItem item, Vector2Int bottomLeftContainerIndex) {
        Vector2Int bounds = item.Size;
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
    public bool TryAddItem(InventoryItem item, out Vector2Int slotIndex) {
        slotIndex = new Vector2Int(-1, -1);
        if (Items.Contains(item)) return false;
        Vector2Int bounds = item.Size;
        for (int y = 0; y < Height; y++) {
            for (int x = 0; x < Width; x++) {
                if (Containers[x, y].HeldItem != null) continue;
                if (!CornersWithinGrid(new Vector2Int(x, y), bounds)) continue;
                if (!IsNotOverlapping(new Vector2Int(x, y), bounds)) continue;
                slotIndex = new Vector2Int(x, y);
                return TryAddItemAtPosition(item, new Vector2Int(x, y));
            }
        }
        return false;
    }
    public bool TryAddItemAtPosition(InventoryItem item, Vector2Int slotIndexBL) {
        if (Items.Contains(item)) return false;
        if (slotIndexBL.x < 0 || slotIndexBL.x >= Width || slotIndexBL.y < 0 || slotIndexBL.y >= Height) return false;
        Vector2Int bounds = item.Size;
        if (!CornersWithinGrid(slotIndexBL, bounds) || !IsNotOverlapping(slotIndexBL, bounds)) return false;
        for (int y = slotIndexBL.y; y < slotIndexBL.y + bounds.y; y++) {
            for (int x = slotIndexBL.x; x < slotIndexBL.x + bounds.x; x++) {
                Containers[x, y].HeldItem = item;
            }
        }
        Items.Add(item);
        return true;
    }
    public bool CornersWithinGrid(Vector2Int slotIndexBL, Vector2Int size) {
        bool bottomLeftInGrid = slotIndexBL.x >= 0 && slotIndexBL.y >= 0;
        bool topRightInGrid = slotIndexBL.x + size.x <= Width && slotIndexBL.y + size.y <= Height;
        return bottomLeftInGrid && topRightInGrid;
    }
    public bool IsNotOverlapping(Vector2Int slotIndexBL, Vector2Int size) {
        for (int y = slotIndexBL.y; y < slotIndexBL.y + size.y; y++) {
            for (int x = slotIndexBL.x; x < slotIndexBL.x + size.x; x++) {
                if (Containers[x, y].HeldItem != null) return false;
            }
        }
        return true;
    }
    public bool RemoveItem(InventoryItem item) {
        if (!Items.Contains(item)) return false;
        Vector2Int bottomLeftContainerIndex = GetBLContainerIndexOf(item);
        Vector2Int bounds = item.Size;
        for (int y = bottomLeftContainerIndex.y; y < bottomLeftContainerIndex.y + bounds.y; y++) {
            for (int x = bottomLeftContainerIndex.x; x < bottomLeftContainerIndex.x + bounds.x; x++) {
                Containers[x, y].HeldItem = null;
            }
        }
        Items.Remove(item);
        return true;
    }
    public Vector2Int GetBLContainerIndexOf(InventoryItem item) {
        for (int y = 0; y < Height; y++) {
            for (int x = 0; x < Width; x++) {
                if (Containers[x, y].HeldItem == item) return new Vector2Int(x, y);
            }
        }
        return new Vector2Int(-1, -1);
    }
}
