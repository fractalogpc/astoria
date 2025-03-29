using System;
using System.Collections;
using TMPro.EditorUtilities;
using Unity.Cinemachine;
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
public class GunInstance : ViewmodelItemInstance
{
	public bool Initialized = false;
	public new GunData ItemData => (GunData)base.ItemData;
	public FireMode CurrentFireMode { get; private set; }
	public int CurrentAmmo { get; private set; }
	public bool HasAmmo => CurrentAmmo > 0 && !_isReloading;
	public delegate void AmmoChangedDelegate(int oldAmmo, int newAmmo);
	public event AmmoChangedDelegate AmmoChanged;
	private CombatCore _combatCore;
	private InventoryComponent _playerInventory;
	private ProjectileManager _projectileManager;
	private FireLogic _currentFireLogic;
	private Coroutine _reloadCoroutine;
	private bool _isReloading;
	private bool _isAds;
	private float _startCameraFov;
	
	// This constructor is called when the item is created in the inventory, it is not initialized yet
	public GunInstance(GunData gunData) : base(gunData) {
		_combatCore = CombatCore.Instance;
	}
	public int GetMaxAmmo() {
		switch (ItemData.ReloadType) {
			case ReloadTypes.MagazineClosedBolt or ReloadTypes.MagazineOpenBolt:
				return ItemData.MagazineSetting.MagazineCapacity;
			case ReloadTypes.InternalClosedBolt or ReloadTypes.InternalOpenBolt:
				return ItemData.InternalSetting.InternalCapacity;
			default:
				Debug.LogError("GunInstance: Did not account for all ReloadTypes in GetMaxAmmo()");
				return 0;
		}
	}
	
