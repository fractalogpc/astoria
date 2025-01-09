using UnityEngine;

public class PropInstance : ItemInstance
{
    // Inject some prop placer dependency that will actually place the prop, the item instance will call the placement when it detects a click?
    
    // private PropPlacer _propPlacer;
    public new PropData ItemData { get; private set; }
    public PropInstance(PropData propData) : base(propData)
    {
        ItemData = propData;
    }
    
    
}
