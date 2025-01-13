using Mirror;
using UnityEngine;

public class ViewmodelManager : NetworkBehaviour
{
	[SerializeField] protected Transform _viewmodelParent;
	[SerializeField][ReadOnly] protected Viewmodel _currentViewmodel;
	
	public void SetViewmodelFor(ViewmodelItemInstance itemInstance) {
		if (_currentViewmodel != null) {
			Destroy(_currentViewmodel.gameObject);
		}
		_currentViewmodel = Instantiate(itemInstance.ItemData.ViewmodelPrefab, _viewmodelParent).GetComponent<Viewmodel>();
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
	public float PlayUnequip() {
		return _currentViewmodel.SetTriggerUnequip(); 
	}
	/// <summary>
	/// Calls the activation function of the relevant animation of the current viewmodel.
	/// </summary>
	/// <returns>Duration of triggered animation in seconds.</returns>
	public float PlayEquip() {
		return _currentViewmodel.SetTriggerEquip(); 
	}
}