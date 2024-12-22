using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Mirror;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

/// <summary>
/// Tightly coupled with CombatViewmodelManager.
///
/// This handles:
/// Holding references to combat scripts
/// Initializing weapons with references to combat scripts
/// Sending inputs to weapons
/// Running coroutines on weapons
/// </summary>

[RequireComponent(typeof(CombatInventory))]
public class CombatCore : NetworkedInputHandlerBase
{
    [SerializeField] private InventoryComponent _playerInventory;
    [SerializeField] private CombatViewmodelManager _combatViewmodelManager;
    public GunInstance CurrentGunInstance { get; private set; }
    
    /// <summary>
    /// Fires when the equipped weapon changes. The parameter is the new weapon.
    /// </summary>
    public UnityEvent<GunInstance> OnEquipWeapon;
    
    /// <summary>
    /// Fires when the ammo of the equipped weapon changes. The first parameter is the current ammo, the second is the max ammo.
    /// </summary>
    public UnityEvent<int, int> OnAmmoChanged;
    
    protected override void InitializeActionMap() {
        RegisterAction(_inputActions.Player.Attack, ctx => OnFireDown(), () => OnFireUp());
        RegisterAction(_inputActions.Player.Reload, ctx => OnReloadDown(), () => OnReloadUp());
        RegisterAction(_inputActions.Player.AttackSecondary, ctx => OnAimDown(), () => OnAimUp());
        RegisterAction(_inputActions.Player.InspectItem, ctx => OnInspect());
        RegisterAction(_inputActions.Player.SwitchFireMode, ctx => OnSwitchFireMode());
    }
    
    public void EquipWeapon(GunInstance instance) {
        CurrentGunInstance = instance;
        if (instance.Initialized == false) {
            instance.InitializeWeapon(this, _combatViewmodelManager, _playerInventory);
        }
        _combatViewmodelManager.SetViewmodelFor(instance);
        instance.AmmoChanged += OnInstanceAmmoChanged;
        OnEquipWeapon.Invoke(CurrentGunInstance);
    }

    private void OnDisable() {
        if (CurrentGunInstance == null) return;
        CurrentGunInstance.AmmoChanged -= OnInstanceAmmoChanged;
    }
    
    private void OnInstanceAmmoChanged(int old, int current) {
        OnAmmoChanged?.Invoke(old, current);
    }
    
    public void UnequipWeapon() {
        if (CurrentGunInstance == null) return;
        CurrentGunInstance.AmmoChanged -= OnInstanceAmmoChanged;
        CurrentGunInstance.Unequip();
        StartCoroutine(UnequipWeaponCoroutine());
        CurrentGunInstance = null;
    }
    private IEnumerator UnequipWeaponCoroutine() {
        yield return new WaitForSeconds(_combatViewmodelManager.PlayHolster());
        _combatViewmodelManager.RemoveViewmodel();
    }

    private void Update() {
        if (CurrentGunInstance == null) return;
        CurrentGunInstance.Tick();
    }
    
    public void OnFireDown() {
        if (CurrentGunInstance == null) return;
        CurrentGunInstance.OnFireDown();
    }
    public void OnFireUp() {
        if (CurrentGunInstance == null) return;
        CurrentGunInstance.OnFireUp();
    }
    public void OnReloadDown() {
        if (CurrentGunInstance == null) return;
        CurrentGunInstance.OnReloadDown();
    }
    public void OnReloadUp() {
        if (CurrentGunInstance == null) return;
        CurrentGunInstance.OnReloadUp();
    }
    public void OnAimDown() {
        if (CurrentGunInstance == null) return;
        CurrentGunInstance.OnAimDown();   
    }
    public void OnAimUp() {
        if (CurrentGunInstance == null) return;
        CurrentGunInstance.OnAimUp();
    }
    public void OnInspect() {
        if (CurrentGunInstance == null) return;
        CurrentGunInstance.OnInspect(); 
    }
    public void OnSwitchFireMode() {
        if (CurrentGunInstance == null) return;
        CurrentGunInstance.SwitchFireMode(); 
    }
}
