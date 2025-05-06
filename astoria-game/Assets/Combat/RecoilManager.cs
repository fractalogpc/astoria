using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class RecoilManager : MonoBehaviour
{
	[SerializeField] private Transform _recoilTransform;
	[SerializeField] private float _returnSpeed = 1f;
	[SerializeField] private float _recoilSnappiness = 5f;
	private Quaternion _targetRotation = Quaternion.identity;

	public void ApplyRecoil(RecoilSettings recoilSettings) {
		_targetRotation *= Quaternion.Euler(-recoilSettings.UpwardsKickDeg, Random.Range(-recoilSettings.SidewaysKickRangeDeg, recoilSettings.SidewaysKickRangeDeg), 0);
	}

	private bool MovingMouse(float threshold = 0.1f) {
		return Input.mousePositionDelta.magnitude > threshold;
	}
	
	private void Start() {
		_recoilTransform.localRotation = Quaternion.identity;
	}

	private void Update() {
		_recoilTransform.localRotation = Quaternion.Slerp(_recoilTransform.localRotation, _targetRotation, Time.deltaTime * _recoilSnappiness);
		if (!MovingMouse()) {
			_targetRotation = Quaternion.Slerp(_targetRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * _returnSpeed);
		}
	}

}