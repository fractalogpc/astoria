using UnityEngine;

public class CombatCameraAnimator : MonoBehaviour
{
	[SerializeField] private Animator _cameraAnimator;
	[SerializeField] private Animation _idleAnimation;
	[SerializeField] private Animation _walkAnimation;
	[SerializeField] private Animation _sprintAnimation;
	[Header("In the weapon animator, set the following triggers, without the Animation suffix:")]
	[SerializeField] private Animation _drawAnimation;
	[SerializeField] private Animation _fireAnimation;
	[SerializeField] private Animation _reloadEmptyAnimation;
	[SerializeField] private Animation _reloadPartialAnimation;
	[SerializeField] private Animation _holsterAnimation;
	
	private float x;
    private float y;
    private Vector2 inputVector;
    
    private void Update() {
        Debug.Log("Problems with camera movement based animation? Check this line, it uses hardcoded input.");
        inputVector.x = Input.GetAxisRaw("Horizontal");
        inputVector.y = Input.GetAxisRaw("Vertical");
        _cameraAnimator.SetFloat("InputMagnitude", inputVector.magnitude);
        _cameraAnimator.SetBool("SprintDown", Input.GetKeyDown(KeyCode.LeftShift));
    }

    /// <summary>
    /// Fires the trigger of the relevant animation of the attached animator.
    /// </summary>
    /// <returns>Duration of triggered animation in seconds.</returns>
    public float SetTriggerFire() {
        _cameraAnimator.SetTrigger("Fire");
        return 0.1f;    
    }
    /// <summary>
    /// Fires the trigger of the relevant animation of the attached animator.
    /// </summary>
    /// <returns>Duration of triggered animation in seconds.</returns>
    public float SetTriggerReloadEmpty() {
        _cameraAnimator.SetTrigger("ReloadEmpty");
        return 0.1f;    
    }
    /// <summary>
    /// Fires the trigger of the relevant animation of the attached animator.
    /// </summary>
    /// <returns>Duration of triggered animation in seconds.</returns>
    public float SetTriggerReloadPartial() {
        _cameraAnimator.SetTrigger("ReloadPartial");
        return 0.1f;    
    }
    /// <summary>
    /// Fires the trigger of the relevant animation of the attached animator.
    /// </summary>
    /// <returns>Duration of triggered animation in seconds.</returns>
    public float SetTriggerHolster() {
        _cameraAnimator.SetTrigger("Holster");
        return 0.1f;    
    }
    /// <summary>
    /// Fires the trigger of the relevant animation of the attached animator.
    /// </summary>
    /// <returns>Duration of triggered animation in seconds.</returns>
    public float SetTriggerDraw() {
        _cameraAnimator.SetTrigger("Draw");
        return 0.1f;    
    }
}