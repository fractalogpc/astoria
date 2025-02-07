using System;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TreeChoppable : Harvestable
{
		[SerializeField] private float _fallingForce = 1000f;
		private Rigidbody _rigidbody;

		protected override void Start() {
			_rigidbody = GetComponent<Rigidbody>();
			_rigidbody.isKinematic = true;
		}
		
		public override void Hit(Vector3 hitPosition) {
			bool deadThisHit = base.Hit(damagePoints, hitPosition, criticalHit);
			if (IsHarvested) {
				
			}
			return deadThisHit;
		}

		protected override void HarvestedEffects(Vector3 hitPosition) {
			base.HarvestedEffects();
			_rigidbody.isKinematic = false;
			_rigidbody.AddForceAtPosition(Camera.main.transform.forward * _fallingForce, hitPosition, ForceMode.Impulse);
		}
}