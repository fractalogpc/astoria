using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Image = UnityEngine.UI.Image;

[RequireComponent(typeof(ClickableEvents))]
public class InventoryEquipableSlot : MonoBehaviour
{
    [ReadOnly] public ItemInstance _heldItemInstance;
    [SerializeField] protected Image _itemImage;
    [SerializeField] protected TextMeshProUGUI _itemText;
    [ReadOnly][SerializeField] protected ClickableEvents _clickableEvents;
    [ReadOnly][SerializeField] protected GameObject _draggablePrefab;
    protected GameObject _draggedInstance;
    public UnityEvent<ItemInstance> OnItemAdded;
    public UnityEvent<ItemInstance> OnItemRemoved;
    
    public virtual bool TryAddToSlot(ItemInstance itemInstance) {
        if (_heldItemInstance != null) {
            return false;
        }
        _heldItemInstance = itemInstance;
        _itemImage.sprite = itemInstance.ItemData.ItemIcon;
        _itemImage.type = Image.Type.Simple;
        _itemImage.preserveAspect = true;
        _itemImage.color = Color.white;
        _itemText.text = itemInstance.ItemData.ItemName;
        OnItemAdded.Invoke(itemInstance);
        return true;
    }
    
    public virtual void OnRemove() {
        if (_heldItemInstance == null) return;
        InstantiateDraggedItem(_heldItemInstance);
        _itemImage.sprite = null;
        _itemImage.color = Color.clear;
        _itemText.text = "";
        OnItemRemoved.Invoke(_heldItemInstance);
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
    
    protected void InstantiateDraggedItem(ItemInstance itemInstance) {
        // Parented to whole canvas
        _draggedInstance = Instantiate(_draggablePrefab, transform.GetComponentInParent<Canvas>().transform);
        InventoryItemDraggedUI script = _draggedInstance.GetComponent<InventoryItemDraggedUI>();
        script.InitializeWithSlot(this, itemInstance);
    }
}
