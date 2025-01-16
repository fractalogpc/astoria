using System;
using UnityEngine;

[RequireComponent(typeof(HealthInterface))]
[RequireComponent(typeof(Rigidbody))]
public class TreeChoppable : MonoBehaviour
{
		[SerializeField] private int _startHealth = 100;
		[SerializeField] private float _fallingForce = 1000f;
		private HealthInterface _healthInterface;
		private Rigidbody _rigidbody;
		private bool _chopped;

		private void Start() {
			_healthInterface = GetComponent<HealthInterface>();
			_rigidbody = GetComponent<Rigidbody>();
			_healthInterface.Initialize(_startHealth);
			_healthInterface.OnDamaged.AddListener(OnDamaged);
			_rigidbody.isKinematic = true;
		}
		
		private void OnDamaged(Vector3 hitPosition) {
			if (_chopped) return;
			if (_healthInterface.IsDead) {
				_chopped = true;
				_rigidbody.isKinematic = false;
				_rigidbody.AddForceAtPosition(Vector3.one, hitPosition, ForceMode.Impulse);
			}
		}
}