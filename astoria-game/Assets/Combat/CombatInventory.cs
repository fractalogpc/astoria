using System;
using System.Collections.Generic;
using Mirror;
using Mirror.BouncyCastle.Security;
using UnityEngine;

/// <summary>
/// Handles calling CombatCore with the main 3 weapon slots.
/// </summary>
public class CombatInventory : InputHandlerBase
{
    [SerializeField] private CombatCore _combatCore;
    [SerializeField] private WeaponData _defaultWeapon;
    public enum WeaponSlot
    {
        PrimarySlot,
        SecondarySlot,
        SpecialSlot,
    }
    [ReadOnly] public WeaponSlot CurrentSlot;
    public WeaponInstance CurrentWeaponInstanceItem => CurrentSlot switch
    {
        WeaponSlot.PrimarySlot => _primaryWeaponInstance,
        WeaponSlot.SecondarySlot => _secondaryWeaponInstance,
        WeaponSlot.SpecialSlot => _specialWeaponInstance,
        _ => throw new ArgumentOutOfRangeException()
    };
    [ReadOnly] public WeaponInstance _primaryWeaponInstance;
    [ReadOnly] public WeaponInstance _secondaryWeaponInstance;
    [ReadOnly] public WeaponInstance _specialWeaponInstance;
    
    protected override void InitializeActionMap() {
        RegisterAction(_inputActions.Player.EquipPrimary, ctx => EquipSlot(WeaponSlot.PrimarySlot));
        RegisterAction(_inputActions.Player.EquipSecondary, ctx => EquipSlot(WeaponSlot.SecondarySlot));
        RegisterAction(_inputActions.Player.EquipSpecial, ctx => EquipSlot(WeaponSlot.SpecialSlot));
    }
    
    /// <summary>
    /// Equips the weapon instance in the specified slot.
    /// </summary>
    /// <param name="slot">The specified slot.</param>
    private void EquipSlot(WeaponSlot slot) {
        CurrentSlot = slot;
        switch (slot) {
            case WeaponSlot.PrimarySlot:
                if (_primaryWeaponInstance == null) return;
                _combatCore.EquipWeapon(_primaryWeaponInstance);
                break;
            case WeaponSlot.SecondarySlot:
                if (_secondaryWeaponInstance == null) return;
                _combatCore.EquipWeapon(_secondaryWeaponInstance);
                break;
            case WeaponSlot.SpecialSlot:
                if (_specialWeaponInstance == null) return;
                _combatCore.EquipWeapon(_specialWeaponInstance);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    public bool AddWeaponInstanceToSlot(WeaponInstance instance, WeaponSlot slot) {
        switch (slot) {
            case WeaponSlot.PrimarySlot:
                if (_primaryWeaponInstance != null) return false;
                _primaryWeaponInstance = instance;
                break;
            case WeaponSlot.SecondarySlot:
                if (_secondaryWeaponInstance != null) return false;
                _secondaryWeaponInstance = instance;
                break;
            case WeaponSlot.SpecialSlot:
                _specialWeaponInstance = instance;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        return true;
    }
    public WeaponInstance RemoveWeaponFromSlot(WeaponSlot slot) {
        WeaponInstance weaponInstanceItem;
        switch (slot) {
            case WeaponSlot.PrimarySlot:
                weaponInstanceItem = _primaryWeaponInstance;
                _primaryWeaponInstance = null;
                break;
            case WeaponSlot.SecondarySlot:
                weaponInstanceItem = _secondaryWeaponInstance;
                _secondaryWeaponInstance = null;
                break;
            case WeaponSlot.SpecialSlot:
                weaponInstanceItem = _specialWeaponInstance;
                _specialWeaponInstance = null;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        return weaponInstanceItem;
    }


}
