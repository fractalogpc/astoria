using UnityEngine;

public class ToolViewmodel : Viewmodel
{
	[SerializeField] private AnimationClip _useAnimation;
	[SerializeField] private AnimationClip _useSecondaryAnimation;
	
	public float SetTriggerUse() {
		_animator.SetTrigger("Use");
		return _useAnimation.length;    	
	}
	public float SetTriggerUseSecondary() {
		_animator.SetTrigger("UseSecondary");
		return _useSecondaryAnimation.length;    	
	}
}
