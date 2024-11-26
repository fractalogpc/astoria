using System;
using System.Collections.Generic;
using Mirror;
using Mirror.BouncyCastle.Security;
using UnityEngine;

/// <summary>
/// Handles calling CombatCore with the main 3 weapon slots.
/// </summary>
public class CombatInventory : MonoBehaviour
{
    // [SerializeField] private CombatViewmodelManager _combatViewmodelManager;
    // [SerializeField] private CombatWeaponLogicManager _combatWeaponLogicManager;
    // [SerializeField] private WeaponData _defaultWeapon;
    // public enum WeaponSlot
    // {
    //     PrimarySlot,
    //     SecondarySlot,
    //     SpecialSlot,
    // }
    // [ReadOnly] public WeaponSlot CurrentSlot;
    // public CombatWeapon CurrentWeapon => CurrentSlot switch
    // {
    //     WeaponSlot.PrimarySlot => PrimaryWeapon,
    //     WeaponSlot.SecondarySlot => SecondaryWeapon,
    //     WeaponSlot.SpecialSlot => SpecialWeapon,
    //     _ => throw new ArgumentOutOfRangeException()
    // };
    // [ReadOnly] public CombatWeapon PrimaryWeapon;
    // [ReadOnly] public CombatWeapon SecondaryWeapon;
    // [ReadOnly] public CombatWeapon SpecialWeapon;
    //
    // public CombatWeapon CreateInstanceFromData(WeaponData data) {
    //     CombatWeapon instance = new CombatWeapon(data, _combatViewmodelManager, _combatWeaponLogicManager);
    //     return instance;
    // }
    // /// <summary>
    // /// Equips the weapon in the specified slot. Calls the CombatViewmodelManager and CombatWeaponLogicManager to spawn in the viewmodel and logic prefabs.
    // /// </summary>
    // /// <param name="slot">The specified slot.</param>
    // public void EquipSlot(WeaponSlot slot) {
    //     CurrentSlot = slot;
    //     switch (slot) {
    //         case WeaponSlot.PrimarySlot:
    //             _combatViewmodelManager.SetCurrentViewmodelTo(PrimaryWeapon.WeaponViewmodelInstance);
    //             break;
    //         case WeaponSlot.SecondarySlot:
    //             _combatViewmodelManager.SetCurrentViewmodelTo(SecondaryWeapon.WeaponViewmodelInstance);
    //             break;
    //         case WeaponSlot.SpecialSlot:
    //             _combatViewmodelManager.SetCurrentViewmodelTo(SpecialWeapon.WeaponViewmodelInstance);
    //             break;
    //         default:
    //             throw new ArgumentOutOfRangeException();
    //     }
    // }
    // // public bool AddWeaponToSlot(WeaponData data, WeaponSlot slot) {
    // //     switch (slot) {
    // //         case WeaponSlot.PrimarySlot:
    // //             if (PrimaryWeapon != null) return false;
    // //             PrimaryWeapon = weapon;
    // //             break;
    // //         case WeaponSlot.SecondarySlot:
    // //             if (SecondaryWeapon != null) return false;
    // //             SecondaryWeapon = weapon;
    // //             break;
    // //         case WeaponSlot.SpecialSlot:
    // //             SpecialWeapon = weapon;
    // //             break;
    // //         default:
    // //             throw new ArgumentOutOfRangeException();
    // //     }
    // //     return true;
    // // }
}
