using Mirror;
using UnityEngine;

public class ViewmodelManager : MonoBehaviour
{
	[SerializeField] private Transform _viewmodelParent;
	[SerializeField][ReadOnly] private Viewmodel _currentViewmodel;
	
	public void SetViewmodelFor(ItemInstance ) {
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