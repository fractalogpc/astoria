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
	
	private ProjectileManager _projectileManager;
	private GunLogic _currentLogic;
	
	// This constructor is called when the item is created in the inventory, it is not initialized yet
	public GunInstance(GunData gunData) : base(gunData) {
		ItemData = gunData;
	}
	
	// Assign any references that need to be attached when first equipped here
	public void InitializeWeapon() {
		Initialized = true;
		_projectileManager = ProjectileManager.Instance;
	}
	
	public void OnFireDown() {
		if (!Initialized) return;
		_currentLogic.OnFireDown();
	}
	public void OnFireUp() {
		if (!Initialized) return;
		_currentLogic.OnFireUp();
	}
	public void OnReloadDown() {
		if (!Initialized) return;
		_currentLogic.OnReloadDown();
	}
	public void OnReloadUp() {
		if (!Initialized) return;
		_currentLogic.OnReloadUp();
	}
	public void OnAimDown() {
		if (!Initialized) return;
		_currentLogic.OnAimDown();
	}
	public void OnAimUp() {
		if (!Initialized) return;
		_currentLogic.OnAimUp();
	}
	public void Tick() {
		if (!Initialized) return;
		_currentLogic.Tick();
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

	private void Fire() {
		if (!Initialized) return;
	}
	
	private void SetFireMode(FireMode mode) {
		CurrentFireMode = mode;
		if (_currentLogic != null) {
			_currentLogic.Cleanup();
			_currentLogic = null;
		}
		switch (mode) {
			case FireMode.Semi:
				_currentLogic = new SemiLogic(this);
				break;
			case FireMode.Burst:
				_currentLogic = new BurstLogic(this);
				break;
			case FireMode.Full:
				_currentLogic = new FullAutoLogic(this);
				break;
			default:
				// This should not happen
				Debug.LogError("GunInstance: Invalid fire mode");
				throw new ArgumentOutOfRangeException();
		}
		_currentLogic.Initialize();
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