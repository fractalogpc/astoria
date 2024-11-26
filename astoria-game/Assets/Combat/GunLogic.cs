using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public struct Aerodymanics {
	public float DragCoefficient;
	public float CrossSectionalArea;
	public float AirDensity;
	public float _speed;
	public Aerodymanics(float dragCoefficient, float crossSectionalArea, float airDensity, float speed) {
		DragCoefficient = dragCoefficient;
		CrossSectionalArea = crossSectionalArea;
		AirDensity = airDensity;
		_speed = speed;
	}
	public float DragForce => 0.5f * DragCoefficient * CrossSectionalArea * AirDensity * _speed * _speed;
	public Aerodymanics NextAerodynamics(float mass, float deltaTime) {
		float acceleration = DragForce / mass;
		return new Aerodymanics(DragCoefficient, CrossSectionalArea, AirDensity, _speed - acceleration * deltaTime);
	}
}

public struct FireModes
{
	bool SemiAuto;
	bool FullAuto;
}

public class Projectile
{
	public float Damage;
	public float Mass;
	public Vector3 Position;
	public Vector3 Velocity;
	public Aerodymanics Aerodymanics;
	
	public Projectile(float damage, float mass, Vector3 startPosition, Vector3 startVelocity, Aerodymanics aerodymanics) {
		Damage = damage;
		Mass = mass;
		Position = startPosition;
		Velocity = startVelocity;
		Aerodymanics = aerodymanics;
	}
	public RaycastHit TickProjectile(float deltaTime) {
		Aerodymanics = Aerodymanics.NextAerodynamics(Mass, deltaTime);
		Velocity = Aerodymanics._speed * Velocity.normalized;
		Velocity += Physics.gravity * deltaTime;
		Physics.Raycast(Position, Velocity.normalized, out RaycastHit hit, Velocity.magnitude * deltaTime);
		Position += Velocity * deltaTime;
		return hit;
	}
}

public class GunLogic : CombatWeaponLogic
{
	[Header("Projectile Settings")]
	[SerializeField] private int _samplesPerFixedUpdate = 5;
	[Header("Bullet Data")]
	[SerializeField] private float _damage;
	[SerializeField] private float _initialVelocityMS;
	[SerializeField] private float _bulletMassKg;
	[SerializeField] private float _bulletDiameterM;
	[SerializeField] private float _airDensityKgPerM = 1.1f;
	[SerializeField] private float _dragCoefficient;
	[Header("Weapon Settings")]
	public FireModes AllowedFireModes;
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
	[SerializeField] private int _currentAmmo;
	[SerializeField] private bool _openBolt;
	// TODO: Implement Chamber Round

	private bool _triggerDown;
	private float _timeSinceLastShot;
	protected override void InitializeActionMap() {
		base.InitializeActionMap();
		RegisterAction(_inputActions.Player.Reload, ctx => ReloadDown());
		RegisterAction(_inputActions.Player.SwitchFireMode, ctx => SwitchFireMode());
	}

	public override void AttackDown() {
		_triggerDown = true;
		if (!CanFire) return;
		switch (CurrentFireMode) {
			case FireMode.SemiAuto:
				if (_timeSinceLastShot < _minCycleTimeSemiAuto) return;
				Fire();
				break;
			case FireMode.FullAuto:
				if (_timeSinceLastShot < _cycleTimeFullAuto) return;
				Fire();
				break;
		}
	}
	
	public override void AttackUp() {
		_triggerDown = false;
	}

	public override void AttackSecondaryDown() {
		throw new System.NotImplementedException();
	}

	public override void AttackSecondaryUp() {
		throw new System.NotImplementedException();
	}

	public void SwitchFireMode() {
		
	}
	
	public void ReloadDown() {
		_currentAmmo = _magazineCapacity;
	}

	private void Fire() {
		_timeSinceLastShot = 0;
		Projectile newProjectile = new Projectile(
			_damage, 
			_bulletMassKg, 
			transform.position, 
			transform.forward * _initialVelocityMS, 
			new Aerodymanics(
				_dragCoefficient, 
				_bulletDiameterM, 
				_airDensityKgPerM, 
				_initialVelocityMS
			)
		);
		_projectilesToTick.Add(newProjectile);
	}

	private void Update() {
		_timeSinceLastShot += Time.deltaTime / 2;
		if (_triggerDown && CurrentFireMode == FireMode.FullAuto && _timeSinceLastShot > _cycleTimeFullAuto) {
			Fire();
		}
		_timeSinceLastShot += Time.deltaTime / 2;
	}


	private List<Projectile> _projectilesToTick = new List<Projectile>();
	private void FixedUpdate() {
		foreach (Projectile projectile in _projectilesToTick) {
			for (int i = 0; i < _samplesPerFixedUpdate; i++) {
				RaycastHit hit = projectile.TickProjectile(Time.fixedDeltaTime / _samplesPerFixedUpdate);
				if (hit.collider != null) {
					Debug.Log($"Hit {hit.collider.name} at {hit.point}");
					_projectilesToTick.Remove(projectile);
					break;
				}
			}
		}
	}
}