	public void InitializeWeapon() {
		Initialized = true;
		_projectileManager = ProjectileManager.Instance;
		_playerInventory = _combatCore.PlayerInventory;
		_startCameraFov = _combatCore.PlayerCamera.fieldOfView;
		switch (ItemData.FireCombination) {
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

	public override void OnHotbarSelected() {
		base.OnHotbarSelected();
		_combatCore.AttachToInputs(this);
		if (Initialized == false) InitializeWeapon();
	}
	
	public override void OnHotbarDeselected( ) {
		base.OnHotbarDeselected();
		_combatCore.DetachFromInputs();
		Unequip();
	}
	
	/// <summary>
	/// Should only be called by FireLogic and its children
	/// </summary>
	public void Fire() {
		if (!Initialized) return;
		// Call recoil on camera
		if (SoundManager.Instance != null) {
			SoundManager.Instance.EmitSound(new SoundEvent(GameObject.FindWithTag("Player").transform.position, 800f, "Gunshot"));
		}
		else {
			Debug.LogWarning("GunInstance: SoundManager.Instance not found! Gunshot sound will not alert SoundManager listeners.");
		}
		CombatCameraRecoil.Instance.ApplyRecoil(ItemData.RecoilSettings);
		if (IsShotgun(ItemData.FireCombination)) {
			for (int i = 0; i < ItemData.ShotgunSetting.PelletsPerShot; i++) {
				ShootProjectile(GetRandomSpreadAngle());
			}
			_viewmodelManager.SetTrigger("Fire");
			SetCurrentAmmoTo(CurrentAmmo - 1);
		}
		else {
			ShootProjectile(GetRandomSpreadAngle());
			_viewmodelManager.SetTrigger("Fire");
			SetCurrentAmmoTo(CurrentAmmo - 1);
		}
	}
	
	public void SwitchFireMode() {
		FireCombinations fireCombination = ItemData.FireCombination;
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
		
		if (!ItemData.CanAimDownSights) return;
		_viewmodel.EnableAds(ItemData.AdsIKTransitionTimeIn);
		_combatCore.CrosshairFade.FadeOut();
		_isAds = true;
	}
	public void OnAimUp() {
		if (!Initialized) return;
		_currentFireLogic.OnAimUp();
		if (!ItemData.CanAimDownSights) return;
		_viewmodel.DisableAds(ItemData.AdsIKTransitionTimeOut);
		_combatCore.CrosshairFade.FadeIn();
		_isAds = false;
	}
	public void Tick() {
		if (!Initialized) return;
		_currentFireLogic.Tick();
		if (!ItemData.CanAimDownSights) return; 
		_combatCore.PlayerCamera.fieldOfView = Mathf.Lerp(_combatCore.PlayerCamera.fieldOfView, _isAds ? ItemData.AdsFov : _startCameraFov, ItemData.AdsZoomSpeed * Time.deltaTime);
	}

	private void SetCurrentAmmoTo(int value) {
		AmmoChanged?.Invoke(CurrentAmmo, value);
		CurrentAmmo = value;
	}

	private void ShootProjectile(float spreadAngle = 0) {
		Vector3 direction = Quaternion.Euler(spreadAngle, spreadAngle, 0) * Camera.main.transform.forward * ItemData.InitialVelocityMS;
		_projectileManager.FireProjectile(
			ItemData.Damage, 
			ItemData.BulletMassKg, 
			Camera.main.transform.position,
			direction, 
			new ProjectileManager.Aerodynamics(ItemData.DragCoefficient, Mathf.Pow(ItemData.BulletDiameterM / 2, 2) * Mathf.PI, ItemData.AirDensityKgPerM), 
			ProjectileCallback	
		);
	}

	private void ProjectileCallback(RaycastHit hit) {
		IDamageable damageable = hit.collider.gameObject.GetComponentInChildren<IDamageable>();
		if (damageable != null) {
			damageable.TakeDamage(ItemData.Damage, hit.point);
		}
	}
	
	private void Unequip() {
		_currentFireLogic.Cleanup();
		_combatCore.PlayerCamera.fieldOfView = _startCameraFov;
		Initialized = false;
	}

	private IEnumerator ReloadCoroutine() {
		_isReloading = true;
		if (AmmoInInventory() <= 0) {
			_isReloading = false;
			yield break;
		}
		int invAmmo;
		switch (ItemData.ReloadType) {
			case ReloadTypes.MagazineClosedBolt:
				// One in chamber and magazine is full
				if (CurrentAmmo > ItemData.MagazineSetting.MagazineCapacity) break;
				// Magazine is empty and chamber is empty
				if (CurrentAmmo == 0) {
					_viewmodelManager.SetTrigger("ReloadEmpty");
					yield return new WaitForSeconds(ItemData.ReloadEmptyAnimation.length);
					if (AmmoInInventory() < ItemData.MagazineSetting.MagazineCapacity) {
						invAmmo = AmmoInInventory();
						RemoveAmmoFromInventory(CurrentAmmo);
						SetCurrentAmmoTo(invAmmo);
					}
					else {
						RemoveAmmoFromInventory(ItemData.MagazineSetting.MagazineCapacity);
						SetCurrentAmmoTo(ItemData.MagazineSetting.MagazineCapacity);
					}
					break;
				}
				// One in chamber and magazine is not full
				if (CurrentAmmo <= ItemData.MagazineSetting.MagazineCapacity) {
					_viewmodelManager.SetTrigger("ReloadPartial");
					yield return new WaitForSeconds(ItemData.ReloadPartialAnimation.length);
					int ammoNeeded = ItemData.MagazineSetting.MagazineCapacity + 1 - CurrentAmmo;
					if (AmmoInInventory() < ammoNeeded) {
						invAmmo = AmmoInInventory();
						RemoveAmmoFromInventory(AmmoInInventory());
						SetCurrentAmmoTo(CurrentAmmo + invAmmo);
					}
					else {
						RemoveAmmoFromInventory(ammoNeeded);
						SetCurrentAmmoTo(ItemData.MagazineSetting.MagazineCapacity + 1);
					}
					break;
				}
				break;
			case ReloadTypes.MagazineOpenBolt:
				// magazine is full
				if (CurrentAmmo >= ItemData.MagazineSetting.MagazineCapacity) break;
				// Magazine is not full
				if (CurrentAmmo < ItemData.MagazineSetting.MagazineCapacity) {
					_viewmodelManager.SetTrigger("ReloadPartial");
					yield return new WaitForSeconds(ItemData.ReloadPartialAnimation.length);
					int ammoNeeded = ItemData.MagazineSetting.MagazineCapacity - CurrentAmmo;
					if (AmmoInInventory() < ammoNeeded) {
						SetCurrentAmmoTo(CurrentAmmo + AmmoInInventory());
						RemoveAmmoFromInventory(AmmoInInventory());
					}
					else {
						SetCurrentAmmoTo(ItemData.MagazineSetting.MagazineCapacity);
						RemoveAmmoFromInventory(ammoNeeded);
					}
				}
				break;
			case ReloadTypes.InternalClosedBolt:
				// Internal is empty and chamber is empty
				if (CurrentAmmo == 0) {
					CurrentAmmo = ItemData.MagazineSetting.MagazineCapacity;
					Debug.Log("Add cancelable reloading and chambering here");
				}
				// One in chamber and internal is not full
				if (CurrentAmmo <= ItemData.MagazineSetting.MagazineCapacity) {
					CurrentAmmo = ItemData.MagazineSetting.MagazineCapacity + 1;
					Debug.Log("Add cancelable reloading here");
				}
				// One in chamber and internal is full
				if (CurrentAmmo > ItemData.MagazineSetting.MagazineCapacity) break;
				break;
			case ReloadTypes.InternalOpenBolt:	
				// Magazine is not full
				if (CurrentAmmo < ItemData.MagazineSetting.MagazineCapacity) {
					CurrentAmmo = ItemData.MagazineSetting.MagazineCapacity;
					Debug.Log("Add cancelable reloading here");
				}
				break;
		}
		_isReloading = false;
	}
	
	private int AmmoInInventory() {
		return _playerInventory.GetItemsWithData(ItemData.AmmoItem).Count;
	}
	
	private bool RemoveAmmoFromInventory(int amount) {
		return _playerInventory.RemoveItemByData(ItemData.AmmoItem, amount);
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
		float angle = CalculateSpreadAngle(ItemData.AccuracySetting.PatternSpread, ItemData.AccuracySetting.EffectiveRange);
		return Random.Range(-angle, angle);
	}
	
	private float CalculateSpreadAngle(float spreadDiameter, float distance) {
		return Mathf.Atan2(spreadDiameter / 2f, distance) * Mathf.Rad2Deg;
	}
	
	private bool ModeAvailable(FireMode mode, FireCombinations combination) {
		return combination switch {
			FireCombinations.Semi or FireCombinations.ShotgunSemi => mode is FireMode.Semi,
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