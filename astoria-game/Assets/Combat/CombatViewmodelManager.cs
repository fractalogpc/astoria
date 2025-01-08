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
		_currentViewmodel = Instantiate(gunInstance.ItemData.ViewmodelPrefab, _viewmodelParent).GetComponent<CombatViewmodel>();
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
		return _currentViewmodel.SetTriggerFire();
	}
	/// <summary>
	/// Calls the activation function of the relevant animation of the current viewmodel.
	/// </summary>
	/// <returns>Duration of triggered animation in seconds.</returns>
	public float PlayReloadEmpty() {
		return _currentViewmodel.SetTriggerReloadEmpty(); 
	}
	/// <summary>
	/// Calls the activation function of the relevant animation of the current viewmodel.
	/// </summary>
	/// <returns>Duration of triggered animation in seconds.</returns>
	public float PlayReloadPartial() {
		return _currentViewmodel.SetTriggerReloadPartial(); 
	}
	/// <summary>
	/// Calls the activation function of the relevant animation of the current viewmodel.
	/// </summary>
	/// <returns>Duration of triggered animation in seconds.</returns>
	public float PlayHolster() {
		return _currentViewmodel.SetTriggerHolster(); 
	}
	/// <summary>
	/// Calls the activation function of the relevant animation of the current viewmodel.
	/// </summary>
	/// <returns>Duration of triggered animation in seconds.</returns>
	public float PlayDraw() {
		return _currentViewmodel.SetTriggerDraw(); 
	}
}