using UnityEngine;
/// <summary>
/// Manages and switches between different viewmodels. Also acts as an interface for calling each one's CombatWeaponViewmodel.
/// </summary>
public class CombatViewmodelManager : MonoBehaviour
{
	[SerializeField] private Transform _viewmodelParent;
	[SerializeField] private CombatWeaponViewmodel _currentViewmodel;

	/// <summary>
	/// Sets the current viewmodel to the specified viewmodel. The viewmodel must be a child of the _viewmodelParent.
	/// </summary>
	/// <param name="viewmodelPrefab">The viewmodel instance to set to.</param>
	/// <returns>Whether or not setting the viewmodel was successful.</returns>
	public bool SetCurrentViewmodel(GameObject viewmodelPrefab)
	{
		if (_viewmodelParent.childCount > 0)
		{
			DisableAllViewmodels();
		}
		GameObject viewmodel = _viewmodelParent.Find(viewmodelPrefab.name)?.gameObject;
		_currentViewmodel = viewmodel.GetComponent<CombatWeaponViewmodel>();
		return viewmodel;
	}
	public GameObject GetCurrentViewmodel() {
		return _currentViewmodel.gameObject;
	}
	public GameObject AddNewViewmodel(GameObject viewmodelPrefab) {
		GameObject viewmodel = Instantiate(viewmodelPrefab, _viewmodelParent);
		return viewmodel;
	}
	
	private bool ViewmodelExists(GameObject viewmodelInstance) {
		foreach (Transform child in _viewmodelParent) {
			if (child.gameObject == viewmodelInstance) {
				return true;
			}
		}
		return false;
	}
	private void DisableAllViewmodels() {
		foreach (Transform child in _viewmodelParent) {
			child.gameObject.SetActive(false);
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