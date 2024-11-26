using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public struct Aerodynamics {
	public float DragCoefficient;
	public float CrossSectionalArea;
	public float AirDensity;
	public Aerodynamics(float dragCoefficient, float crossSectionalArea, float airDensity) {
		DragCoefficient = dragCoefficient;
		CrossSectionalArea = crossSectionalArea;
		AirDensity = airDensity;
	}
}

public struct AllowedModes
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
	public Aerodynamics Aerodynamics;
	
	public Projectile(float damage, float mass, Vector3 startPosition, Vector3 startVelocity, Aerodynamics aerodynamics) {
		Damage = damage;
		Mass = mass;
		Position = startPosition;
		Velocity = startVelocity;
		Aerodynamics = aerodynamics;
	}
	public RaycastHit TickProjectile(float deltaTime) {
		// Calculate drag force direction
		Vector3 dragForceDirection = -Velocity.normalized;
		// Calculate drag force magnitude
		float dragForceMagnitude = 0.5f * Aerodynamics.DragCoefficient * Aerodynamics.CrossSectionalArea * Aerodynamics.AirDensity * Velocity.sqrMagnitude;
		// Compute drag acceleration
		Vector3 dragAcceleration = dragForceDirection * (dragForceMagnitude / Mass);

		// Update velocity considering both drag and gravity
		Velocity += (dragAcceleration + Physics.gravity) * deltaTime;

		// Update position based on new velocity
		Position += Velocity * deltaTime;

		// Handle collision
		Physics.Raycast(Position, Velocity.normalized, out RaycastHit hit, Velocity.magnitude * deltaTime);
		Debug.DrawLine(Position, Position + Velocity * deltaTime, Color.red, 0.1f);
		return hit;
	}
}

public class GunLogic : CombatWeaponLogic
{
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

	private void Start() {
		Debug.Log("Replace magazine capacity here with an actual magazine item later");
	}

	private void Fire() {
		_timeSinceLastShot = 0;
		_currentAmmo--;
		Projectile newProjectile = new Projectile(
			_damage, 
			_bulletMassKg, 
			Camera.main.transform.position, 
			Camera.main.transform.forward * _initialVelocityMS, 
			new Aerodynamics(
				_dragCoefficient, 
				Mathf.PI * Mathf.Pow(_bulletDiameterM / 2, 2), 
				_airDensityKgPerM
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