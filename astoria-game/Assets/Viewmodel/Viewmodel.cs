using System;
using System.Collections;
using Mirror;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Viewmodel : MonoBehaviour
{
	[SerializeField] protected Animator _viewmodelAnimator;
	[SerializeField][ReadOnly] protected Animator _itemAnimator;
	[SerializeField] protected Transform _itemHolder;
	[SerializeField] protected Transform _animatedItemHolder;
	[SerializeField] private RuntimeAnimatorController _defaultController;
	[SerializeField] private AnimationClip _defaultAnimation;
	[SerializeField] private AnimationClip _interactAnimation;
	[SerializeField] private Transform _adsIkTarget;
	[SerializeField] private Rig _adsRig;
	[SerializeField] private bool _loadCutsceneAnimator = false;
	[SerializeField] private RuntimeAnimatorController _cutsceneController;

	private RuntimeAnimatorController _holdingViewmodelAnimator;
	private Coroutine _interactAnimCoroutine;
	private bool _holdingItem;
	
	public void SetTrigger(string triggerName) {
		if (_interactAnimCoroutine != null) {
			StopCoroutine(_interactAnimCoroutine);
		}
		_viewmodelAnimator.runtimeAnimatorController = _holdingViewmodelAnimator;
		_viewmodelAnimator.SetTrigger(triggerName);
		if (_itemAnimator == null) return;
		_itemAnimator.SetTrigger(triggerName);
	}

	public void InteractAnimation() {
		if (_interactAnimCoroutine != null) {
			StopCoroutine(_interactAnimCoroutine);
		}
		_interactAnimCoroutine = StartCoroutine(InteractAnimationCoroutine());
	}
	
	public void SetItemTo(ViewmodelItemInstance item) {
		_holdingItem = true;
		if (_itemHolder.childCount > 0) {
			Destroy(_itemHolder.GetChild(0).gameObject);
		}
		_holdingViewmodelAnimator = item.ItemData.ViewmodelAnimatorController;
		_viewmodelAnimator.runtimeAnimatorController = _holdingViewmodelAnimator;
		if (ItemAnimationsValid(item)) {
			GameObject itemPrefab = Instantiate(item.ItemData.HeldItemPrefab, _animatedItemHolder);
			_itemAnimator = itemPrefab.GetComponentInChildren<Animator>();
			_itemAnimator.runtimeAnimatorController = item.ItemData.ItemAnimations;
			itemPrefab.transform.SetParent(_animatedItemHolder);
			itemPrefab.transform.localPosition = Vector3.zero;
		}
		else {
			Instantiate(item.ItemData.HeldItemPrefab, _itemHolder);
			_itemAnimator = null;
		}
		if (item.ItemData is GunData) {
			// Set IK target for ADS
			item._viewmodel = this;
			_adsIkTarget.localPosition = item.GetGunItemData().AdsIkTarget;
			_adsIkTarget.localRotation = Quaternion.Euler(item.GetGunItemData().AdsIkTargetRot);
		}
	}

	public void EnableAds(float transitionTime) {
		StopCoroutine("LerpAds");
		StartCoroutine(LerpAds(transitionTime, true));
	}

	public void DisableAds(float transitionTime) {
		StopCoroutine("LerpAds");
		StartCoroutine(LerpAds(transitionTime, false));
	}
	
	public void UnsetItem() {
		_holdingItem = false;
		_holdingViewmodelAnimator = null;
		_viewmodelAnimator.runtimeAnimatorController = _defaultController;
		if (_itemHolder.childCount > 0) {
			DestroyAllChildren(_itemHolder);
		}
		if (_animatedItemHolder.childCount > 0) {
			DestroyAllChildren(_animatedItemHolder);
		}
	}

	private void Start() {
		if (_loadCutsceneAnimator) {
			_viewmodelAnimator.runtimeAnimatorController = _cutsceneController;
			return;
		}
		_viewmodelAnimator.runtimeAnimatorController = _defaultController;
	}

	public void LoadDefaultAnimator() {
		_viewmodelAnimator.runtimeAnimatorController = _defaultController;
	}

	private IEnumerator InteractAnimationCoroutine() {
		_viewmodelAnimator.runtimeAnimatorController = _defaultController;
		_viewmodelAnimator.SetTrigger("Interact");
		yield return new WaitForSeconds(_interactAnimation.length);
		_viewmodelAnimator.runtimeAnimatorController = _holdingItem ? _holdingViewmodelAnimator : _defaultController;
	}
	
	private IEnumerator LerpAds(float transitionTime, bool ads) {
		float elapsedTime = 0;
		float start = ads ? 0 : 1;
		float end = ads ? 1 : 0;
		while (elapsedTime < transitionTime) {
			elapsedTime += Time.deltaTime;
			// Lerp the value
			_adsRig.weight = Mathf.Lerp(start, end, elapsedTime / transitionTime);
			yield return null;
		}
		_adsRig.weight = end;
	}
	
	private bool ItemAnimationsValid(ViewmodelItemInstance item) {
		if (item == null) return false;
		if (item.ItemData.ItemAnimations == null) return false;
		if (item.ItemData.HeldItemPrefab.GetComponentInChildren<Animator>() == null) return false;
		return true;
	}
	
	private void DestroyAllChildren(Transform parent) {
		for (int i = parent.childCount - 1; i >= 0; i--) {
			Destroy(parent.GetChild(i).gameObject);
		}
	}
}