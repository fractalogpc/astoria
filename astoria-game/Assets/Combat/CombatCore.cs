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
public class CombatCore : NetworkedInputHandlerBase
{
    [SerializeField] private CombatViewmodelManager _combatViewmodelManager;
    [SerializeField] private CombatWeaponLogicManager _combatWeaponLogicManager;
    [SerializeField] private List<CombatWeapon> _weaponInstances = new List<CombatWeapon>();
    
    // These should be handled by the CombatInventory
    [SerializeField] private WeaponData DebugPrimary;
    [SerializeField] private WeaponData DebugPistol;
    [SerializeField] private WeaponData DebugSpecial;
    
    private CombatWeapon _primaryWeapon;
    private CombatWeapon _secondaryWeapon;
    private CombatWeapon _specialWeapon;
    
    
    protected override void InitializeActionMap() {
        LocalPlayerIndicator localPlayerIndicator = GetComponentInParent<LocalPlayerIndicator>();
        if (!localPlayerIndicator.IsLocalClientPlayer) {
            Debug.Log($"{localPlayerIndicator.gameObject.name} CombatCore: Skipping initialization of inputs for network player.");
            return;
        }
        RegisterAction(_inputActions.Player.EquipPrimary, ctx => EquipPrimary());
        RegisterAction(_inputActions.Player.EquipSecondary, ctx => EquipSecondary());
        RegisterAction(_inputActions.Player.EquipSpecial, ctx => EquipSpecial());
    }

    private void Start() {
        if (DebugPrimary != null) _primaryWeapon = RegisterWeaponByData(DebugPrimary);
        if (DebugPistol != null) _secondaryWeapon = RegisterWeaponByData(DebugPistol);
        if (DebugSpecial != null) _specialWeapon = RegisterWeaponByData(DebugSpecial);
    }
    private void EquipPrimary() {
        print("Equipping primary weapon.");
        EquipWeapon(_primaryWeapon);
    }
    private void EquipSecondary() {
        EquipWeapon(_secondaryWeapon);
    }
    private void EquipSpecial() {
        EquipWeapon(_specialWeapon);
    }
    
    public CombatWeapon RegisterWeaponByData(WeaponData data) {
        GameObject viewmodel = _combatViewmodelManager.AddViewmodel(data.ViewmodelPrefab);
        GameObject logic = _combatWeaponLogicManager.AddWeaponLogic(data.LogicPrefab);
        CombatWeapon weapon = new(data, viewmodel, logic);
        _weaponInstances.Add(weapon);
        return weapon;
    }
    public void RegisterWeapon(CombatWeapon weapon) {
        _weaponInstances.Add(weapon);
        _combatViewmodelManager.AddViewmodel(weapon.WeaponViewmodelInstance);
        _combatWeaponLogicManager.AddWeaponLogic(weapon.WeaponLogicInstance);
    }
    public bool EquipWeapon(CombatWeapon weapon) {
        if (_weaponInstances.Contains(weapon)) {
            _combatViewmodelManager.SetCurrentViewmodelTo(weapon.WeaponViewmodelInstance);
            _combatWeaponLogicManager.SetCurrentWeaponLogicTo(weapon.WeaponLogicInstance);
            return true;
        }
        return false;
    }
}
