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

[RequireComponent(typeof(CombatInput))]
[RequireComponent(typeof(CombatInventory))]
public class CombatCore : MonoBehaviour
{
    [SerializeField] private CombatInput _combatInput;
    [SerializeField] private CombatInventory _inventory;
    [SerializeField] private CombatViewmodelManager _combatViewmodelManager;
    [SerializeField] private CombatWeaponData DebugPrimary;
    [SerializeField] private CombatWeaponData DebugPistol;
    [SerializeField] private CombatWeaponData DebugSpecial;
   
    private void Start() {
        _combatInput.WeaponPrimary.AddListener(SwitchToPrimary);
        _combatInput.WeaponPistol.AddListener(SwitchToPistol);
        _combatInput.WeaponSpecial.AddListener(SwitchToSpecial);    
    }

    private void OnDisable() {
        _combatInput.WeaponPrimary.RemoveListener(SwitchToPrimary);
        _combatInput.WeaponPistol.RemoveListener(SwitchToPistol);
        _combatInput.WeaponSpecial.RemoveListener(SwitchToSpecial);
    }

    private void SwitchToPrimary() {
        _inventory.EquipWeaponInSlot(CombatInventory.WeaponSlot.Primary);
    }
    private void SwitchToPistol() {
        _inventory.EquipWeaponInSlot(CombatInventory.WeaponSlot.Primary);
    }
    private void SwitchToSpecial() {
        _inventory.EquipWeaponInSlot(CombatInventory.WeaponSlot.Primary);
    }
}
