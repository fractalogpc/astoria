using UnityEngine;

[CreateAssetMenu(fileName = "New Build Tool", menuName = "Scriptable Objects/BuildTool")]
public class BuildToolData : BaseToolData
{
	public override ItemInstance CreateItem() {
		return new BuildToolInstance(this);
	}
}
