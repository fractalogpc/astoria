using System;
using UnityEngine;

public class ToolCore : InputHandlerBase
{
	public static ToolCore Instance; // Singleton
	public BaseToolInstance CurrentTool { get; private set; }
	
	[SerializeField] private ViewmodelManager _viewmodelManager;
	
	private bool _useDown;
	private bool _altUseDown;
	
	public void EquipTool(BaseToolInstance toolInstance) {
		toolInstance.Initialize(this, _viewmodelManager);
		_viewmodelManager.SetViewmodelFor(toolInstance);
		CurrentTool = toolInstance;
		CurrentTool.OnEquip();
	}
	public void UnequipTool() {
		CurrentTool.OnUnequip();
		CurrentTool = null;
	}

	protected override void InitializeActionMap() {
		RegisterAction(_inputActions.Player.Attack, _ => OnUseDown(), OnUseUp);
		RegisterAction(_inputActions.Player.AttackSecondary, _ => OnAltUseDown(), OnAltUseUp);
	}

	private void Start() {
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
		_useDown = true;
		CurrentTool.OnUseDown();
	}
	private void OnUseUp() {
		_useDown = false;
		CurrentTool.OnUseUp();
	}
	private void OnUseHold() {
		CurrentTool.OnUseHold();
	}
	private void OnAltUseDown() {
		_altUseDown = true;
		CurrentTool.OnAltUseDown();
	}
	private void OnAltUseUp() {
		_altUseDown = false;
		CurrentTool.OnAltUseUp();
	}
	private void OnAltUseHold() {
		CurrentTool.OnAltUseHold();
	}
	private void OnTick() {
		CurrentTool.OnTick();
	}
}