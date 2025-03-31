using UnityEngine;

[CreateAssetMenu(fileName = "Multitool", menuName = "Scriptable Objects/Items/Tools/MultitoolData")]
public class MultitoolData : BaseToolData
{
	public override ItemInstance CreateItem() {
		return new MultitoolInstance(this);
	}
}
