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
    
    public override void OnHotbarSelected() {
        base.OnHotbarSelected();
        _selected = true;
        _constructionCore.SelectData(ItemData.ConstructionData);
    }
    public override void OnHotbarDeselected() {
        base.OnHotbarDeselected();
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
        InventoryHotbar hotbar = PlayerInstance.Instance.GetComponentInChildren<InventoryHotbar>();
        hotbar.RemoveItem(this);
    }
}
