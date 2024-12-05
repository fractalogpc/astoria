using System;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Handles visual effects for the player when they take damage or are low on health.
/// Requires CombatHealth.
/// </summary>
public class LowHealthFX : MonoBehaviour
{
    private CombatHealth _health;
    [SerializeField] private Volume _lowHealthEffects;
    [Header("Effect Strength over percentage of health gone")]
    [SerializeField] private AnimationCurve _lowHealthStartCurve;
    
    private void Start() {
        if (_health == null) {
            _health = GameObject.FindWithTag("Player").GetComponent<CombatHealth>();
        }
        _health.OnHealthChanged.AddListener(OnHealthChanged);
    }

    private void OnHealthChanged(int oldHealth, int newHealth) {
        _lowHealthEffects.weight = _lowHealthStartCurve.Evaluate(1 - (float)newHealth / _health.MaxHealth);
    }

    private void OnDisable() {
        _health.OnHealthChanged.RemoveListener(OnHealthChanged);
    }
}