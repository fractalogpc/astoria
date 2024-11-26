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
    [SerializeField] private CombatInventory _inventory;
    [SerializeField] private CombatViewmodelManager _combatViewmodelManager;
    [SerializeField] private WeaponData DebugPrimary;
    [SerializeField] private WeaponData DebugPistol;
    [SerializeField] private WeaponData DebugSpecial;

    protected override void InitializeActionMap() {
        
    }

    public void EquipWeapon(WeaponData data) {
        
    }

    
}
