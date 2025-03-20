using System.Collections;
using FMODUnity;
using Unity.Cinemachine;
using UnityEngine;

public class AxeInstance : BaseToolInstance
{
	public new AxeData ItemData => (AxeData)base.ItemData;
	private float _timeSinceLastSideChop = float.MaxValue;
	private bool _canChop => _timeSinceLastSideChop >= ItemData.SideChopCooldown;
	private Coroutine _chopDelayCoroutine;
	private Coroutine _soundDelayCoroutine;
	
	public AxeInstance(ItemData itemData) : base(itemData) {
	}
	
	public override void OnEquip() {
		base.OnEquip();
	}
	public override void OnUnequip() {
		base.OnUnequip();
		if (_chopDelayCoroutine != null) _toolCore.StopCoroutine(_chopDelayCoroutine);
		// Sound should probably play out
		// if (_soundDelayCoroutine != null) _toolCore.StopCoroutine(_soundDelayCoroutine);
	}

	public override void OnTick() {
		base.OnTick();
		_timeSinceLastSideChop += Time.deltaTime;
	}
	public override void OnUseDown() {
		if (!_canChop) return;
		base.OnUseDown();
		_timeSinceLastSideChop = 0;
		_viewmodelManager.SetTrigger("Use");
		DelayChop(_toolCore, ItemData.UseAnimation.length * ItemData.AnimChopMoment);
		DelaySound(_toolCore, ItemData.SwingSoundDelay, ItemData.SwingSound);
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
	
	private void DelayChop(MonoBehaviour holdingMonobehaviour, float delay) {
		_chopDelayCoroutine = holdingMonobehaviour.StartCoroutine(DelayChopCoroutine(delay));
	}
	private IEnumerator DelayChopCoroutine(float delay) {
		yield return new WaitForSeconds(delay);
		ChopTree(ItemData.ChopRange);
	}
	private void DelaySound(MonoBehaviour holdingMonobehaviour, float delay, EventReference sound) {
		_chopDelayCoroutine = holdingMonobehaviour.StartCoroutine(DelaySoundCoroutine(delay, sound));
	}
	private IEnumerator DelaySoundCoroutine(float delay, EventReference sound) {
		yield return new WaitForSeconds(delay);
		AudioManager.Instance.PlayOneShot(sound, _toolCore.transform.position);
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
		GameObject decal = GameObject.Instantiate(ItemData.WoodHitDecalPrefab, hit.point, Quaternion.LookRotation(-hit.normal));
		decal.transform.SetParent(tree.transform);
		AudioManager.Instance.PlayOneShot(ItemData.HitSound, hit.point);
	}
}
