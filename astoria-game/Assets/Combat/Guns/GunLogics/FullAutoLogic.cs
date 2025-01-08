using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class FullAutoLogic : FireLogic
{
	private float _timeSinceLastShot;
	private bool _triggerDown;
	
	public FullAutoLogic(GunInstance instance) : base(instance) {
	}
	
	public override void Initialize() {
		_timeSinceLastShot = 0;
		_triggerDown = false;
	}

	public override void Tick() {
		if (!Instance.HasAmmo) return;
		if (!_triggerDown) return;
		if (_timeSinceLastShot < RPMToSeconds(Instance.ItemData.FullAutoSetting.RoundsPerMinute)) return;
		Instance.Fire();
	}

	public override void OnFireDown() {
		_triggerDown = true;
	}

	public override void OnFireUp() {
		_triggerDown = false;
	}
}