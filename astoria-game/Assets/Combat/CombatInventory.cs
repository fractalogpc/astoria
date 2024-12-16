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
    [SerializeField] private GunData _defaultGun;
    public enum WeaponSlot
    {
        PrimarySlot,
        SecondarySlot,
        SpecialSlot,
    }
    [ReadOnly] public WeaponSlot CurrentSlot;
    public GunInstance CurrentGunInstanceItem => CurrentSlot switch
    {
        WeaponSlot.PrimarySlot => _primaryGunInstance,
        WeaponSlot.SecondarySlot => _secondaryGunInstance,
        WeaponSlot.SpecialSlot => _specialGunInstance,
        _ => throw new ArgumentOutOfRangeException()
    };
    [ReadOnly] public GunInstance _primaryGunInstance;
    [ReadOnly] public GunInstance _secondaryGunInstance;
    [ReadOnly] public GunInstance _specialGunInstance;
    
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
                if (_primaryGunInstance == null) return;
                _combatCore.EquipWeapon(_primaryGunInstance);
                break;
            case WeaponSlot.SecondarySlot:
                if (_secondaryGunInstance == null) return;
                _combatCore.EquipWeapon(_secondaryGunInstance);
                break;
            case WeaponSlot.SpecialSlot:
                if (_specialGunInstance == null) return;
                _combatCore.EquipWeapon(_specialGunInstance);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    public bool AddWeaponInstanceToSlot(GunInstance instance, WeaponSlot slot) {
        switch (slot) {
            case WeaponSlot.PrimarySlot:
                if (_primaryGunInstance != null) return false;
                _primaryGunInstance = instance;
                break;
            case WeaponSlot.SecondarySlot:
                if (_secondaryGunInstance != null) return false;
                _secondaryGunInstance = instance;
                break;
            case WeaponSlot.SpecialSlot:
                _specialGunInstance = instance;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        return true;
    }
    public GunInstance RemoveWeaponFromSlot(WeaponSlot slot) {
        GunInstance gunInstanceItem;
        switch (slot) {
            case WeaponSlot.PrimarySlot:
                gunInstanceItem = _primaryGunInstance;
                _primaryGunInstance = null;
                break;
            case WeaponSlot.SecondarySlot:
                gunInstanceItem = _secondaryGunInstance;
                _secondaryGunInstance = null;
                break;
            case WeaponSlot.SpecialSlot:
                gunInstanceItem = _specialGunInstance;
                _specialGunInstance = null;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        return gunInstanceItem;
    }


}
