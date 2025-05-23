﻿using System;
using Mirror;
using UnityEngine;
using KinematicCharacterController;
using Player;

// public class ViewmodelManager : NetworkBehaviour
public class ViewmodelManager : MonoBehaviour {
	[Tooltip("The transform all viewmodels will be parented to.")]
	[SerializeField] protected Transform _viewmodelParent;
	[Tooltip("The current viewmodel being used.")]
	[SerializeField] protected Viewmodel _currentViewmodel;
	[Tooltip("The current item being held.")]
	[SerializeField] protected ViewmodelItemInstance _currentItem;
	
	public void SetViewmodelTo(GameObject viewmodelPrefab) {
		RemoveViewmodel();
		_currentViewmodel = Instantiate(viewmodelPrefab, _viewmodelParent).GetComponent<Viewmodel>();
		if (_currentItem != null) {
			_currentViewmodel.SetItemTo(_currentItem);
		}
	}

	public bool SetItemTo(ViewmodelItemInstance item) {
		if (_currentViewmodel == null) return false;
		_currentViewmodel.SetItemTo(item);
		_currentItem = item;
		return true;
	}
	
	public bool UnsetItem() {
		if (_currentViewmodel == null) return false;
		_currentViewmodel.UnsetItem();
		_currentItem = null;
		return true;
	}
	
	public void RemoveViewmodel() {
		if (_currentViewmodel == null) return;
		Destroy(_currentViewmodel.gameObject);
		_currentViewmodel = null;
		_currentItem = null;
	}
	
	public void SetTrigger(string triggerName) {
		_currentViewmodel.SetTrigger(triggerName);
	}

	public void InteractAnimation() {
		_currentViewmodel.InteractAnimation();
	}
}