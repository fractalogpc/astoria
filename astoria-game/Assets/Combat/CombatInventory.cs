using System;
using System.Collections.Generic;
using Mirror.BouncyCastle.Security;
using UnityEngine;

/// <summary>
/// Holds a list of weapons that the player can use in combat. CombatViewmodelAndLogicManager reads from this to spawn in the weapon.
/// </summary>
[Serializable]
public class CombatWeaponInstance
{
    public CombatWeaponData WeaponData;
    public GameObject WeaponViewmodel;
    public GameObject WeaponLogic;
}

public class CombatInventory : MonoBehaviour
{
    [SerializeField] private CombatViewmodelManager _combatViewmodelManager;
    [SerializeField] private CombatWeaponLogicManager _combatWeaponLogicManager;
    [SerializeField] private CombatWeaponInstance _defaultWeapon;
    public enum WeaponSlot
    {
        Primary,
        Pistol,
        Special,
    }
    public CombatWeaponInstance CurrentWeapon => CurrentSlot switch
    {
        WeaponSlot.Primary => PrimaryWeapon,
        WeaponSlot.Pistol => PistolWeapon,
        WeaponSlot.Special => SpecialWeapon,
        _ => throw new ArgumentOutOfRangeException()
    };
    public WeaponSlot CurrentSlot;
    [Header("Dont change these manually, these are assigned by the script.")]
    public CombatWeaponInstance PrimaryWeapon;
    public CombatWeaponInstance PistolWeapon;
    public CombatWeaponInstance SpecialWeapon;

    public CombatWeaponInstance CreateInstanceFromData(CombatWeaponData data) {
        CombatWeaponInstance instance = new CombatWeaponInstance();
        instance.WeaponData = data;
        instance.WeaponViewmodel = _combatViewmodelManager.SetWeaponViewmodel(data.WeaponViewmodelPrefab);
        instance.WeaponLogic = Instantiate(data.WeaponLogicPrefab);
        return instance;
    }
    /// <summary>
    /// Equips the weapon in the specified slot. Calls the CombatViewmodelManager and CombatWeaponLogicManager to spawn in the viewmodel and logic prefabs.
    /// </summary>
    /// <param name="slot">The specified slot.</param>
    public void EquipWeaponInSlot(WeaponSlot slot) {
        CurrentSlot = slot;
        switch (slot) {
            case WeaponSlot.Primary:
                _combatViewmodelManager.SetWeaponViewmodel(PrimaryWeapon.WeaponViewmodel);
                break;
            case WeaponSlot.Pistol:
                _combatViewmodelManager.SetWeaponViewmodel(PistolWeapon.WeaponViewmodel);
                break;
            case WeaponSlot.Special:
                _combatViewmodelManager.SetWeaponViewmodel(SpecialWeapon.WeaponViewmodel);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    private void SwitchToPrimary() {
        CurrentSlot = WeaponSlot.Primary;
        _combatViewmodelManager.SetWeaponViewmodel(PrimaryWeapon.WeaponViewmodel);
    }
    private void SwitchToPistol() {
        CurrentSlot = WeaponSlot.Pistol;
        _combatViewmodelManager.SetWeaponViewmodel(PistolWeapon.WeaponViewmodel);
    }
    private void SwitchToSpecial() {
        CurrentSlot = WeaponSlot.Special;
        _combatViewmodelManager.SetWeaponViewmodel(SpecialWeapon.WeaponViewmodel);
    }
}
