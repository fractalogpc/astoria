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
// public class HealthInterface : NetworkBehaviour
public class HealthManager : PlayerVitalHandler, IDamageable
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

  private float _regenTimer = 0;
  
  public override float GetCurrentValue() {
    return _currentHealth;
  }
  public override float GetMaxValue() {
    return _maxHealth;
  }
  
  public virtual void Initialize(float maxHealth) {
    if (maxHealth <= 0) {
      Debug.LogWarning("Max health is zero or less. Is this intended?");
    }
    _maxHealth = maxHealth;
    _currentHealth = maxHealth;
    OnHealthInitialize?.Invoke(maxHealth);
  }
  
  public virtual void Heal(float healPoints) {
    OnHeal?.Invoke();
    float initialHealth = _currentHealth;
    if (_currentHealth + healPoints >= MaxHealth) _currentHealth = MaxHealth;
    else _currentHealth += healPoints;
    NotifyVitalChange(initialHealth, _currentHealth, MaxHealth);
  }

  public void SetHealthDirect(float health) {
    _currentHealth = health;
    NotifyVitalChange(health, health, MaxHealth);
  }

  public void TakeDamage(float damage, Vector3 hitPosition) {
    Damage(damage, hitPosition);
  }

  public virtual void Damage(float damagePoints, Vector3 hitPosition) {
    if (_currentHealth - damagePoints <= 0) {
      _currentHealth = 0;
      OnHealthZero?.Invoke();
    }
    else {
      float initialHealth = _currentHealth;
      _currentHealth -= damagePoints;
      _regenTimer = 0;
      NotifyVitalChange(initialHealth, _currentHealth, MaxHealth);
    }
    OnDamaged?.Invoke(hitPosition);
  }
  
  protected virtual void OnValidate() {
    _currentHealth = _maxHealth;
  }
  
  protected virtual void Start() {
    Initialize(_maxHealth);
  }
}