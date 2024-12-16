using System;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

public enum FireMode
{
	Semi,
	Burst,
	Full
}

/// <summary>
/// Represents a weapon item. This is a monolithic class, but that should be okay since we don't have many different weapon types.
/// </summary>
[Serializable]
public class GunInstance : ItemInstance
{
	public bool Initialized = false;
	public GunData WeaponData => (GunData)ItemData;
	public FireMode CurrentFireMode { get; private set; }
	public int CurrentAmmo { get; private set; }
	public bool CanFire => CurrentAmmo > 0 && !_isReloading;
	private ProjectileManager _projectileManager;
	private CombatViewmodelManager _viewmodelManager;
	private FireLogic _currentFireLogic;
	private bool _isReloading;
	
	// This constructor is called when the item is created in the inventory, it is not initialized yet
	public GunInstance(GunData gunData) : base(gunData) {
		ItemData = gunData;
	}
	
	// Assign any references that need to be attached when first equipped here
	public void InitializeWeapon(CombatViewmodelManager viewmodelManager) {
		Initialized = true;
		_projectileManager = ProjectileManager.Instance;
		_viewmodelManager = viewmodelManager;
	}
	
	/// <summary>
	/// Should only be called by FireLogic and its children
	/// </summary>
	public void Fire() {
		if (!Initialized) return;
		if (IsShotgun(WeaponData.FireCombination)) {
			// Shotgun logic
		} else {
			// Regular logic
		}
	}
	
	public void SwitchFireMode() {
		FireCombinations fireCombination = WeaponData.FireCombination;
		// Iterate through the fire modes until we find the next available one
		for (int i = 0; i < Enum.GetValues(typeof(FireMode)).Length; i++) {
			FireMode nextMode = (FireMode)(((int)CurrentFireMode + i) % Enum.GetValues(typeof(FireMode)).Length);
			if (!ModeAvailable(nextMode, fireCombination)) continue;
			SetFireMode(nextMode);
			return;
		}
	}

	public void OnInspect() {
		Debug.Log("Implement OnInspect()");
	}
	
	public void OnFireDown() {
		if (!Initialized) return;
		_currentFireLogic.OnFireDown();
	}
	public void OnFireUp() {
		if (!Initialized) return;
		_currentFireLogic.OnFireUp();
	}
	public void OnReloadDown() {
		if (!Initialized) return;
		switch (WeaponData.ReloadType) {
			case ReloadTypes.MagazineClosedBolt:
				// One in chamber and magazine is full
				if (CurrentAmmo > WeaponData.MagazineSetting.MagazineCapacity) return;
				// One in chamber and magazine is not full
				if (CurrentAmmo == WeaponData.MagazineSetting.MagazineCapacity) {
					
				}
		}
	}
	public void OnReloadUp() {
		if (!Initialized) return;
		// _currentFireLogic.OnReloadUp();
	}
	public void OnAimDown() {
		if (!Initialized) return;
		_currentFireLogic.OnAimDown();
	}
	public void OnAimUp() {
		if (!Initialized) return;
		_currentFireLogic.OnAimUp();
	}
	public void Tick() {
		if (!Initialized) return;
		_currentFireLogic.Tick();
	}
	
	private void SetFireMode(FireMode mode) {
		CurrentFireMode = mode;
		if (_currentFireLogic != null) {
			_currentFireLogic.Cleanup();
			_currentFireLogic = null;
		}
		switch (mode) {
			case FireMode.Semi:
				_currentFireLogic = new SemiLogic(this);
				break;
			case FireMode.Burst:
				_currentFireLogic = new BurstLogic(this);
				break;
			case FireMode.Full:
				_currentFireLogic = new FullAutoLogic(this);
				break;
			default:
				// This should not happen
				Debug.LogError("GunInstance: Invalid fire mode");
				throw new ArgumentOutOfRangeException();
		}
		_currentFireLogic.Initialize();
	}

	private bool ModeAvailable(FireMode mode, FireCombinations combination) {
		return combination switch {
			FireCombinations.Semi or FireCombinations.ShotgunSemi => mode == FireMode.Semi,
			FireCombinations.SemiBurst or FireCombinations.ShotgunSemiBurst => mode is FireMode.Semi or FireMode.Burst,
			FireCombinations.SemiFull or FireCombinations.ShotgunSemiFull => mode is FireMode.Semi or FireMode.Full,
			FireCombinations.SemiBurstFull or FireCombinations.ShotgunSemiBurstFull => mode is FireMode.Semi or FireMode.Burst or FireMode.Full,
			_ => false
		};
	}

	private bool IsShotgun(FireCombinations combination) {
		return combination is
			FireCombinations.ShotgunSemi or
			FireCombinations.ShotgunSemiBurst or
			FireCombinations.ShotgunSemiFull or
			FireCombinations.ShotgunSemiBurstFull;
	}
}