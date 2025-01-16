using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Fires when the health is initialized. The parameter is the max health.
/// </summary>
[Serializable]
public class OnHealthInitializeEvent : UnityEvent<float> {}
/// <summary>
/// Fires when the health is changed. The parameters are the health before the damage, the current health, and the max health.
/// </summary>
[Serializable]
public class OnHealthChangedEvent : UnityEvent<float, float, float> {}
/// <summary>
/// Fires at the end of the damage calculation. The parameter is the hit position.
/// </summary>
[Serializable]
public class OnDamageEvent : UnityEvent<Vector3> {}
/// <summary>
/// Fires when the health is healed.
/// </summary>
[Serializable]
public class OnHealEvent : UnityEvent {}
/// <summary>
/// Fires when the health reaches zero.
/// </summary>
[Serializable]
public class OnHealthZeroEvent : UnityEvent {}
public class HealthInterface : NetworkBehaviour
{

  public float CurrentHealth => _currentHealth;
  public float MaxHealth => _maxHealth;
  public bool IsDead => _currentHealth <= 0;
  
  [Tooltip("OnHealthInitialize(_maxHealth)")] public OnHealthInitializeEvent OnHealthInitialize;
  [Tooltip("OnHealthZero()")] public OnHealthZeroEvent OnHealthZero;
  [Tooltip("OnHealthChanged(healthBeforeDamage, _currentHealth, _maxHealth) Doesn't fire when health changes to a value below zero.")]
  public OnHealthChangedEvent OnHealthChanged;
  [Tooltip("OnDamaged(hitPosition)")] public OnDamageEvent OnDamaged;
  [Tooltip("OnHeal()")] public OnHealEvent OnHeal;
  
  [SerializeField] private float _currentHealth;
  [SerializeField] private float _maxHealth;
  [Tooltip("If true, the health will not be initialized in Start().")]
  [SerializeField] private bool _initializeWithScript = true;

  private float _regenTimer = 0;
  
  public void Initialize(float maxHealth) {
    _maxHealth = maxHealth;
    _currentHealth = maxHealth;
    OnHealthInitialize?.Invoke(maxHealth);
  }
  
  public void Heal(float healPoints) {
    OnHeal?.Invoke();
    float initialHealth = _currentHealth;
    if (_currentHealth + healPoints >= GetMaxHealth()) _currentHealth = GetMaxHealth();
    else _currentHealth += healPoints;
    OnHealthChanged?.Invoke(initialHealth, _currentHealth, GetMaxHealth());
  }

  public void Damage(float damagePoints, Vector3 hitPosition) {
    if (_currentHealth - damagePoints <= 0) {
      OnHealthZero?.Invoke();
    }
    else {
      float initialHealth = _currentHealth;
      _currentHealth -= damagePoints;
      _regenTimer = 0;
      OnHealthChanged?.Invoke(initialHealth, _currentHealth, GetMaxHealth());
    }
    OnDamaged?.Invoke(hitPosition);
  }
  
  protected override void OnValidate() {
    base.OnValidate();
    _currentHealth = _maxHealth;
  }
  
  private void Start() {
    if (!_initializeWithScript) return;
    Initialize(_maxHealth);
  }
  
  private void Update() {

  }

  private float GetMaxHealth() {
      return _maxHealth;
  }
}