using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

public abstract class GunLogic
{
	public GunInstance GunInstance;

	protected GunLogic(GunInstance gunInstance) {
		GunInstance = gunInstance;
	}
	
	/// <summary>
	/// Called when the logic is first switched to.
	/// </summary>
	public abstract void Initialize();
	
	/// <summary>
	/// Called when the logic is switched away from.
	/// </summary>
	public abstract void Cleanup();
	
	/// <summary>
	/// Called every frame if the logic is active & the gun is equipped.
	/// </summary>
	public abstract void Tick();
	
	/// <summary>
	/// Called when the player presses the fire button & the gun is equipped.
	/// </summary>
	public abstract void OnFireDown();
	
	/// <summary>
	/// Called when the player releases the fire button & the gun is equipped.
	/// </summary>
	public abstract void OnFireUp();
	
	/// <summary>
	/// Called when the player presses the reload button & the gun is equipped.
	/// </summary>
	public abstract void OnReloadDown();
	
	/// <summary>
	/// Called when the player releases the reload button & the gun is equipped.
	/// </summary>
	public abstract void OnReloadUp();
	
	/// <summary>
	/// Called when the player presses the aim button & the gun is equipped.
	/// </summary>
	public abstract void OnAimDown();
	
	/// <summary>
	/// Called when the player releases the aim button & the gun is equipped.
	/// </summary>
	public abstract void OnAimUp();
}