using System;
using Mirror;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

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
	public bool HasAmmo => CurrentAmmo > 0 && !_isReloading;
	private CombatCore _combatCore;
	private CombatViewmodelManager _viewmodelManager;
	private ProjectileManager _projectileManager;
	private FireLogic _currentFireLogic;
	private bool _isReloading;
	
	// This constructor is called when the item is created in the inventory, it is not initialized yet
	public GunInstance(GunData gunData) : base(gunData) {
		ItemData = gunData;
	}
	
	// Assign any references that need to be attached when first equipped here
	public void InitializeWeapon(CombatCore combatCore, CombatViewmodelManager viewmodelManager) {
		Initialized = true;
		_combatCore = combatCore;
		_viewmodelManager = viewmodelManager;
		_projectileManager = ProjectileManager.Instance;
	}
	
	public void Unequip() {
		_currentFireLogic.Cleanup();
		Initialized = false;
		_combatCore = null;
		_viewmodelManager = null;
		_projectileManager = null;
		_currentFireLogic = null;
	}
	
	/// <summary>
	/// Should only be called by FireLogic and its children
	/// </summary>
	public void Fire() {
		if (!Initialized) return;
		if (IsShotgun(WeaponData.FireCombination)) {
			for (int i = 0; i < WeaponData.ShotgunSetting.PelletsPerShot; i++) {
				ShootProjectile(GetRandomSpreadAngle());
			}
			CurrentAmmo--;
		}
		else {
			ShootProjectile(GetRandomSpreadAngle());
			CurrentAmmo--;
		}
	}

	private void ShootProjectile(float spreadAngle = 0) {
		Vector3 direction = Quaternion.Euler(spreadAngle, spreadAngle, 0) * Camera.main.transform.forward;
		_projectileManager.FireProjectile(
			WeaponData.Damage, 
			WeaponData.BulletMassKg, 
			Camera.main.transform.position,
			direction, 
			new ProjectileManager.Aerodynamics(WeaponData.DragCoefficient, WeaponData.BulletDiameterM, WeaponData.AirDensityKgPerM), 
			ProjectileCallback	
		);
	}

	private void ProjectileCallback(RaycastHit hit) {
		Debug.Log($"hit {hit.collider.gameObject.name} at {hit.point}");
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
				// Magazine is empty and chamber is empty
				if (CurrentAmmo == 0) {
					CurrentAmmo = WeaponData.MagazineSetting.MagazineCapacity;
					Debug.Log("Add waiting here");
					return;
				}
				// One in chamber and magazine is not full
				if (CurrentAmmo <= WeaponData.MagazineSetting.MagazineCapacity) {
					CurrentAmmo = WeaponData.MagazineSetting.MagazineCapacity + 1;
					Debug.Log("Add waiting here");
					return;
				}
				// One in chamber and magazine is full
				if (CurrentAmmo > WeaponData.MagazineSetting.MagazineCapacity) return;
				break;
			case ReloadTypes.MagazineOpenBolt:
				// Magazine is not full
				if (CurrentAmmo < WeaponData.MagazineSetting.MagazineCapacity) {
					CurrentAmmo = WeaponData.MagazineSetting.MagazineCapacity;
					Debug.Log("Add waiting here");
				}
				break;
			default:
				throw new NotImplementedException();
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
				_currentFireLogic = new BurstLogic(this, _combatCore);
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

	private float GetRandomSpreadAngle() {
		float angle = CalculateSpreadAngle(WeaponData.AccuracySetting.PatternSpread, WeaponData.AccuracySetting.EffectiveRange);
		return Random.Range(-angle, angle);
	}
	
	private float CalculateSpreadAngle(float spreadDiameter, float distance) {
		return Mathf.Atan2(spreadDiameter / 2f, distance) * Mathf.Rad2Deg;
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