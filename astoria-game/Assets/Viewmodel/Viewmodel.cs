using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Viewmodel : MonoBehaviour
{
	[SerializeField] protected Animator _animator;
	[SerializeField] protected Transform _itemHolder;
	[SerializeField] private Transform _adsIkTarget;
	[SerializeField] private Rig _adsRig;
	
	public void SetTrigger(string triggerName) {
		_animator.SetTrigger(triggerName);
	}
	
	public void SetItemTo(ViewmodelItemInstance item) {
		if (_itemHolder.childCount > 0) {
			Destroy(_itemHolder.GetChild(0).gameObject);
		}
		Instantiate(item.ItemData.HeldItemPrefab, _itemHolder);
		_animator.runtimeAnimatorController = item.ItemData.ItemAnimatorController;
		if (typeof(GunData).IsInstanceOfType(item.ItemData)) {
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
	
	public void UnsetItem() {
		if (_itemHolder.childCount > 0) {
			DestroyAllChildren(_itemHolder);
		}
	}
	
	private void DestroyAllChildren(Transform parent) {
		for (int i = parent.childCount - 1; i >= 0; i--) {
			Destroy(parent.GetChild(i).gameObject);
		}
	}
	
	
}