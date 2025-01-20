using UnityEngine;

[CreateAssetMenu(fileName = "New Pickaxe", menuName = "Scriptable Objects/PickaxeData")]
public class PickaxeData : BaseToolData
{
	// TODO: Make this a random range
	[Tooltip("The value subtracted from the rocks's health when swinging.")]
	public int SwingDamage = 50;
	[Tooltip("Time in seconds axe cannot be used after a side chop is started.")]
	public float SwingCooldown = 1f;
	[Tooltip("The range in meters the pickaxe can harvest rocks and minerals.")]
	public float SwingRange = 4f;
	
	public override ItemInstance CreateItem() {
		return new PickaxeInstance(this);
	}
}
