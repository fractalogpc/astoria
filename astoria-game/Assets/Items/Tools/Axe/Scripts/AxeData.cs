using FMODUnity;
using UnityEngine;

[CreateAssetMenu(fileName = "New Axe", menuName = "Scriptable Objects/Items/Tools/AxeData")]
public class AxeData : BaseToolData
{
	[Header("Axe Properties")]
	// TODO: Make this a random range
	[Tooltip("The value subtracted from the tree's health when chopping.")]
	public Vector2 ChopDamage = new Vector2(10f, 20f);
	[Tooltip("The normalized time in the swing animation when the axe hits the tree.")]
	[Range(0, 1)] public float AnimChopMoment;
	[Tooltip("Time in seconds axe cannot be used after a side chop is started.")]
	public float SideChopCooldown = 1f;
	[Tooltip("The range in meters the axe can chop trees.")]
	public float ChopRange = 4f;
	[Tooltip("The prefab instantiated when the axe hits a tree. Used to add a hit decal to the tree.")]
	public GameObject WoodHitDecalPrefab;
	public GameObject WoodHitParticles;
	[Tooltip("The sound that plays when the axe is swung.")]
	public EventReference SwingSound;
	public float SwingSoundDelay;
	[Tooltip("The sound that plays when the axe hits.")]
	public EventReference HitSound;
	
	public override ItemInstance CreateItem() {
		return new AxeInstance(this);
	}
}
