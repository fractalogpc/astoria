using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class AxeInstance : BaseToolInstance
{
	public new AxeData ItemData => (AxeData)base.ItemData;
	private float _timeSinceLastSideChop = 0;
	private float _timeSinceLastDownChop = 0;
	private bool _canChop => _timeSinceLastSideChop >= ItemData.SideChopCooldown && _timeSinceLastDownChop >= ItemData.DownChopCooldown;
	private Coroutine _chopDelayCoroutine;
	
	public AxeInstance(ItemData itemData) : base(itemData) {
	}
	
	public override void OnEquip() {
		base.OnEquip();
	}
	public override void OnUnequip() {
		base.OnUnequip();
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
		if (!Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out RaycastHit hit, 4f)) return;
		if (TreeChopping.Instance == null) Debug.LogError("AxeInstance: TreeChopping instance not found!");
		GameObject tree = TreeChopping.Instance.RealizeTree(hit.point);
		if (tree == null) return;
		tree.GetComponentInChildren<HealthInterface>().Damage(ItemData.ChopDamage, hit.point);
	}
}
