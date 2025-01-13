using Mirror;
using UnityEngine;
/// <summary>
/// Manages viewmodels. Also acts as an interface for calling each one's animations.
/// </summary>
public class CombatViewmodelManager : ViewmodelManager
{
	private new CombatViewmodel _currentViewmodel => (CombatViewmodel)base._currentViewmodel;
	
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
}