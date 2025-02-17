using System;
using UnityEngine;

public class RagdollToggle : MonoBehaviour
{
	[SerializeField] private Animator _animator;
	[SerializeField] private Rigidbody _rootRigidbody;
	[SerializeField] private Collider _rootCollider;
	
	public void Ragdoll(bool ragdoll) {
		_animator.enabled = !ragdoll;
		_rootCollider.enabled = !ragdoll;
		foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>()) {
			rb.isKinematic = !ragdoll;
		}
		foreach (Collider col in GetComponentsInChildren<Collider>()) {
			col.enabled = ragdoll;
		}
	}
	public void ApplyForce(Vector3 force) {
		_rootRigidbody.AddForce(force, ForceMode.Impulse);
	}

	private void Start() {
		Ragdoll(false);
	}
}
