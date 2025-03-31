using UnityEngine;

// This is a base class for all tools in the game. This shouldn't be used directly, but rather extended by a specific tool type.
public class BaseToolData : ViewmodelItemData
{
	[Header("Animations")]
	public AnimationClip UseAnimation;
	
	public override ItemInstance CreateItem() {
		return new BaseToolInstance(this);
	}
}
