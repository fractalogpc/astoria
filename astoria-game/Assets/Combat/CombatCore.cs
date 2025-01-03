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
    /// Fires when the equipped weapon is unequipped.
    /// </summary>
    public UnityEvent OnUnequipWeapon;
    
    /// <summary>
    /// Fires when the ammo of the equipped weapon changes. The first parameter is the current ammo, the second is the max ammo.
    /// </summary>
    public UnityEvent<int, int> OnAmmoChanged;
    
    public static CombatCore Instance { get; private set; }

    private void Start() {
        if (Instance != null) {
            Debug.LogError($"Removing CombatCore on {gameObject.name}. Multiple CombatCore instances are not allowed.");
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void EquipWeapon(GunInstance instance) {
        CurrentGunInstance = instance;
        if (instance.Initialized == false) {
            instance.InitializeWeapon(this, _combatViewmodelManager, _playerInventory);
        }
        _combatViewmodelManager.SetViewmodelFor(instance);
        instance.AmmoChanged += OnInstanceAmmoChanged;
        OnEquipWeapon?.Invoke(CurrentGunInstance);
    }
    public void UnequipWeapon() {
        if (CurrentGunInstance == null) return;
        CurrentGunInstance.AmmoChanged -= OnInstanceAmmoChanged;
        CurrentGunInstance.Unequip();
        StartCoroutine(UnequipWeaponCoroutine());
        CurrentGunInstance = null;
        OnUnequipWeapon?.Invoke();
    }
    
    protected override void InitializeActionMap() {
        RegisterAction(_inputActions.Player.Attack, ctx => OnFireDown(), () => OnFireUp());
        RegisterAction(_inputActions.Player.Reload, ctx => OnReloadDown(), () => OnReloadUp());
        RegisterAction(_inputActions.Player.AttackSecondary, ctx => OnAimDown(), () => OnAimUp());
        RegisterAction(_inputActions.Player.InspectItem, ctx => OnInspect());
        RegisterAction(_inputActions.Player.SwitchFireMode, ctx => OnSwitchFireMode());
    }
    
    private void OnDisable() {
        if (CurrentGunInstance == null) return;
        CurrentGunInstance.AmmoChanged -= OnInstanceAmmoChanged;
    }
    
    private void OnInstanceAmmoChanged(int old, int current) {
        OnAmmoChanged?.Invoke(old, current);
    }
    
    private IEnumerator UnequipWeaponCoroutine() {
        yield return new WaitForSeconds(_combatViewmodelManager.PlayHolster());
        _combatViewmodelManager.RemoveViewmodel();
    }

    private void Update() {
        if (CurrentGunInstance == null) return;
        CurrentGunInstance.Tick();
    }
    
    private void OnFireDown() {
        if (CurrentGunInstance == null) return;
        CurrentGunInstance.OnFireDown();
    }
    private void OnFireUp() {
        if (CurrentGunInstance == null) return;
        CurrentGunInstance.OnFireUp();
    }
    private void OnReloadDown() {
        if (CurrentGunInstance == null) return;
        CurrentGunInstance.OnReloadDown();
    }
    private void OnReloadUp() {
        if (CurrentGunInstance == null) return;
        CurrentGunInstance.OnReloadUp();
    }
    private void OnAimDown() {
        if (CurrentGunInstance == null) return;
        CurrentGunInstance.OnAimDown();   
    }
    private void OnAimUp() {
        if (CurrentGunInstance == null) return;
        CurrentGunInstance.OnAimUp();
    }
    private void OnInspect() {
        if (CurrentGunInstance == null) return;
        CurrentGunInstance.OnInspect(); 
    }
    private void OnSwitchFireMode() {
        if (CurrentGunInstance == null) return;
        CurrentGunInstance.SwitchFireMode(); 
    }
}
