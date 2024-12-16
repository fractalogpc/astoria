using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

public abstract class FireLogic
{
	public GunInstance Instance;

	protected FireLogic(GunInstance instance) {
		Instance = instance;
	}

	/// <summary>
	/// Called when the logic is first switched to.
	/// </summary>
	public virtual void Initialize() {
			
	}

	/// <summary>
	/// Called when the logic is switched away from.
	/// </summary>
	public virtual void Cleanup() {
		
	}

	/// <summary>
	/// Called every frame if the logic is active & the gun is equipped.
	/// </summary>
	public virtual void Tick() {
		
	}

	/// <summary>
	/// Called when the player presses the fire button & the gun is equipped.
	/// </summary>
	public virtual void OnFireDown() {
		
	}

	/// <summary>
	/// Called when the player releases the fire button & the gun is equipped.
	/// </summary>
	public virtual void OnFireUp() {
		
	}

	/// <summary>
	/// Called when the player presses the aim button & the gun is equipped.
	/// </summary>
	public virtual void OnAimDown() {
		
	}

	/// <summary>
	/// Called when the player releases the aim button & the gun is equipped.
	/// </summary>
	public virtual void OnAimUp() {
		
	}
}