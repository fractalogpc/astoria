using System;
using Mirror;
using TMPro;
using TMPro.EditorUtilities;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

[RequireComponent(typeof(ClickableEvents))]
public class InventoryEquipableSlot : MonoBehaviour
{
    public UnityEvent OnSlotChanged;
    [ReadOnly] public ItemInstance _heldItemInstance;
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _itemText;
    [ReadOnly][SerializeField] private ClickableEvents _clickableEvents;
    [ReadOnly][SerializeField] private GameObject _draggablePrefab;
    private GameObject _draggedInstance;
    
    public virtual bool TryAddToSlot(ItemInstance itemInstance) {
        if (_heldItemInstance != null) {
            return false;
        }
        _heldItemInstance = itemInstance;
        _itemImage.sprite = itemInstance.ItemData.ItemIcon;
        _itemImage.color = Color.white;
        _itemText.text = itemInstance.ItemData.ItemName;
        OnSlotChanged.Invoke();
        return true;
    }
    
    public virtual void OnRemove() {
        if (_heldItemInstance == null) return;
        InstantiateDraggedItem();
        _itemImage.sprite = null;
        _itemImage.color = Color.clear;
        _itemText.text = "";
        _heldItemInstance = null;
    }
    
    private void GetReferences() {
        _clickableEvents = GetComponent<ClickableEvents>();
        if (_draggablePrefab == null) {
            Debug.LogError("WeaponEquipSlot: Draggable prefab is null! This should be assigned in the script default references.");
        }
    }

    private void OnValidate() {
        GetReferences();
    }

    private void Start() {
        GetReferences();
        _heldItemInstance = null;
        _itemImage.sprite = null;
        _itemImage.color = Color.clear;
        _itemText.text = "";
        _clickableEvents.OnClickDownSelected.AddListener(OnRemove);
        _clickableEvents.OnClickUpAnywhere.AddListener(OnClickUpAnywhere);
    }

    private void OnDisable() {
        _clickableEvents.OnClickDownSelected.RemoveListener(OnRemove);
        _clickableEvents.OnClickUpAnywhere.RemoveListener(OnClickUpAnywhere);
    }
    
    private void OnClickUpAnywhere() {
        if (_draggedInstance == null) return;
        _draggedInstance.GetComponent<InventoryItemDraggedUI>().OnLetGoOfDraggedItem();
        _draggedInstance = null;
    }
    
    private void InstantiateDraggedItem() {
        // Parented to whole canvas
        _draggedInstance = Instantiate(_draggablePrefab, transform.GetComponentInParent<Canvas>().transform);
        InventoryItemDraggedUI script = _draggedInstance.GetComponent<InventoryItemDraggedUI>();
        script.InitializeWithSlot(this, _heldItemInstance);
    }
}
