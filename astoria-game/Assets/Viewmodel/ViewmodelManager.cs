using System;
using Mirror;
using UnityEngine;
using KinematicCharacterController;
using Player;

// public class ViewmodelManager : NetworkBehaviour
public class ViewmodelManager : MonoBehaviour {
	[Tooltip("The transform all viewmodels will be parented to.")]
	[SerializeField] protected Transform _viewmodelParent;
	[Tooltip("The current viewmodel being used.")]
	[SerializeField][Mirror.ReadOnly] protected Viewmodel _currentViewmodel;
	[Tooltip("The current item being held.")]
	[SerializeField] protected GameObject _currentItem;
	
	public void SetViewmodelTo(GameObject viewmodelPrefab) {
		RemoveViewmodel();
		_currentViewmodel = Instantiate(viewmodelPrefab, _viewmodelParent).GetComponent<Viewmodel>();
		if (_currentItem != null) {
			_currentViewmodel.SetItemTo(_currentItem);
		}
	}

	public bool SetItemTo(GameObject itemPrefab) {
		if (_currentViewmodel == null) return false;
		_currentViewmodel.SetItemTo(itemPrefab);
		return true;
	}
	
	public bool UnsetItem() {
		if (_currentViewmodel == null) return false;
		_currentViewmodel.UnsetItem();
		return true;
	}
	
	public void RemoveViewmodel() {
		if (_currentViewmodel == null) return;
		Destroy(_currentViewmodel.gameObject);
		_currentViewmodel = null;
	}

	public void PlayAnimation(AnimationClip clip) {
		_currentViewmodel.PlayAnimation(clip);
	}
}