using System;
using Mirror;
using UnityEngine;

public class Harvestable : NetworkBehaviour
{
    public float MaxHealth => _maxHealth;
    public float CurrentHealth => _currentHealth;
    public bool IsDead => _currentHealth <= 0;
    public bool FinalSwingAvailable = true;
    
    [Tooltip("The amount a tool has to damage the harvestable to start the harvesting minigame.")]
    [SerializeField] private float _maxHealth;
    [Tooltip("The amount a tool has to damage the harvestable to start the harvesting minigame.")]
    [SerializeField][SyncVar][ReadOnly] private float _currentHealth;
    [Tooltip("The items that are given as a reward, regardless of the minigame outcome.")]
    [SerializeField] private LootTable _rewardItems; 
    [Tooltip("The items that are added to the reward if the minigame is completed successfully.")]
    [SerializeField] private LootTable _additionalItems;
    
    /// <summary>
    /// Damages the harvestable by the given amount.
    /// </summary>
    /// <param name="damagePoints">The amount to damage by.</param>
    /// <param name="hitPosition">The position the hit came from. Useful for FX and polish.</param>
    /// <returns>Whether this damage causes the harvestable health to go below 0.</returns>
    // [Server]
    public virtual bool Damage(float damagePoints, Vector3 hitPosition) {
        _currentHealth -= damagePoints;
        if (_currentHealth <= 0) {
            _currentHealth = -1;
            return true;
        }
        return false;
    }
    
    protected virtual void Start() {
        _currentHealth = _maxHealth;
        if (_rewardItems == null) {
            Debug.LogError("Harvestable " + name + " has no reward items set.", gameObject);
        }
        if (_additionalItems == null) {
            Debug.LogError("Harvestable " + name + " has no additional items set. This will cause a successful minigame to give no extra reward.", gameObject);
        }
    }
}
