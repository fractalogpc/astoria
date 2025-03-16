using UnityEngine;

public class Viewmodel : MonoBehaviour
{
	[SerializeField] protected Animator _animator;
	[SerializeField] protected Transform _itemHolder;
	
	public void SetTrigger(string triggerName) {
		_animator.SetTrigger(triggerName);
	}
	
	public void SetItemTo(ViewmodelItemInstance item) {
		if (_itemHolder.childCount > 0) {
			Destroy(_itemHolder.GetChild(0).gameObject);
		}
		Instantiate(item.ItemData.HeldItemPrefab, _itemHolder);
		_animator.runtimeAnimatorController = item.ItemData.ItemAnimatorController;
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