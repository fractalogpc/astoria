using UnityEngine;

public class Viewmodel : MonoBehaviour
{
	[SerializeField] protected Animator _animator;
	[SerializeField] protected AnimationClip _idleAnimation;
	[SerializeField] protected AnimationClip _walkAnimation;
	[SerializeField] protected AnimationClip _sprintAnimation;
	
	private Vector2 inputVector;
	
	protected void Update() {
		Debug.Log("Problems with viewmodel animation during movement? Check this line, it uses hardcoded input.");
		inputVector.x = Input.GetAxisRaw("Horizontal");
		inputVector.y = Input.GetAxisRaw("Vertical");
		_animator.SetFloat("InputMagnitude", inputVector.magnitude);
		_animator.SetBool("SprintDown", Input.GetKey(KeyCode.LeftShift));
	}
}