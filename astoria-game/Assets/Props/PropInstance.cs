using Construction;

public class PropInstance : ItemInstance
{
    public new PropData ItemData { get; private set; }
    public PropInstance(PropData propData) : base(propData)
    {
        ItemData = propData;
        _constructionCore = PlayerInstance.Instance.gameObject.GetComponentInChildren<ConstructionCore>();
        _constructionCore.OnObjectPlaced.AddListener(OnConstructionCorePlace);
    }
    
    public ConstructionCore _constructionCore;
    private bool _selected = false;
    private InventoryHotbarSlot _hotbarSlot = null;
    
    public override void OnHotbarSelected(InventoryHotbarSlot hotbarSlot) {
        base.OnHotbarSelected(hotbarSlot);
        _hotbarSlot = hotbarSlot;
        _selected = true;
        _constructionCore.SelectData(ItemData.ConstructionData);
    }
    public override void OnHotbarDeselected(InventoryHotbarSlot hotbarSlot) {
        base.OnHotbarDeselected(hotbarSlot);
        _hotbarSlot = null;
        _selected = false;
        _constructionCore.SetConstructionState(ConstructionCore.ConstructionState.None);
    }

    public override void OnItemDestruction() {
        base.OnItemDestruction();
        // _constructionCore.DeselectData();
        _constructionCore.OnObjectPlaced.RemoveListener(OnConstructionCorePlace);
    }

    private void OnConstructionCorePlace(ConstructionData constructionData) {
        if (!_selected || !constructionData == ItemData.ConstructionData) return;
        _hotbarSlot.Deselect();
        _hotbarSlot.AttachedSlot.RemoveItem();
    }
}
