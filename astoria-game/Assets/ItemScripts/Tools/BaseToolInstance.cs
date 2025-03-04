using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class BaseToolInstance : ViewmodelItemInstance
{
	protected ToolCore _toolCore;
	protected new ViewmodelManager _viewmodelManager;
	
	public BaseToolInstance(ItemData itemData) : base(itemData) {
		Debug.Log("Toolcore assigned");
		_toolCore = ToolCore.Instance;
	}

	public virtual void Initialize(ToolCore toolCore, ViewmodelManager viewmodelManager) {
		_viewmodelManager = viewmodelManager;
	}

	public override void OnHotbarSelected() {
		base.OnHotbarSelected();
		Debug.Log(_toolCore == null);
		_toolCore.EquipTool(this);
	}
	public override void OnHotbarDeselected() {
		base.OnHotbarDeselected();
		_toolCore.UnequipTool();
	}

	public virtual void OnEquip() {
		
	}
	public virtual void OnUnequip() {
		
	}

	public virtual void OnTick() {
		
	}
	public virtual void OnUseDown() {
		
	}
	public virtual void OnUseUp() {
		
	}
	public virtual void OnUseHold() {
		
	}
	public virtual void OnAltUseDown() {
		
	}
	public virtual void OnAltUseUp() {
		
	}
	public virtual void OnAltUseHold() {
		
	}
}
