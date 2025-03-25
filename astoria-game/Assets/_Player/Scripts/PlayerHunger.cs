using System;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerHunger : PlayerVitalHandler
{
	[Tooltip("The player's health manager.")]
	[FormerlySerializedAs("_healthInterface")] 
	[SerializeField] private HealthManager _healthManager;
	[Tooltip("The maximum amount of food the player can have.")]
	[SerializeField] private float _maxFood;
	[Tooltip("The current amount of food the player has.")]
	[SerializeField] private float _currentFood;
	[Tooltip("The rate at which the player loses food, per second.")]
	[SerializeField] private float _lossRate;
	[Tooltip("The rate at which the player loses health when food is zero, per second.")]
	[SerializeField] private float _healthDamageRate;
	
	public override float GetCurrentValue() {
		return _currentFood;
	}
	public override float GetMaxValue() {
		return _maxFood;
	}
	
	public void AddFood(float amount) {
		_currentFood += amount;
		if (_currentFood > _maxFood) {
			_currentFood = _maxFood;
		}
		NotifyVitalChange(_currentFood - amount, _currentFood, _maxFood);
	}

	private void Update() {
		_currentFood -= _lossRate * Time.deltaTime;
		if (_currentFood <= 0) {
			_currentFood = 0;
			_healthManager.TakeDamage(_healthDamageRate * Time.deltaTime, transform.position);
			return;
		}
		NotifyVitalChange(_currentFood + _lossRate * Time.deltaTime, _currentFood, _maxFood);
	}
}