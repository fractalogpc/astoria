using Mirror;
using UnityEngine;
/// <summary>
/// Manages viewmodels. Also acts as an interface for calling each one's animations.
/// </summary>
public class CombatViewmodelManager : NetworkBehaviour
{
	[SerializeField] private Transform _viewmodelParent;
	[SerializeField][ReadOnly] private CombatViewmodel _currentViewmodel;

	public void SetViewmodelFor(GunInstance gunInstance) {
		if (_currentViewmodel != null) {
			Destroy(_currentViewmodel.gameObject);
		}
		_currentViewmodel = Instantiate(gunInstance.WeaponData.ViewmodelPrefab, _viewmodelParent).GetComponent<CombatViewmodel>();
	}
	
	public void RemoveViewmodel() {
		if (_currentViewmodel != null) {
			Destroy(_currentViewmodel.gameObject);
			_currentViewmodel = null;
		}
	}
	
	/// <summary>
	/// Calls the activation function of the relevant animation of the current viewmodel.
	/// </summary>
	/// <returns>Duration of triggered animation in seconds.</returns>
	public float PlayFire() {
		_currentViewmodel.SetTriggerFire();
		return 0.1f;    
	}
	/// <summary>
	/// Calls the activation function of the relevant animation of the current viewmodel.
	/// </summary>
	/// <returns>Duration of triggered animation in seconds.</returns>
	public float PlayReloadEmpty() {
		_currentViewmodel.SetTriggerReloadEmpty();
		return 0.1f;    
	}
	/// <summary>
	/// Calls the activation function of the relevant animation of the current viewmodel.
	/// </summary>
	/// <returns>Duration of triggered animation in seconds.</returns>
	public float PlayReloadPartial() {
		_currentViewmodel.SetTriggerReloadPartial();
		return 0.1f;    
	}
	/// <summary>
	/// Calls the activation function of the relevant animation of the current viewmodel.
	/// </summary>
	/// <returns>Duration of triggered animation in seconds.</returns>
	public float PlayHolster() {
		_currentViewmodel.SetTriggerHolster();
		return 0.1f;    
	}
	/// <summary>
	/// Calls the activation function of the relevant animation of the current viewmodel.
	/// </summary>
	/// <returns>Duration of triggered animation in seconds.</returns>
	public float PlayDraw() {
		_currentViewmodel.SetTriggerDraw();
		return 0.1f;    
	}
}