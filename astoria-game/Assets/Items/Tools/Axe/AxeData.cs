using UnityEngine;

[CreateAssetMenu(fileName = "New Axe", menuName = "Scriptable Objects/AxeData")]
public class AxeData : BaseToolData
{
	[Header("Animations")]
	public AnimationClip DownChopAnimation;
	
	[Header("Axe Properties")]
	// TODO: Make this a random range
	[Tooltip("The value subtracted from the tree's health when chopping.")]
	public int ChopDamage = 50;
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
