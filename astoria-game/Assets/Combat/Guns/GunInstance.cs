using System;
using System.Collections;
using UnityEngine;
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
public class GunInstance : ItemInstance
{
	public bool Initialized = false;
	public GunData WeaponData => (GunData)ItemData;
	public FireMode CurrentFireMode { get; private set; }
	public int CurrentAmmo { get; private set; }
	public int GetMaxAmmo() {
		switch (WeaponData.ReloadType) {
			case ReloadTypes.MagazineClosedBolt or ReloadTypes.MagazineOpenBolt:
				return WeaponData.MagazineSetting.MagazineCapacity;
			case ReloadTypes.InternalClosedBolt or ReloadTypes.InternalOpenBolt:
				return WeaponData.InternalSetting.InternalCapacity;
			default:
				Debug.LogError("GunInstance: Did not account for all ReloadTypes in GetMaxAmmo()");
				return 0;
		}
	}
	private void SetCurrentAmmoTo(int value) {
		AmmoChanged.Invoke(CurrentAmmo, value);
		CurrentAmmo = value;
	}
	public bool HasAmmo => CurrentAmmo > 0 && !_isReloading;
	private CombatCore _combatCore;
	private CombatViewmodelManager _viewmodelManager;
	private InventoryComponent _playerInventory;
	private ProjectileManager _projectileManager;
	private FireLogic _currentFireLogic;
	private Coroutine _reloadCoroutine;
	private bool _isReloading;
	
	public delegate void AmmoChangedDelegate(int oldAmmo, int newAmmo);
	public event AmmoChangedDelegate AmmoChanged;
	
	// This constructor is called when the item is created in the inventory, it is not initialized yet
	public GunInstance(GunData gunData) : base(gunData) {
		ItemData = gunData;
	}
	
	// Assign any references that need to be attached when first equipped here
	public void InitializeWeapon(CombatCore combatCore, CombatViewmodelManager viewmodelManager, InventoryComponent playerInventory) {
		Initialized = true;
		_combatCore = combatCore;
		_viewmodelManager = viewmodelManager;
		_playerInventory = playerInventory;
		_projectileManager = ProjectileManager.Instance;
		switch (WeaponData.FireCombination) {
			case FireCombinations.Semi or FireCombinations.ShotgunSemi:
				SetFireMode(FireMode.Semi);
				break;
			case FireCombinations.SemiBurst or FireCombinations.ShotgunSemiBurst:
				SetFireMode(FireMode.Burst);
				break;
			case FireCombinations.SemiFull or FireCombinations.ShotgunSemiFull:
			case FireCombinations.SemiBurstFull or FireCombinations.ShotgunSemiBurstFull:
				SetFireMode(FireMode.Full);
				break;
			default:
				Debug.LogError("GunInstance: Did not account for all FireCombinations in initialization");
				break;
		}
	}

	public override void OnSelected() {
		base.OnSelected();
		if (_combatCore == null) {
			Debug.Log("Try to find a better way to initialize GunInstances when Selected than making CombatCore a singleton");
			_combatCore = CombatCore.Instance;
		}
		_combatCore.EquipWeapon(this);
	}
	
