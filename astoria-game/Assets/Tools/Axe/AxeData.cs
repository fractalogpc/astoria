using UnityEngine;

[CreateAssetMenu(fileName = "New Axe", menuName = "Scriptable Objects/AxeData")]
public class AxeData : BaseToolData
{
	public override ItemInstance CreateItem() {
		return new AxeInstance(this);
	}
}
