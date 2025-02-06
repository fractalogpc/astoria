using UnityEngine;

[CreateAssetMenu(fileName = "New Axe", menuName = "Scriptable Objects/AxeData")]
public class AxeData : HarvesterData
{
	[Tooltip("Time in seconds axe cannot be used after a side chop is started.")]
	public float SideChopCooldown = 1f;
	[Tooltip("Time in seconds axe cannot be used after a down chop is started.")]
	public float DownChopCooldown = 1f;
	[Tooltip("The range in meters the axe can chop trees.")]
	public float ChopRange = 4f;
	
	public override ItemInstance CreateItem() {
		return new AxeInstance(this);
	}
}
