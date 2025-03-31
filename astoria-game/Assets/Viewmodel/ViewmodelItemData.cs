using UnityEngine;

public class ViewmodelItemData : ItemData
{
	[Header("Viewmodel Appearance")]
	[Tooltip("The prefab that will be instantiated in the viewmodel's hand when the item is equipped.")]
	public GameObject HeldItemPrefab;
	[Tooltip("The animator controller for the viewmodel. Ensure that any AnimationClip variable names have a corresponding trigger. Ex. The presence of VMEquipAnimation and VMUnequipAnimation requires 'Equip' and 'Unequip' triggers.")]
	public RuntimeAnimatorController ViewmodelAnimatorController;
	[Tooltip("(OPTIONAL) The animator controller override for the HeldItemPrefab. If the prefab has animations, create an override of the ViewmodelAnimatorController and assign the prefab's animations in it.")]
	public AnimatorOverrideController ItemAnimations;
	[Tooltip("The equip animation the viewmodel will play. Ensure this anim matches the associated state in the animator.")]	
	public AnimationClip VMEquipAnimation;
	[Tooltip("The unequip animation the viewmodel will play. Ensure this anim matches the associated state in the animator.")]	
	public AnimationClip VMUnequipAnimation;
}