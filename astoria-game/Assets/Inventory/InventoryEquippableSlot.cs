using System;
using Mirror;
using TMPro.EditorUtilities;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[RequireComponent(typeof(ClickableEvents))]
public class InventoryEquippableSlot : MonoBehaviour
{
    public UnityEvent OnSlotChanged;
    [ReadOnly][SerializeField] private ClickableEvents _clickableEvents;
    [ReadOnly][SerializeField] private GameObject _draggablePrefab;
    private InventoryItem _heldItem;

    private void Start() {
        _clickableEvents.OnClickDownSelected.AddListener(OnClicked);
    }

    private void OnDisable() {
        throw new NotImplementedException();
    }

    private void OnClicked() {
        throw new NotImplementedException();
    }

    public bool TryAddToSlot(InventoryItem item) {
        if (_heldItem != null) {
            return false;
        }
        _heldItem = item;
        return true;
    }
    
    
    
    
}
