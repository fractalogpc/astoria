using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHotbarSlot : MonoBehaviour
{
    public InventoryEquipableSlot AttachedSlot => _attachedSlot;
    public bool Selected { get; private set; }
    [SerializeField][ReadOnly] private InventoryHotbar _hotbar;
    [SerializeField] private Image _itemImage;
    [SerializeField] private HighlightEffect _highlightEffect;
    
    private InventoryEquipableSlot _attachedSlot;

    public bool Initialize(InventoryHotbar hotbar) {
        if (_hotbar != null) {
            return false;
        }
        _hotbar = hotbar;
        return true;
    }
    
    public void Select() {
        Selected = true;
        _highlightEffect.FadeIn();
    }

    public void Deselect() {
        Selected = false;
        _highlightEffect.FadeOut();
    }
    
    public void AttachSlot(InventoryEquipableSlot slot) {
        if (_attachedSlot != null) {
            _attachedSlot.OnItemAdded.RemoveListener(UpdateSlotState);
            _attachedSlot.OnItemRemoved.RemoveListener(UpdateSlotState);
        }
        _attachedSlot = slot;
        _attachedSlot.OnItemAdded.AddListener(UpdateSlotState);
        _attachedSlot.OnItemRemoved.AddListener(UpdateSlotState);
        UpdateSlotState(_attachedSlot._heldItemInstance);
    }
    
    private void UpdateSlotState(ItemInstance item) {
        if (_attachedSlot._heldItemInstance == null) {
            _itemImage.sprite = null;
            _itemImage.color = Color.clear;
            _highlightEffect.FadeOut();
            return;
        }
        _itemImage.color = Color.white;
        _itemImage.sprite = _attachedSlot._heldItemInstance.ItemData.ItemIcon;
    }
    
    
    
    
}
