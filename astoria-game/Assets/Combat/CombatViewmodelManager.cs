using Mirror;
using UnityEngine;
/// <summary>
/// Manages and switches between different viewmodels. Also acts as an interface for calling each one's CombatViewmodel.
/// </summary>
public class CombatViewmodelManager : NetworkBehaviour
{
	[SerializeField] private Transform _viewmodelParent;
	[SerializeField][ReadOnly] private CombatViewmodel _currentViewmodel;

	/// <summary>
	/// Sets the current viewmodel to the specified viewmodel. The viewmodel must be a child of the _viewmodelParent.
	/// </summary>
	/// <param name="viewmodelInstance">The viewmodel instance to set to.</param>
	/// <returns>Whether or not setting the viewmodel was successful.</returns>
	public bool SetCurrentViewmodelTo(GameObject viewmodelInstance)
	{
		if (_viewmodelParent.childCount > 0)
		{
			DisableAllViewmodels();
		}
		if (!ViewmodelInstanceExists(viewmodelInstance))
		{
			Debug.LogError($"CombatViewmodelManager: Could not find viewmodel {viewmodelInstance.name}.");
			return false;
		}
		viewmodelInstance.SetActive(true);
		_currentViewmodel = viewmodelInstance.GetComponent<CombatViewmodel>();
		return true;
	}
	public GameObject GetCurrentViewmodel() {
		return _currentViewmodel.gameObject;
	}
	/// <summary>
	/// Instantiates a new viewmodel prefab. The instance starts disabled.
	/// </summary>
	/// <param name="viewmodelPrefab">The viewmodel prefab to instantiate.</param>
	/// <returns>The viewmodel instance.</returns>
	public GameObject AddViewmodel(GameObject viewmodelPrefab) {
		GameObject viewmodel = Instantiate(viewmodelPrefab, _viewmodelParent);
		viewmodel.SetActive(false);
		return viewmodel;
	}
	private bool ViewmodelInstanceExists(GameObject viewmodelInstance) {
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