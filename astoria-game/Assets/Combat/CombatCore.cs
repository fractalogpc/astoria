using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Mirror;
using UnityEngine;

/// <summary>
/// Tightly coupled with CombatViewmodelManager.
///
/// This handles:
/// Sending inputs to weapons
/// </summary>

[RequireComponent(typeof(CombatInventory))]
public class CombatCore : NetworkedInputHandlerBase
{
    [SerializeField] private CombatViewmodelManager _combatViewmodelManager;
    public GunInstance CurrentGunInstance { get; private set; }
    
    public void EquipWeapon(GunInstance instance) {
        CurrentGunInstance = instance;
        _combatViewmodelManager.AddViewmodelOf(instance);
    }
    protected override void InitializeActionMap() {
        
    }
}
