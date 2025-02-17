using System;
using UnityEngine;

public class RagdollToggle : MonoBehaviour
{
	[SerializeField] private Animator _animator;
	[SerializeField] private Rigidbody _rootRigidbody;
	
	public void Ragdoll(bool ragdoll) {
		_animator.enabled = !ragdoll;
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
