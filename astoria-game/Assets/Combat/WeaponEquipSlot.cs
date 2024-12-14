using System;
using Mirror;
using TMPro;
using TMPro.EditorUtilities;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

[RequireComponent(typeof(ClickableEvents))]
public class WeaponEquipSlot : InventoryEquipableSlot
{
    [SerializeField] private CombatInventory _combatInventory;
    [SerializeField] private CombatInventory.WeaponSlot _slotType;
    
    
}
