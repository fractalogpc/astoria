using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class BurstLogic : FireLogic
{
	private float _timeSinceLastBurst;
	
	private MonoBehaviour _coroutineRunner;
	private Coroutine _burstRoutine;
	private bool _firing;
	
	public BurstLogic(GunInstance instance, MonoBehaviour coroutineRunner) : base(instance) {
		_coroutineRunner = coroutineRunner;
	}
	
	public override void Initialize() {
		_timeSinceLastBurst = 0;
		_burstRoutine = null;
	}

	public override void Cleanup() {
		if (_burstRoutine != null) {
			_coroutineRunner.StopCoroutine(_burstRoutine);
		}
	}

	public override void Tick() {
		_timeSinceLastBurst += Time.deltaTime;
	}

	public override void OnFireDown() {
		if (Instance.HasAmmo && !_firing && _timeSinceLastBurst > Instance.ItemData.BurstSetting.CycleTime) {
			_burstRoutine = _coroutineRunner.StartCoroutine(BurstCoroutine());
		}
	}

	private IEnumerator BurstCoroutine() {
		_firing = true;
		int shotsFired = 0;
		while (shotsFired < Instance.ItemData.BurstSetting.ShotsPerBurst) {
			if (!Instance.HasAmmo && !BackgroundInfo._infAmmo) break;
			Instance.Fire();
			shotsFired++;
			yield return new WaitForSeconds(RPMToSeconds(Instance.ItemData.BurstSetting.RoundsPerMinute));
		}
		_firing = false;
	}
}