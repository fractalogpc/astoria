using System;
using UnityEngine;

[Serializable]
public class InventoryContainer
{ 
    public InventoryContainer(Vector2Int index) {
        Index = index;
        HeldStack = null;
        Highlight = ContainerHighlight.None;
    }
    
    public delegate void ContainerUpdated();
    public event ContainerUpdated OnContainerUpdated;
    
    public Vector2Int Index { get; }
    
    private ItemStack _heldStack;
    public ItemStack HeldStack {
        get => _heldStack;
        set {
            _heldStack = value;
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