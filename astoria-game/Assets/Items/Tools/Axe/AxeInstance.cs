using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class AxeInstance : BaseToolInstance
{
	public new AxeData ItemData => (AxeData)base.ItemData;
	private float _timeSinceLastSideChop = float.MaxValue;
	private float _timeSinceLastDownChop = float.MaxValue;
	private bool _canChop => _timeSinceLastSideChop >= ItemData.SideChopCooldown && _timeSinceLastDownChop >= ItemData.DownChopCooldown;
	private Coroutine _chopDelayCoroutine;
	
	public AxeInstance(ItemData itemData) : base(itemData) {
	}
	
	public override void OnEquip() {
		base.OnEquip();
	}
	public override void OnUnequip() {
		base.OnUnequip();
		_toolCore.StopCoroutine(_chopDelayCoroutine);
	}

	public override void OnTick() {
		base.OnTick();
		_timeSinceLastSideChop += Time.deltaTime;
		_timeSinceLastDownChop += Time.deltaTime;
	}
	public override void OnUseDown() {
		if (!_canChop) return;
		base.OnUseDown();
		_timeSinceLastSideChop = 0;
		_viewmodelManager.PlayToolUse();
		DelayChop(_toolCore, ItemData.SideChopCooldown);
	}
	public override void OnUseUp() {
		base.OnUseUp();
	}
	public override void OnUseHold() {
		base.OnUseHold();
	}
	public override void OnAltUseDown() {
		if (!_canChop) return;
		base.OnAltUseDown();
		_timeSinceLastDownChop = 0;
		_viewmodelManager.PlayUseSecondary();
	}
	public override void OnAltUseUp() {
		base.OnAltUseUp();
	}
	public override void OnAltUseHold() {
		base.OnAltUseHold();
	}
	
	private void DelayChop(MonoBehaviour holdingMonobehaviour, float delay) {
		_chopDelayCoroutine = holdingMonobehaviour.StartCoroutine(DelayChopCoroutine(delay));
	}

	private IEnumerator DelayChopCoroutine(float delay) {
		yield return new WaitForSeconds(delay);
		ChopTree(ItemData.ChopRange);
	}
	private void ChopTree(float range) {
		Camera mainCamera = Camera.main;
		if (mainCamera == null) Debug.LogError("AxeInstance: Main camera not found!");
		// Check for LOS
		if (!Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out RaycastHit hit, range)) return;
		if (hit.collider.gameObject == null) return;
		GameObject tree;
		if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Tree")) {
			Debug.Log("Hit tree layer object: " + hit.collider.gameObject.name);
			if (TreeChopping.Instance == null) Debug.LogError("AxeInstance: TreeChopping instance not found!");
			tree = TreeChopping.Instance.RealizeTree(hit.point);
		}
		else {
			Debug.Log("Hit non tree layer object: " + hit.collider.gameObject.name);
			tree = hit.collider.gameObject;
		}
		TreeChoppable treeChoppable = tree.GetComponentInChildren<TreeChoppable>();
		if (treeChoppable == null) return;
		treeChoppable.Damage(ItemData.ChopDamage, hit.point);
		
	}
}
