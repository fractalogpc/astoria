using UnityEngine;

public class ViewmodelItemData : ItemData
{
	[Header("Viewmodel Appearance")]
	public GameObject HeldItemPrefab;
	public RuntimeAnimatorController ItemAnimatorController;
	public AnimationClip EquipAnimation;
	public AnimationClip UnequipAnimation;
}