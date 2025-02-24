using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Image = UnityEngine.UI.Image;

[RequireComponent(typeof(ClickableEvents))]
public class InventoryEquipableSlot : MonoBehaviour
{
    [ReadOnly] public ItemInstance HeldItem;
    [SerializeField] protected Image _itemImage;
    [SerializeField] protected TextMeshProUGUI _itemText;
    [ReadOnly][SerializeField] protected ClickableEvents _clickableEvents;
    [ReadOnly][SerializeField] protected GameObject _draggablePrefab;
    protected GameObject _draggedInstance;
    public UnityEvent<ItemInstance> OnItemAdded;
    public UnityEvent<ItemInstance> OnItemRemoved;
    
    /// <summary>
    /// Puts an item into the slot, if an item isn't in there already. Invokes OnItemAdded.
    /// </summary>
    /// <param name="itemInstance">The item instance to put into the slot.</param>
    /// <returns>Whether the item was successfully added into the slot.</returns>
    public virtual bool TryAddToSlot(ItemInstance itemInstance) {
        if (HeldItem != null) {
            return false;
        }
        HeldItem = itemInstance;
        _itemImage.sprite = itemInstance.ItemData.ItemIcon;
        _itemImage.type = Image.Type.Simple;
        _itemImage.preserveAspect = true;
        _itemImage.color = Color.white;
        _itemText.text = itemInstance.ItemData.ItemName;
        OnItemAdded.Invoke(itemInstance);
        return true;
    }
    
    public virtual void OnPickup() {
        if (HeldItem == null) return;
        InstantiateDraggedItem(HeldItem);
        _itemImage.sprite = null;
        _itemImage.color = Color.clear;
        _itemText.text = "";
        OnItemRemoved.Invoke(HeldItem);
        HeldItem = null;
    }
    
    /// <summary>
    /// Removes the item from the slot. Invokes OnItemRemoved and calls the item's OnItemDestruction method.
    /// </summary>
    public virtual void RemoveItem() {
        if (HeldItem == null) return;
        _itemImage.sprite = null;
        _itemImage.color = Color.clear;
        _itemText.text = "";
        OnItemRemoved.Invoke(HeldItem);
        HeldItem.OnItemDestruction();
        HeldItem = null;
    }
    
    private void GetReferences() {
        _clickableEvents = GetComponent<ClickableEvents>();
        if (_draggablePrefab == null) {
            Debug.LogError("WeaponEquipSlot: Draggable prefab is null! This should be assigned in the script default references.", gameObject);
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
        _clickableEvents.OnClickDownSelected.AddListener(OnPickup);
        _clickableEvents.OnClickUpAnywhere.AddListener(OnClickUpAnywhere);
    }

    private void OnDisable() {
        _clickableEvents.OnClickDownSelected.RemoveListener(OnPickup);
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
        script.InitializeFromSlot(this, itemInstance);
    }
}
