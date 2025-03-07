using UnityEngine;

public class Viewmodel : MonoBehaviour
{
	[SerializeField] protected Animator _animator;
	[SerializeField] protected Transform _itemHolder;
	
	public void PlayAnimation(AnimationClip clip) {
		_animator.Play(clip.name);
	}
	
	public void SetItemTo(GameObject itemPrefab) {
		if (_itemHolder.childCount > 0) {
			Destroy(_itemHolder.GetChild(0).gameObject);
		}
		Instantiate(itemPrefab, _itemHolder);
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