	public override void OnDeselected() {
		base.OnDeselected();
		_combatCore.UnequipWeapon();
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
			_viewmodelManager.PlayFire();
			SetCurrentAmmoTo(CurrentAmmo - 1);
		}
		else {
			ShootProjectile(GetRandomSpreadAngle());
			_viewmodelManager.PlayFire();
			SetCurrentAmmoTo(CurrentAmmo - 1);
		}
	}

	private void ShootProjectile(float spreadAngle = 0) {
		Vector3 direction = Quaternion.Euler(spreadAngle, spreadAngle, 0) * Camera.main.transform.forward * WeaponData.InitialVelocityMS;
		_projectileManager.FireProjectile(
			WeaponData.Damage, 
			WeaponData.BulletMassKg, 
			Camera.main.transform.position,
			direction, 
			new ProjectileManager.Aerodynamics(WeaponData.DragCoefficient, Mathf.Pow(WeaponData.BulletDiameterM / 2, 2) * Mathf.PI, WeaponData.AirDensityKgPerM), 
			ProjectileCallback	
		);
	}

	private void ProjectileCallback(RaycastHit hit) {
		Debug.Log($"hit {hit.collider.gameObject.name} at {hit.point}");
		HealthInterface healthInterface = hit.collider.gameObject.GetComponentInChildren<HealthInterface>();
		if (healthInterface != null) {
			healthInterface.Damage(WeaponData.Damage, hit.point);
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

	private IEnumerator ReloadCoroutine() {
		_isReloading = true;
		if (AmmoInInventory() <= 0) {
			_isReloading = false;
			yield break;
		}
		switch (WeaponData.ReloadType) {
			case ReloadTypes.MagazineClosedBolt:
				// One in chamber and magazine is full
				if (CurrentAmmo > WeaponData.MagazineSetting.MagazineCapacity) break;
				// Magazine is empty and chamber is empty
				if (CurrentAmmo == 0) {
					yield return new WaitForSeconds(_viewmodelManager.PlayReloadEmpty());
					if (AmmoInInventory() < WeaponData.MagazineSetting.MagazineCapacity) {
						SetCurrentAmmoTo(AmmoInInventory());
						RemoveAmmoFromInventory(CurrentAmmo);
					}
					else {
						SetCurrentAmmoTo(WeaponData.MagazineSetting.MagazineCapacity);
						RemoveAmmoFromInventory(WeaponData.MagazineSetting.MagazineCapacity);
					}
					break;
				}
				// One in chamber and magazine is not full
				if (CurrentAmmo <= WeaponData.MagazineSetting.MagazineCapacity) {
					yield return new WaitForSeconds(_viewmodelManager.PlayReloadPartial());
					int ammoNeeded = WeaponData.MagazineSetting.MagazineCapacity + 1 - CurrentAmmo;
					if (AmmoInInventory() < ammoNeeded) {
						SetCurrentAmmoTo(CurrentAmmo + AmmoInInventory());
						RemoveAmmoFromInventory(AmmoInInventory());
					}
					else {
						SetCurrentAmmoTo(WeaponData.MagazineSetting.MagazineCapacity + 1);
						RemoveAmmoFromInventory(ammoNeeded);
					}
					break;
				}
				break;
			case ReloadTypes.MagazineOpenBolt:
				// magazine is full
				if (CurrentAmmo >= WeaponData.MagazineSetting.MagazineCapacity) break;
				// Magazine is not full
				if (CurrentAmmo < WeaponData.MagazineSetting.MagazineCapacity) {
					yield return new WaitForSeconds(_viewmodelManager.PlayReloadPartial());
					int ammoNeeded = WeaponData.MagazineSetting.MagazineCapacity - CurrentAmmo;
					if (AmmoInInventory() < ammoNeeded) {
						SetCurrentAmmoTo(CurrentAmmo + AmmoInInventory());
						RemoveAmmoFromInventory(AmmoInInventory());
					}
					else {
						SetCurrentAmmoTo(WeaponData.MagazineSetting.MagazineCapacity);
						RemoveAmmoFromInventory(ammoNeeded);
					}
				}
				break;
			case ReloadTypes.InternalClosedBolt:
				// Internal is empty and chamber is empty
				if (CurrentAmmo == 0) {
					CurrentAmmo = WeaponData.MagazineSetting.MagazineCapacity;
					Debug.Log("Add cancelable reloading and chambering here");
				}
				// One in chamber and internal is not full
				if (CurrentAmmo <= WeaponData.MagazineSetting.MagazineCapacity) {
					CurrentAmmo = WeaponData.MagazineSetting.MagazineCapacity + 1;
					Debug.Log("Add cancelable reloading here");
				}
				// One in chamber and internal is full
				if (CurrentAmmo > WeaponData.MagazineSetting.MagazineCapacity) break;
				break;
			case ReloadTypes.InternalOpenBolt:	
				// Magazine is not full
				if (CurrentAmmo < WeaponData.MagazineSetting.MagazineCapacity) {
					CurrentAmmo = WeaponData.MagazineSetting.MagazineCapacity;
					Debug.Log("Add cancelable reloading here");
				}
				break;
		}
		_isReloading = false;
	}
	
	public void OnReloadDown() {
		if (!Initialized) return;
		if (!_isReloading) {
			_reloadCoroutine = _combatCore.StartCoroutine(ReloadCoroutine());
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
	
	private int AmmoInInventory() {
		return _playerInventory.GetItemsOfType(WeaponData.AmmoItem).Count;
	}
	
	private bool RemoveAmmoFromInventory(int amount) {
		return _playerInventory.TryRemoveItemByData(WeaponData.AmmoItem, amount);
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