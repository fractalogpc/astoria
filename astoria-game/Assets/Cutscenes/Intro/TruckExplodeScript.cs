using System;
using UnityEngine;
using UnityEngine.VFX;

public class TruckExplodeScript : MonoBehaviour
{
	[SerializeField] private VisualEffect _explosionEffect;
	[SerializeField] private Material _truckMaterial;
	
	private void Start() {
		SetKinematicsAllRigidbodies(true);
		SetMaterialWhiteColor(_truckMaterial);
	}

	public void Explode() {
		_explosionEffect.Play();
		SetKinematicsAllRigidbodies(false);
		SetMaterialBlackColor(_truckMaterial);

		Invoke(nameof(InvokeKinematic), 2f);
	}
	
	private void SetKinematicsAllRigidbodies(bool isKinematic) {
		Debug.Log("Set Kinematics: " + isKinematic);
		Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
		foreach (Rigidbody rb in rigidbodies) {
			rb.isKinematic = isKinematic;
		}
	}

	private void InvokeKinematic() {
		SetKinematicsAllRigidbodies(true);
	}
	
	private void SetMaterialBlackColor(Material material) {
		material.SetColor("_BaseColor", Color.black);
	}
	private void SetMaterialWhiteColor(Material material) {
		material.SetColor("_BaseColor", Color.white);
	}
}
