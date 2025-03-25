using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TreeChoppable : HealthManager
{
	[SerializeField] private ItemData _woodItem;
	[SerializeField] private int _dropAmountOnFell = 10;
	[SerializeField] private float _healthOnFell = 200f;
	[SerializeField] private float _damagePerDrop = 20f;
	[SerializeField] private float _fallingForce = 1000f;
	private Rigidbody _rigidbody;
	private bool _chopped;
	private float _damageAccountedFor = 0f;

	protected override void Start()
	{
		base.Start();
		_rigidbody = GetComponent<Rigidbody>();
		_rigidbody.isKinematic = true;

		if (_woodItem == null)
		{
			Debug.LogError("Wood item is not set in " + name);
		}
	}

	public override void Damage(float damagePoints, Vector3 hitPosition)
	{
		base.Damage(damagePoints, hitPosition);
		if (_chopped) {
			
			float damageUnaccountedFor = _healthOnFell - CurrentHealth - _damageAccountedFor;
			for (int i = 0; i < damageUnaccountedFor / _damagePerDrop; i++) {
				PlayerInstance.Instance.GetComponentInChildren<InventoryComponent>().AddItemByData(_woodItem);
				_damageAccountedFor += _damagePerDrop;
				Debug.Log("Added wood item");
			}
			if (IsDead)
			{
				Destroy(gameObject);
				return;
			}
		}
		if (IsDead && !_chopped)
		{
			_chopped = true;
			_rigidbody.isKinematic = false;
			PlayerInstance.Instance.GetComponentInChildren<InventoryComponent>().AddItemByData(_woodItem, _dropAmountOnFell);
			_rigidbody.AddForceAtPosition(Camera.main.transform.forward * _fallingForce, hitPosition + Vector3.up * 100, ForceMode.Impulse);
			base.SetHealthDirect(_healthOnFell);
			// This doesn't work because the force is applied on the first frame and the tree is still in the ground, either we need to fix this or animate it.
			// I also tried adding torque:
			// Vector3 torqueDirection = Vector3.Cross(Vector3.up, Camera.main.transform.forward).normalized;
			// _rigidbody.AddTorque(torqueDirection * _fallingForce, ForceMode.Impulse);
		}
	}
}