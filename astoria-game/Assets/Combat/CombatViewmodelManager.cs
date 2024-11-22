using UnityEngine;
/// <summary>
/// Manages and switches between different viewmodels. Also acts as an interface for calling each one's CombatWeaponViewmodel.
/// </summary>
public class CombatViewmodelManager : MonoBehaviour
{
	[SerializeField] private Transform _viewmodelParent;
	[SerializeField] private CombatWeaponViewmodel _currentViewmodel;

	/// <summary>
	/// Destroys the current viewmodel and instantiates a new one.
	/// </summary>
	/// <param name="viewmodelPrefab">The viewmodel to instantiate.</param>
	/// <returns>A reference to the instantiated viewmodel.</returns>
	public GameObject SetWeaponViewmodel(GameObject viewmodelPrefab)
	{
		if (_viewmodelParent.childCount > 0)
		{
			Object.Destroy(_viewmodelParent.GetChild(0).gameObject);
		}
		GameObject viewmodel = Instantiate(viewmodelPrefab, _viewmodelParent);
		_currentViewmodel = viewmodel.GetComponent<CombatWeaponViewmodel>();
		return viewmodel;
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