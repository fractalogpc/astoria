using Mirror;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Anything that requires health and can take damage should have this component.
/// If you run into issues with damaging, make sure that the damage is applied on the server.
/// </summary>
[RequireComponent(typeof(NetworkIdentity))]
public class NetworkedHealth : NetworkBehaviour
{
    [SyncVar(hook = nameof(HookHealthChanged))] private int _health = 100;
    [SerializeField] private int _maxHealth = 100;

    public int Health => _health;
    public int MaxHealth => _maxHealth;

    /// <summary>
    /// Fires when the health changes. The first parameter is the old health, the second is the new health.
    /// </summary>
    public UnityEvent<int, int> OnHealthChanged;
    /// <summary>
    /// Fires instantly after the health reaches or surpasses 0.
    /// </summary>
    public UnityEvent OnDeath;
    
    public void TakeDamage(int damage) {
        if (!isServer) {
            Debug.LogWarning("Client cannot apply damage! Make sure that all Damage calls are done on the server!");
            return;
        }
        _health -= damage;
        if (_health <= 0) {
            _health = 0;
            Die();
        }
    }

    private void Die() {
        Debug.Log("Dead!");
        OnDeath?.Invoke();
    }

    private void HookHealthChanged(int oldHealth, int newHealth) {
        Debug.Log($"Health changed from {oldHealth} to {newHealth}");
        OnHealthChanged?.Invoke(oldHealth, newHealth);
    }
}
