using System;
using UnityEngine;

/// <summary>
/// This should be attached to every weapon viewmodel prefab at the top level. It handles animations and effects for the weapon.
/// Depends on CombatViewmodelAndLogicManager to call public functions
/// </summary>
public class CombatViewmodel : MonoBehaviour
{
    public WeaponInstance WeaponInstance;
    
    [SerializeField] private Animator _animator;
    [SerializeField] private AnimationClip _idleAnimation;
    [SerializeField] private AnimationClip _walkAnimation;
    [SerializeField] private AnimationClip _sprintAnimation;
    [Header("In the weapon animator, set the following triggers, without the Animation suffix:")]
    [SerializeField] private AnimationClip _drawAnimation;
    [SerializeField] private AnimationClip _fireAnimation;
    [SerializeField] private AnimationClip _reloadEmptyAnimation;
    [SerializeField] private AnimationClip _reloadPartialAnimation;
    [SerializeField] private AnimationClip _holsterAnimation;

    private float x;
    private float y;
    private Vector2 inputVector;
    
    private void Update() {
        Debug.Log("Problems with viewmodel animation during movement? Check this line, it uses hardcoded input.");
        inputVector.x = Input.GetAxisRaw("Horizontal");
        inputVector.y = Input.GetAxisRaw("Vertical");
        _animator.SetFloat("InputMagnitude", inputVector.magnitude);
        _animator.SetBool("SprintDown", Input.GetKey(KeyCode.LeftShift));
    }

    /// <summary>
    /// Fires the trigger of the relevant animation of the attached animator.
    /// </summary>
    /// <returns>Duration of triggered animation in seconds.</returns>
    public float SetTriggerFire() {
        _animator.SetTrigger("Fire");
        return 0.1f;    
    }
    /// <summary>
    /// Fires the trigger of the relevant animation of the attached animator.
    /// </summary>
    /// <returns>Duration of triggered animation in seconds.</returns>
    public float SetTriggerReloadEmpty() {
        _animator.SetTrigger("ReloadEmpty");
        return 0.1f;    
    }
    /// <summary>
    /// Fires the trigger of the relevant animation of the attached animator.
    /// </summary>
    /// <returns>Duration of triggered animation in seconds.</returns>
    public float SetTriggerReloadPartial() {
        _animator.SetTrigger("ReloadPartial");
        return 0.1f;    
    }
    /// <summary>
    /// Fires the trigger of the relevant animation of the attached animator.
    /// </summary>
    /// <returns>Duration of triggered animation in seconds.</returns>
    public float SetTriggerHolster() {
        _animator.SetTrigger("Holster");
        return 0.1f;    
    }
    /// <summary>
    /// Fires the trigger of the relevant animation of the attached animator.
    /// </summary>
    /// <returns>Duration of triggered animation in seconds.</returns>
    public float SetTriggerDraw() {
        _animator.SetTrigger("Draw");
        return 0.1f;    
    }
}
