using UnityEngine;

[CreateAssetMenu(fileName = "New Pickaxe", menuName = "Scriptable Objects/Items/Tools/PickaxeData")]
public class PickaxeData : BaseToolData
{
	// TODO: Make this a random range
	[Tooltip("The value subtracted from the rocks's health when swinging.")]
	public Vector2 SwingDamage = new Vector2(10f, 20f);
	[Tooltip("Time in seconds axe cannot be used after a side chop is started.")]
	public float SwingCooldown = 1f;
	public float AnimationSwingDelay = 0.5f;
	[Tooltip("The range in meters the pickaxe can harvest rocks and minerals.")]
	public float SwingRange = 4f;
	
	public override ItemInstance CreateItem() {
		return new PickaxeInstance(this);
	}
}
