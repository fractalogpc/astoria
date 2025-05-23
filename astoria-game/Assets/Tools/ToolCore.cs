﻿using System;
using UnityEngine;

public class ToolCore : InputHandlerBase
{
	public static ToolCore Instance; // Singleton
	public BaseToolInstance CurrentTool { get; private set; }
	
	public ViewmodelManager ViewmodelManager;
	
	private bool _useDown;
	private bool _altUseDown;
	
	public void AttachToInputs(BaseToolInstance toolInstance) {
		CurrentTool = toolInstance;
		CurrentTool.OnEquip();
	}
	public void DetachFromInputs() {
		CurrentTool = null;
	}

	protected override void InitializeActionMap() {
		RegisterAction(_inputActions.Player.Attack, _ => OnUseDown(), OnUseUp);
		RegisterAction(_inputActions.Player.AttackSecondary, _ => OnAltUseDown(), OnAltUseUp);
	}

	private void OnEnable() {
		if (Instance != null) {
			Debug.LogError("Multiple ToolCore instances detected!");
			Destroy(this);
			return;
		}
		Instance = this;
	}

	private void Update() {
		if (CurrentTool == null) return;
		OnTick();
		if (_useDown) OnUseHold();
		if (_altUseDown) OnAltUseHold();
	}

	private void OnUseDown() {
		if (CurrentTool == null) return;
		_useDown = true;
		CurrentTool.OnUseDown();
	}
	private void OnUseUp() {
		if (CurrentTool == null) return;
		_useDown = false;
		CurrentTool.OnUseUp();
	}
	private void OnUseHold() {
		if (CurrentTool == null) return;
		CurrentTool.OnUseHold();
	}
	private void OnAltUseDown() {
		if (CurrentTool == null) return;
		_altUseDown = true;
		CurrentTool.OnAltUseDown();
	}
	private void OnAltUseUp() {
		if (CurrentTool == null) return;
		_altUseDown = false;
		CurrentTool.OnAltUseUp();
	}
	private void OnAltUseHold() {
		if (CurrentTool == null) return;
		CurrentTool.OnAltUseHold();
	}
	private void OnTick() {
		if (CurrentTool == null) return;
		CurrentTool.OnTick();
	}
}