using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Mirror;
using UnityEngine;

/// <summary>
/// Tightly coupled with CombatInventory and CombatViewmodelAndLogicManager.
/// This should be attached to the parent of the player's viewmodel/fps arms.
///
/// It handles:
/// Switching between viewmodels.
/// Left/right sway from the player's movement.
/// Calling fire animations. (CombatViewmodelAndLogicManager)
/// Calling fire functions. (CombatWeaponLogic)
/// </summary>

[RequireComponent(typeof(CombatInventory))]
public class CombatCore : NetworkBehaviour
{
    [SerializeField] private CombatViewmodelManager _combatViewmodelManager;
    [SerializeField] private CombatWeaponLogicManager _combatWeaponLogicManager;
    [SerializeField] private List<WeaponInstance> _weaponInstances = new List<WeaponInstance>();
    
    /// <summary>
    /// Use this to create weapon instances. Never call the constructor directly.
    /// </summary>
    /// <param name="data">The data to create the instance from.</param>
    /// <returns>The weapon instance that was created.</returns>
    public WeaponInstance CreateWeaponInstance(WeaponData data) {
        WeaponInstance instance = new WeaponInstance(data);
        GameObject viewmodel = _combatViewmodelManager.AddViewmodel(instance);
        instance.WeaponViewmodelInstance = viewmodel;
        GameObject logic = _combatWeaponLogicManager.AddWeaponLogic(instance);
        instance.WeaponLogicInstance = logic;
        _weaponInstances.Add(instance);
        return instance;
    }
    /// <summary>
    /// Equips a weapon instance.
    /// </summary>
    /// <param name="weaponInstanceItem">The weapon instance to equip.</param>
    /// <returns>Whether equipping was successful.</returns>
    public bool EquipWeapon(WeaponInstance weaponInstanceItem) {
        if (_weaponInstances.Contains(weaponInstanceItem)) {
            _combatViewmodelManager.SetCurrentViewmodelTo(weaponInstanceItem.WeaponViewmodelInstance);
            _combatWeaponLogicManager.SetCurrentWeaponLogicTo(weaponInstanceItem.WeaponLogicInstance);
            return true;
        }
        return false;
    }
}
