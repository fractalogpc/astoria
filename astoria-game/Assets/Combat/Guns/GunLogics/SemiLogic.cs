using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class SemiLogic : FireLogic
{
	private float _timeSinceLastShot;
	
	public SemiLogic(GunInstance instance) : base(instance) {
	}
	
	public override void Initialize() {
		_timeSinceLastShot = 0;
	}

	public override void Tick() {
		_timeSinceLastShot += Time.deltaTime;
	}

	public override void OnFireDown() {
		if (!Instance.HasAmmo) return;
		if (_timeSinceLastShot < Instance.WeaponData.SemiAutoSetting.CycleTime) return;
		Instance.Fire();
	}
}