using System;
using Mirror;
using UnityEngine;

public class ViewmodelManager : NetworkBehaviour
{
	public Type ViewmodelType => _currentViewmodel.GetType();
	
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
	
	// Global Viewmodel Animations
	
	/// <summary>
	/// Calls the unequip trigger of the current viewmodel.
	/// </summary>
	/// <returns>Duration of unequip animation in seconds.</returns>
	public float PlayUnequip() {
		return _currentViewmodel.SetTriggerUnequip(); 
	}
	/// <summary>
	/// Calls the equip trigger of the current viewmodel.
	/// </summary>
	/// <returns>Duration of equip animation in seconds.</returns>
	public float PlayEquip() {
		return _currentViewmodel.SetTriggerEquip(); 
	}
	
	// Tool Viewmodel Animations
	
	/// <summary>
	/// Calls the use trigger of the tool viewmodel. The viewmodel must be of type ToolViewmodel.
	/// </summary>
	/// <returns>The length of the use animation in seconds. -1 if the viewmodel is not a ToolViewmodel.</returns>
	public float PlayToolUse() {
		if (ViewmodelType != typeof(ToolViewmodel)) return -1;
		return ((ToolViewmodel)_currentViewmodel).SetTriggerUse();
	}
	/// <summary>
	/// Calls the use secondary trigger of the tool viewmodel. The viewmodel must be of type ToolViewmodel.
	/// </summary>
	/// <returns>The length of the use secondary animation in seconds. -1 if the viewmodel is not a ToolViewmodel.</returns>
	public float PlayUseSecondary() {
		if (ViewmodelType != typeof(ToolViewmodel)) return -1;
		return ((ToolViewmodel)_currentViewmodel).SetTriggerUseSecondary();
	}
	
	// Combat Viewmodel Animations
	
	/// <summary>
	/// Calls the activation function of the relevant animation of the current viewmodel.
	/// </summary>
	/// <returns>Duration of triggered animation in seconds.</returns>
	public float PlayFire() {
		if (ViewmodelType != typeof(CombatViewmodel)) return -1;
		return ((CombatViewmodel)_currentViewmodel).SetTriggerFire();
	}
	/// <summary>
	/// Calls the activation function of the relevant animation of the current viewmodel.
	/// </summary>
	/// <returns>Duration of triggered animation in seconds.</returns>
	public float PlayReloadEmpty() {
		if (ViewmodelType != typeof(CombatViewmodel)) return -1;
		return ((CombatViewmodel)_currentViewmodel).SetTriggerReloadEmpty(); 
	}
	/// <summary>
	/// Calls the activation function of the relevant animation of the current viewmodel.
	/// </summary>
	/// <returns>Duration of triggered animation in seconds.</returns>
	public float PlayReloadPartial() {
		if (ViewmodelType != typeof(CombatViewmodel)) return -1;
		return ((CombatViewmodel)_currentViewmodel).SetTriggerReloadPartial();
	}
}