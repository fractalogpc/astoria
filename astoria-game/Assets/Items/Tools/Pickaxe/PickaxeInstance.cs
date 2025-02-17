using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class PickaxeInstance : BaseToolInstance
{
	public new PickaxeData ItemData => (PickaxeData)base.ItemData;
	private float _timeSinceLastSwing = float.MaxValue;
	private bool _canSwing => _timeSinceLastSwing >= ItemData.SwingCooldown;
	private Coroutine _swingDelayCoroutine;
	
	public PickaxeInstance(ItemData itemData) : base(itemData) {
	}
	
	public override void OnEquip() {
		base.OnEquip();
	}
	public override void OnUnequip() {
		base.OnUnequip();
		_toolCore.StopCoroutine(_swingDelayCoroutine);
	}

	public override void OnTick() {
		base.OnTick();
		_timeSinceLastSwing += Time.deltaTime;
	}
	public override void OnUseDown() {
		if (!_canSwing) return;
		base.OnUseDown();
		_timeSinceLastSwing = 0;
		_viewmodelManager.PlayToolUse();
		DelaySwing(_toolCore, ItemData.SwingCooldown);
	}
	public override void OnUseUp() {
		base.OnUseUp();
	}
	public override void OnUseHold() {
		base.OnUseHold();
	}
	public override void OnAltUseDown() {
		
	}
	public override void OnAltUseUp() {
		base.OnAltUseUp();
	}
	public override void OnAltUseHold() {
		base.OnAltUseHold();
	}
	
	private void DelaySwing(MonoBehaviour holdingMonobehaviour, float delay) {
		_swingDelayCoroutine = holdingMonobehaviour.StartCoroutine(DelaySwingCoroutine(delay));
	}

	private IEnumerator DelaySwingCoroutine(float delay) {
		yield return new WaitForSeconds(delay);
		HarvestRock(ItemData.SwingRange);
	}
	private void HarvestRock(float range) {
		Camera mainCamera = Camera.main;
		Debug.Log("E");
		if (mainCamera == null) Debug.LogError("PickaxeInstance: Main camera not found!");
		// Check for LOS
		if (!Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out RaycastHit hit, range)) return;
		if (hit.collider.gameObject == null) return;
		GameObject rock = hit.collider.gameObject;
		HarvestableRock harvestableRock = rock.GetComponentInChildren<HarvestableRock>();
		if (harvestableRock == null) return;
		harvestableRock.Damage(ItemData.SwingDamage, hit.point);
	}
}
