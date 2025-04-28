using System;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.VFX;

public class TruckExplodeScript : MonoBehaviour
{
	[SerializeField] private VisualEffect _explosionEffect;
	[SerializeField] private EventReference _explosionSound;
	[SerializeField] private List<MeshRenderer> _meshRenders;
	private Material _truckMaterial;
	
	private void Start() {
		SetKinematicsAllRigidbodies(true);
		_truckMaterial = new Material(_meshRenders[0].material);
		foreach (MeshRenderer mesh in _meshRenders) {
			mesh.material = _truckMaterial;
		}
		SetMaterialWhiteColor(_truckMaterial);
	}

	public void Explode() {
		_explosionEffect.Play();
		AudioManager.Instance.PlayOneShotAttached(_explosionSound, gameObject);
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
