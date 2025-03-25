using System;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerThirst : PlayerVitalHandler
{
	[Tooltip("The player's health manager.")]
	[FormerlySerializedAs("_healthInterface")] 
	[SerializeField] private HealthManager _healthManager;
	[Tooltip("The maximum amount of water the player can have.")]
	[SerializeField] private float _maxWater;
	[Tooltip("The current amount of water the player has.")]
	[SerializeField] private float _currentWater;
	[Tooltip("The rate at which the player loses water, per second.")]
	[SerializeField] private float _lossRate;
	[Tooltip("The rate at which the player loses health when water is zero, per second.")]
	[SerializeField] private float _healthDamageRate;
	
	public override float GetCurrentValue() {
		return _currentWater;
	}
	public override float GetMaxValue() {
		return _maxWater;
	}
	
	public void AddWater(float amount) {
		_currentWater += amount;
		if (_currentWater > _maxWater) {
			_currentWater = _maxWater;
		}
		NotifyVitalChange(_currentWater - amount, _currentWater, _maxWater);
	}

	private void Update() {
		_currentWater -= _lossRate * Time.deltaTime;
		if (_currentWater <= 0) {
			_currentWater = 0;
			_healthManager.TakeDamage(_healthDamageRate * Time.deltaTime, transform.position);
			return;
		}
		NotifyVitalChange(_currentWater + _lossRate * Time.deltaTime, _currentWater, _maxWater);
	}
}