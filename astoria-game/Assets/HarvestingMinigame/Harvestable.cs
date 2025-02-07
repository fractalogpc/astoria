using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;
public class HarvestableEvent : UnityEvent<Harvestable> {}
public class Harvestable : NetworkBehaviour
{
    
    public HarvestableEvent OnHit;
    public HarvestableEvent OnHarvested;
    
    public float CritMarkerPositionPercent { get; private set; }
    public float CritPositionPercent { get; private set; }
    
    [Tooltip("The amount a harvester has to damage the harvestable to start the harvesting minigame.")]
    [SerializeField] private float _damageLimit;
    [SerializeField][SyncVar][ReadOnly] private float _currentDamage;
    [Tooltip("The items that are given as a reward, regardless of the minigame outcome.")]
    [SerializeField] private LootTable _rewardItems; 
    [Tooltip("The items that are added to the reward if the minigame is completed successfully.")]
    [SerializeField] private LootTable _additionalItems;

    public virtual void Focus(HarvesterData harvester) {
        
    }
    
    /// <summary>
    /// Damages the harvestable by the value in the HarvesterData of the CurrentHarvester in HarvestingManager.
    /// </summary>
    /// <param name="hitPosition">The position the hit came from. Useful for FX and polish.</param>
    // [Server]
    public virtual void Hit(Vector3 hitPosition) {
        if (_currentDamage >= _damageLimit) {
            HarvestedEffects(hitPosition);
            OnHarvested?.Invoke(this);
            return;
        }
        OnHit.Invoke(this);
        _currentDamage += HarvestingManager.Instance.CurrentHarvester.HarvestDamage;
    }
    
    protected virtual void Start() {
        HarvestingManager.Instance.RegisterHarvestable(this);
        _currentDamage = 0;
        if (_rewardItems == null) {
            Debug.LogError("Harvestable " + name + " has no reward items set.", gameObject);
        }
        if (_additionalItems == null) {
            Debug.LogError("Harvestable " + name + " has no additional items set. This will cause a successful minigame to give no extra reward.", gameObject);
        }
    }

    protected virtual void HarvestedEffects(Vector3 hitPosition) {
        
    }
}

// public class HarvestState
// {
//     public Harvestable HarvestableObject { get; private set; }
//     public bool InMinigame { get; private set; }
//     
//     // Progress Bar
//     public float CurrentValue;
//     public float MaxValue;
//     
//     // Minigame Bar
//     public float CritPosition {get; private set;}
//     public float CritWidth {get; private set;}
// 	public float MarkerPositionPercent {get; private set;}
//     
//     public HarvestState(Harvestable harvestable, float critWidthPercent)
//     {
//         HarvestableObject = harvestable;
//         CurrentValue = HarvestableObject.;
//         MaxValue = harvestable.;
//         CritWidth = critWidthPercent;
//         float rangeHigh = 100 - (critWidthPercent / 2);
//         float rangeLow = critWidthPercent / 2;
//         CritPosition = Random.Range(rangeLow, rangeHigh);
//     }
//
//     public void UpdateState(Harvestable harvestable, float critWidthPercent) {
//         CurrentValue = harvestable.CurrentHealth;
//         MaxValue = harvestable.MaxHealth;
//         CritWidth = critWidthPercent;
//         float rangeHigh = 100 - (critWidthPercent / 2);
//         float rangeLow = critWidthPercent / 2;
//         CritPosition = Random.Range(rangeLow, rangeHigh);
//     }
// }
