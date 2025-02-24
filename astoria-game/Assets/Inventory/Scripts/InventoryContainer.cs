using System;
using UnityEngine;

[Serializable]
public class InventoryContainer
{ 
    public InventoryContainer(Vector2Int index) {
        Index = index;
        HeldItemSetList = null;
        Highlight = ContainerHighlight.None;
    }
    
    public delegate void ContainerUpdated();
    public event ContainerUpdated OnContainerUpdated;
    
    public Vector2Int Index { get; }
    
    private ItemSetList _heldItemSet;
    public ItemSetList HeldItemSetList {
        get => _heldItemSet;
        set {
            _heldItemSet = value;
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