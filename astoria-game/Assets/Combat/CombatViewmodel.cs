using System;
using UnityEngine;

/// <summary>
/// This should be attached to every weapon viewmodel prefab at the top level. It handles animations and effects for the weapon.
/// Depends on CombatViewmodelAndLogicManager to call public functions
/// </summary>
public class CombatViewmodel : Viewmodel
{
    [Header("In the weapon animator, set the following triggers, without the Animation suffix:")]
    [SerializeField] private AnimationClip _fireAnimation;
    [SerializeField] private AnimationClip _reloadEmptyAnimation;
    [SerializeField] private AnimationClip _reloadPartialAnimation;

    /// <summary>
    /// Fires the trigger of the relevant animation of the attached animator.
    /// </summary>
    /// <returns>Duration of triggered animation in seconds.</returns>
    public float SetTriggerFire() {
        _animator.SetTrigger("Fire");
        return _fireAnimation.length;    
    }
    /// <summary>
    /// Fires the trigger of the relevant animation of the attached animator.
    /// </summary>
    /// <returns>Duration of triggered animation in seconds.</returns>
    public float SetTriggerReloadEmpty() {
        _animator.SetTrigger("ReloadEmpty");
        return _reloadEmptyAnimation.length;    
    }
    /// <summary>
    /// Fires the trigger of the relevant animation of the attached animator.
    /// </summary>
    /// <returns>Duration of triggered animation in seconds.</returns>
    public float SetTriggerReloadPartial() {
        _animator.SetTrigger("ReloadPartial");
        return _reloadPartialAnimation.length;    
    }
}
