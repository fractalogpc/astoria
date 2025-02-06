using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

public class Harvestable : NetworkBehaviour
{
    HarvestState CurrentState { get; set; }

    [Tooltip("The amount a harvester has to damage the harvestable to start the harvesting minigame.")]
    [SerializeField] private float _damageLimit;
    [SerializeField][SyncVar][ReadOnly] private float _currentDamage;
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
    public virtual bool Hit(HarvesterData harvester, Vector3 hitPosition) {
        
    }
    
    protected virtual void Start() {
        _currentDamage = 0;
        if (_rewardItems == null) {
            Debug.LogError("Harvestable " + name + " has no reward items set.", gameObject);
        }
        if (_additionalItems == null) {
            Debug.LogError("Harvestable " + name + " has no additional items set. This will cause a successful minigame to give no extra reward.", gameObject);
        }
    }
}

public class HarvestState
{
    public bool InMinigame {get; private set;}
    
    // Progress Bar
    public float CurrentValue;
    public float MaxValue;
    
    // Minigame Bar
    public float CritPosition {get; private set;}
    public float CritWidth {get; private set;}
	public float MarkerPositionPercent {get; private set;}
    
    public HarvestState(Harvestable harvestable, float critWidthPercent)
    {
        CurrentValue = harvestable.CurrentHealth;
        MaxValue = harvestable.MaxHealth;
        CritWidth = critWidthPercent;
        float rangeHigh = 100 - (critWidthPercent / 2);
        float rangeLow = critWidthPercent / 2;
        CritPosition = Random.Range(rangeLow, rangeHigh);
    }

    public void UpdateState(Harvestable harvestable, float critWidthPercent) {
        CurrentValue = harvestable.CurrentHealth;
        MaxValue = harvestable.MaxHealth;
        CritWidth = critWidthPercent;
        float rangeHigh = 100 - (critWidthPercent / 2);
        float rangeLow = critWidthPercent / 2;
        CritPosition = Random.Range(rangeLow, rangeHigh);
    }
}
