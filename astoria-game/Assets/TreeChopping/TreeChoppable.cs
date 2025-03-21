using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TreeChoppable : HealthInterface
{
	[SerializeField] private ItemData _woodItem;
	[SerializeField] private int _amount;
	[SerializeField] private float _fallingForce = 1000f;
	private Rigidbody _rigidbody;
	private bool _chopped;

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
		if (_chopped) return;
		base.Damage(damagePoints, hitPosition);
		if (IsDead)
		{
			_chopped = true;
			_rigidbody.isKinematic = false;
			PlayerInstance.Instance.GetComponentInChildren<InventoryComponent>().AddItemByData(_woodItem, _amount);
			_rigidbody.AddForceAtPosition(Camera.main.transform.forward * _fallingForce, hitPosition + Vector3.up * 100, ForceMode.Impulse);
			// This doesn't work because the force is applied on the first frame and the tree is still in the ground, either we need to fix this or animate it.
			// I also tried adding torque:
			// Vector3 torqueDirection = Vector3.Cross(Vector3.up, Camera.main.transform.forward).normalized;
			// _rigidbody.AddTorque(torqueDirection * _fallingForce, ForceMode.Impulse);
		}
	}
}