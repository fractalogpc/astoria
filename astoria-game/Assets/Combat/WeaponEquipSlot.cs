using UnityEngine;

[RequireComponent(typeof(ClickableEvents))]
public class WeaponEquipSlot : InventoryEquipableSlot
{
    [SerializeField] private CombatInventory _combatInventory;
    [SerializeField] private CombatInventory.WeaponSlot _slotType;
    
    
}
