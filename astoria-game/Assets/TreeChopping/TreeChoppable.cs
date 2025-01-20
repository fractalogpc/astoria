using System;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TreeChoppable : HealthInterface
{
		[SerializeField] private ItemData _woodItem;
		[SerializeField] private float _fallingForce = 1000f;
		private Rigidbody _rigidbody;
		private bool _chopped;

		protected override void Start() {
			base.Start();
			_rigidbody = GetComponent<Rigidbody>();
			_rigidbody.isKinematic = true;
		}
		
		public override void Damage(float damagePoints, Vector3 hitPosition) {
			if (_chopped) return;
			base.Damage(damagePoints, hitPosition);
			if (IsDead) {
				_chopped = true;
				_rigidbody.isKinematic = false;
				NetworkClient.localPlayer.GetComponentInChildren<InventoryComponent>().AddItemByData(_woodItem);
				_rigidbody.AddForceAtPosition(Camera.main.transform.forward * _fallingForce, hitPosition, ForceMode.Impulse);
			}
		}
}