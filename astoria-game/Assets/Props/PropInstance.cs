using Mirror;
using UnityEngine;

public class PropInstance : ItemInstance
{
    public new PropData ItemData { get; private set; }
    public PropInstance(PropData propData) : base(propData)
    {
        ItemData = propData;
        _constructionCore = NetworkClient.localPlayer.gameObject.GetComponentInChildren<NEWConstructionCore>();
    }
    
    private NEWConstructionCore _constructionCore;
    
    public override void OnSelected() {
        Debug.Log("Called construction core Selected prop instance");
        _constructionCore.SelectData(ItemData.ConstructionData);
    }
    public override void OnDeselected() {
        base.OnDeselected();
        _constructionCore.DeselectData();
    }
}
