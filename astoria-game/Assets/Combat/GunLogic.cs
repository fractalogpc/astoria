using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

public struct AllowedModes
{
	bool SemiAuto;
	bool FullAuto;
}

public class GunLogic : CombatWeaponLogic
{
	[Header("Refs")]
	[SerializeField] private ProjectileManager _projectileManager;
	[Header("Projectile Settings")]
	[SerializeField] private int _samplesPerFixedUpdate = 5;
	[Header("Bullet Data")]
	[SerializeField] private float _damage = 30f;
	[SerializeField] private float _initialVelocityMS = 400f;
	[SerializeField] private float _bulletMassKg = 0.007f;
	[SerializeField] private float _bulletDiameterM = 0.009f;
	[SerializeField] private float _airDensityKgPerM = 1.1f;
	[SerializeField] private float _dragCoefficient = 0.149f;
	[Header("Weapon Settings")]
	public AllowedModes AllowedFireModes;
	public FireMode CurrentFireMode;
	public enum FireMode {
		SemiAuto,
		FullAuto
	}
	[Header("SemiAuto Settings")]
	[SerializeField] private float _minCycleTimeSemiAuto;
	[Header("FullAuto Settings")]
	[SerializeField] private float _cycleTimeFullAuto;
	[Header("Recoil Settings")]
	[SerializeField] private float _recoilForce;
	[Header("Ammo Settings")]
	public bool CanFire => _currentAmmo > 0;
	[SerializeField] private int _magazineCapacity;
	[SerializeField][ReadOnly] private int _currentAmmo;
	[SerializeField] private bool _openBolt;
	// TODO: Implement Chamber Round
	[Header("Events")]
	public UnityEvent OnFire;

	private bool _triggerDown;
	private float _timeSinceLastShot;
	
	protected override void InitializeActionMap() {
		base.InitializeActionMap();
		RegisterAction(_inputActions.Player.Reload, ctx => ReloadDown());
		RegisterAction(_inputActions.Player.SwitchFireMode, ctx => SwitchFireMode());
	}

	protected override void AttackDown() {
		print("called attack down");
		_triggerDown = true;
		if (!CanFire) return;
		switch (CurrentFireMode) {
			case FireMode.SemiAuto:
				if (_timeSinceLastShot < _minCycleTimeSemiAuto) return;
				Fire();
				break;
			case FireMode.FullAuto:
				// Handled by Update()
				break;
		}
	}
	
	protected override void AttackUp() {
		_triggerDown = false;
	}

	protected override void AttackSecondaryDown() {
		throw new System.NotImplementedException();
	}

	protected override void AttackSecondaryUp() {
		throw new System.NotImplementedException();
	}

	protected void SwitchFireMode() {
		CurrentFireMode = CurrentFireMode == FireMode.SemiAuto ? FireMode.FullAuto : FireMode.SemiAuto;
	}
	
	protected void ReloadDown() {
		print("reloading");
		_currentAmmo = _magazineCapacity;
	}

	protected override void OnValidate() {
		base.OnValidate();
		Debug.LogWarning("Ignore the previous error. The NetworkIdentity will be on the parents of GunLogic.");
	}

	private void Start() {
		Debug.Log("Replace magazine capacity here with an actual magazine item later");
		_projectileManager = ProjectileManager.Instance;
	}

	private void Fire() {
		_timeSinceLastShot = 0;
		_currentAmmo--;
		_projectileManager.FireProjectile(
			_damage, 
			_bulletMassKg, 
			Camera.main.transform.position, 
			Camera.main.transform.forward * _initialVelocityMS, 
			new ProjectileManager.Aerodynamics(
				_dragCoefficient, 
				Mathf.Pow(_bulletDiameterM / 2, 2) * Mathf.PI, 
				_airDensityKgPerM
			),
			OnBulletHit
		);
		OnFire?.Invoke();
	}

	private void OnBulletHit(RaycastHit hit) {
		Debug.Log($"Hit {hit.collider.name} at {hit.point}");
	}

	private void Update() {
		_timeSinceLastShot += Time.deltaTime / 2;
		if (_triggerDown && CurrentFireMode == FireMode.FullAuto && _timeSinceLastShot > _cycleTimeFullAuto && CanFire) {
			Fire();
		}
		_timeSinceLastShot += Time.deltaTime / 2;
	}
}