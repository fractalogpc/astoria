using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Image = UnityEngine.UI.Image;

[RequireComponent(typeof(ClickableEvents))]
public class InventoryEquipableSlot : MonoBehaviour
{
    public UnityEvent OnSlotChanged;
    [ReadOnly] public InventoryItem HeldItem;
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _itemText;
    [ReadOnly][SerializeField] private ClickableEvents _clickableEvents;
    [ReadOnly][SerializeField] private GameObject _draggablePrefab;
    private GameObject _draggedInstance;
    
    public virtual bool TryAddToSlot(InventoryItem item) {
        if (HeldItem != null) {
            return false;
        }
        HeldItem = item;
        _itemImage.sprite = item.ItemData.ItemIcon;
        _itemImage.color = Color.white;
        _itemText.text = item.ItemData.ItemName;
        OnSlotChanged.Invoke();
        return true;
    }
    
    public virtual void OnRemove() {
        if (HeldItem == null) return;
        InstantiateDraggedItem();
        _itemImage.sprite = null;
        _itemImage.color = Color.clear;
        _itemText.text = "";
        HeldItem = null;
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
        HeldItem = null;
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
        script.InitializeWithSlot(this, HeldItem);
    }
}
