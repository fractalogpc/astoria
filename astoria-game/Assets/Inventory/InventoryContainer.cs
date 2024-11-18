using System;
using UnityEngine;

[Serializable]
public class InventoryContainer
{ 
    public InventoryContainer(Vector2Int index) {
        Index = index;
        HeldItem = null;
        Highlight = ContainerHighlight.None;
    }
    
    public delegate void ContainerUpdated();
    public event ContainerUpdated OnContainerUpdated;
    
    public Vector2Int Index { get; }
    
    private InventoryItem _heldItem;
    public InventoryItem HeldItem {
        get => _heldItem;
        set {
            _heldItem = value;
            OnContainerUpdated?.Invoke();
        }
    }
    
    private ContainerHighlight _highlight;
    public ContainerHighlight Highlight {
        get => _highlight;
        set {
            _highlight = value;
            OnContainerUpdated?.Invoke();
        }
    }
}

public enum ContainerHighlight
{
    None,
    Green,
    Red
}