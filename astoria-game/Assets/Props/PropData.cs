using UnityEngine;


[CreateAssetMenu(fileName = "New Prop", menuName = "Scriptable Objects/PropData")]
public class PropData : ItemData
{
    public ConstructionData ConstructionData;

    public override ItemInstance CreateItem() {
        return new PropInstance(this);
    }
}
