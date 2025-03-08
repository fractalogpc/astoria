using UnityEngine;

public class Viewmodel : MonoBehaviour
{
	[SerializeField] protected Animator _animator;
	[SerializeField] protected AnimationClip _idleAnimation;
	[SerializeField] protected AnimationClip _walkAnimation;
	[SerializeField] protected AnimationClip _sprintAnimation;
	[SerializeField] private AnimationClip _equipAnimation;
	[SerializeField] private AnimationClip _unequipAnimation;
	
	private Vector2 inputVector;
	
	protected void Update() {
		// Debug.LogWarning("Problems with viewmodel animation during movement? Check this line, it uses hardcoded input.");
		inputVector.x = Input.GetAxisRaw("Horizontal");
		inputVector.y = Input.GetAxisRaw("Vertical");
		_animator.SetFloat("InputMagnitude", inputVector.magnitude);
		_animator.SetBool("SprintDown", Input.GetKey(KeyCode.LeftShift));
	}
	
	/// <summary>
	/// Fires the trigger of the relevant animation of the attached animator.
	/// </summary>
	/// <returns>Duration of triggered animation in seconds.</returns>
	public float SetTriggerUnequip() {
		_animator.SetTrigger("Unequip");
		return _unequipAnimation.length;    
	}
	/// <summary>
	/// Fires the trigger of the relevant animation of the attached animator.
	/// </summary>
	/// <returns>Duration of triggered animation in seconds.</returns>
	public float SetTriggerEquip() {
		_animator.SetTrigger("Equip");
		return _equipAnimation.length;    
	}
}